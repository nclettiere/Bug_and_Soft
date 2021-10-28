using System.Collections;
using Controllers.Damage;
using Controllers.Movement;
using Controllers.StateMachine;
using Player;
using UnityEngine;
using UnityEngine.Events;

namespace Controllers.Characters.Vergen.States
{
    public class VergenTorbeshinoState : State
    {
        private VergenController vController;

        //public UnityEvent OnVergenShootingSpears;
        //public int NumberOfSpears;
        //public int NumberOfSpearsShot;

        private float attackDuration;
        private float flipWait = float.NegativeInfinity;
        private bool spriteFlip;

        private float attackCooldown = float.NegativeInfinity;

        public VergenTorbeshinoState(BaseController controller, ControllerStateMachine stateMachine,
            string animBoolName,
            VergenController vController)
            : base(controller, stateMachine, animBoolName)
        {
            this.vController = vController;
        }

        public override void Enter()
        {
            base.Enter();

            controller.GetAnimator().SetBool(animBoolName, true);

            attackDuration = startTime + Random.Range(2f, 5f);
        }

        public override void UpdateState()
        {
            base.UpdateState();

            float playerPositionX = GameManager.Instance.GetPlayerTransform().position.x;
            int dir = controller.transform.position.x < playerPositionX ? 1 : -1;
            vController
                .GetMovementController<BaseMovementController>()
                .Move(dir, false);
        }

        public override void UpdatePhysics()
        {
            base.UpdatePhysics();

            if (Time.time <= attackDuration)
            {
                if (Time.time >= flipWait)
                {
                    spriteFlip = !spriteFlip;
                    controller.FlipSpriteX(spriteFlip);
                    flipWait = Time.time + 0.25f;
                }

                CheckForDamage();
            }
            else
            {
                spriteFlip = false;
                controller.FlipSpriteX(false);
                stateMachine.ChangeState(vController.vergenPhaseOneState);
            }
        }

        public override void Exit()
        {
            base.Exit();
            vController.GetAnimator().SetBool(animBoolName, false);
        }

        protected virtual void LookAtPlayer()
        {
            float playerPositionX = GameManager.Instance.GetPlayerTransform().position.x;
            if ((controller.transform.position.x < playerPositionX && controller.FacingDirection == -1) ||
                (controller.transform.position.x > playerPositionX && controller.FacingDirection == 1))
                controller.Flip();
        }

        private void CheckForDamage()
        {
            if (Time.time >= attackCooldown)
            {
                RaycastHit2D hit = Physics2D.CircleCast(
                    controller.transform.position,
                    5,
                    controller.transform.right,
                    0,
                    controller.ctrlData.whatIsPlayer);

                if (hit != null && hit.transform != null)
                {
                    if (hit.transform.CompareTag("Player"))
                    {
                        DamageInfo dInfo = new DamageInfo(
                            10, controller.transform.position.x, true);
                        dInfo.MoveOnAttackForce = new Vector2(1000, 30);
                        hit.transform.GetComponent<PlayerController>()
                            .Damage(dInfo);
                        attackCooldown = Time.time + 1f;
                    }
                }
            }
        }
    }
}