using Controllers.Movement;
using Controllers.StateMachine;
using UnityEngine;

namespace Controllers.Characters.Zanopiano.States
{
    public class Zanopiano_DeadState : State
    {
        private ZanopianoController zController;

        private bool itemsgiven;

        public Zanopiano_DeadState(BaseController controller, ControllerStateMachine stateMachine, string animBoolName,
            ZanopianoController zController)
            : base(controller, stateMachine, animBoolName)
        {
            this.zController = zController;
        }

        public override void Enter()
        {
            base.Enter();
            
            if (!itemsgiven)
            {
                zController.DropItems();
                itemsgiven = true;
            }
        }

        public override void UpdateState()
        {
            base.UpdateState();
            controller.GetAnimator().SetBool("Alerting", false);
            controller.GetAnimator().SetBool("Alerting_Completed", false);
            controller.GetAnimator().SetBool("Attacking", false);
            controller.GetAnimator().SetBool("Attacking_Cooldown", false);
            controller.GetAnimator().SetBool(animBoolName, true);
            zController.GetMovementController<BaseMovementController>()
                .Speed = 0f;
        }
    }
}