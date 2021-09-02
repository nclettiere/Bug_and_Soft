using Controllers.StateMachine;
using Controllers.StateMachine.States;
using Controllers.StateMachine.States.Data;
using UnityEngine;

namespace Controllers.Characters.Mortadelo.States
{
    public class Mortadelo_AttackState : AttackState
    {
        private MortadeloController mController;

        private bool isDetectingGround;
        private bool rotated;
        private bool dashed;
        private float dashCooldown = float.NegativeInfinity;

        public Mortadelo_AttackState(BaseController controller, ControllerStateMachine stateMachine, string animBoolName,
            AttackStateData stateData, MortadeloController mController)
            : base(controller, stateMachine, animBoolName, stateData)
        {
            this.mController = mController;
        }

        public override void Enter()
        {
            base.Enter();

            mController.GetTransfrom().localScale = new Vector3(5, 5);
            mController.GetAnimator().SetBool(animBoolName, true);
        }


        public override void UpdateState()
        {
        }

        public override void UpdatePhysics()
        {
            base.UpdatePhysics();

            LookAtPlayer();
            CheckForDash();

            mController.SetTargetDestination(GameManager.Instance.GetPlayerTransform().position);
        }

        private void CheckForDash()
        {
            if (!dashed)
            {
                RaycastHit2D hit =
                    Physics2D.CircleCast(mController.transform.position,
                        1f,
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