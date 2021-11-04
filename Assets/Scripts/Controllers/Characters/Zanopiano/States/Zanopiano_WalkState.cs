using Controllers.Movement;
using Controllers.StateMachine;
using UnityEngine;

namespace Controllers.Characters.Zanopiano.States
{
    public class Zanopiano_WalkState : State
    {
        private ZanopianoController zController;

        private bool alerting;

        public Zanopiano_WalkState(BaseController controller, ControllerStateMachine stateMachine, string animBoolName,
            ZanopianoController zController)
            : base(controller, stateMachine, animBoolName)
        {
            this.zController = zController;
        }

        public override void Enter()
        {
            base.Enter();
            controller.GetAnimator().SetBool(animBoolName, true);
            alerting = false;
        }

        public override void UpdateState()
        {
            if (zController.currentHealth <= 0)
            {
                zController.StateMachine.ChangeState(zController.DeadState);
            }

            CheckearLedge();

            CheckLongRange();
            CheckNearRange();
        }

        private void CheckearLedge()
        {
            if (controller.CheckWall() || controller.CheckGround() && !controller.CheckLedge())
                controller.Flip();
        }

        private void CheckLongRange()
        {
            if (controller.CheckPlayerInLongRange())
            {
                if (!controller.GetAnimator().GetBool("Alerting_Completed"))
                {
                    controller.GetAnimator().SetBool("Alerting", true);
                    alerting = true;
                }
                else
                {
                    alerting = false;
                }
            }
            else
            {
                controller.GetAnimator().SetBool("Alerting", false);
                controller.GetAnimator().SetBool("Alerting_Completed", false);
                alerting = false;
            }
        }

        private void CheckNearRange()
        {
            if (controller.CheckPlayerInNearRange())
            {
                alerting = false;
                if (!controller.GetAnimator().GetBool("Attacking_Completed"))
                {
                    controller.GetAnimator().SetBool("Alerting_Completed", false);
                    controller.GetAnimator().SetBool("Alerting", false);
                    stateMachine.ChangeState(zController.AttackState);
                }
            }
        }

        public override void UpdatePhysics()
        {
            base.UpdatePhysics();

            if (!controller.CheckPlayerInLongRange() && !controller.CheckPlayerInNearRange())
            {
                controller.GetAnimator().SetBool("Alerting_Completed", false);
                controller.GetAnimator().SetBool("Alerting", false);
                controller.GetAnimator().SetBool("Attacking", false);

                controller.GetMovementController<BaseMovementController>()
                    .Move();
            }
        }

        public void AlertingFinished()
        {
            if (!controller.CheckPlayerInLongRange() && !controller.CheckPlayerInNearRange())
                return;

            controller.GetAnimator().SetBool("Alerting", false);
            controller.GetAnimator().SetBool("Alerting_Completed", true);
        }
    }
}