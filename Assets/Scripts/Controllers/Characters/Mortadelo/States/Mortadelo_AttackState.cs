using Controllers.Damage;
using Controllers.StateMachine;
using Controllers.StateMachine.States;
using Controllers.StateMachine.States.Data;
using Player;
using UnityEngine;

namespace Controllers.Characters.Mortadelo.States
{
    public class Mortadelo_AttackState : AttackState
    {
        private MortadeloController mController;

        private bool isDetectingGround;
        private bool rotated;
        private bool dashed;
        private bool attack;
        private float dashCooldown = float.NegativeInfinity;
        private float attackCooldown = float.NegativeInfinity;

        public Mortadelo_AttackState(BaseController controller, ControllerStateMachine stateMachine, string animBoolName,
            AttackStateData stateData, MortadeloController mController)
            : base(controller, stateMachine, animBoolName, stateData)
        {
            this.mController = mController;
        }

        public override void Enter()
        {
            base.Enter();

            mController.GetTransfrom().localScale = new Vector3(0.8f, 0.8f);
            mController.GetAnimator().SetBool(animBoolName, true);
            mController.SetTargetDestination();
            LookAtPlayer();
            //DashNow();
        }


        public override void UpdateState()
        {
            if (mController.currentHealth <= 0)
            {
                stateMachine.ChangeState(mController.DeadState);
            }
            
            //CheckForDash();
            CheckForDamage();
        }

        public override void UpdatePhysics()
        {
            base.UpdatePhysics();
            LookAtPlayer();
            mController.SetTargetDestination();
        }

        private void CheckForDash()
        {
            if (!dashed)
            {
                RaycastHit2D hit =
                    Physics2D.CircleCast(mController.transform.position,
                        3.5f,
                        Vector2.zero,
                        mController.ctrlData.whatIsPlayer);

                if (hit != null)
                {
                    DashNow();
                    dashCooldown = Time.time + 1f;
                }
            }
            
            if (Time.time >= dashCooldown)
            {
                dashed = false;
            }
        }

        private void CheckForDamage()
        {
            if (!attack)
            {
                Collider2D hit =
                    Physics2D.OverlapCircle(mController.transform.position,
                        1f,
                        mController.ctrlData.whatIsPlayer);
                if (hit != null)
                {
                    PlayerController bctrl = hit.transform.GetComponent<PlayerController>();
                    if (bctrl != null && (bctrl is IDamageable))
                    {
                        var dInfo = new DamageInfo(18, mController.GetTransfrom().position.x,
                            true, false, false);
                        bctrl.Damage(mController, dInfo);
                        GameManager.Instance.ApplyBlindness(5f);

                        attack = true;
                        attackCooldown = Time.time + 1.5f;
                    }
                }
            }
            
            if (Time.time >= attackCooldown)
            {
                attack = false;
            }
        }

        private void DashNow()
        {
            Vector2 dir = (Vector2)(GameManager.Instance.GetPlayerTransform().position) - (Vector2)(controller.transform.position);
            dir.Normalize();
            controller.rBody.AddForce(dir * 1000, ForceMode2D.Force);
            dashed = true;
        }

        private void LookAtPlayer()
        {
            float playerPositionX = GameManager.Instance.GetPlayerTransform().position.x;
                
            if ((controller.transform.position.x < playerPositionX && controller.FacingDirection == -1) ||
                (controller.transform.position.x > playerPositionX && controller.FacingDirection == 1))
                controller.Flip();
        }
    }
}