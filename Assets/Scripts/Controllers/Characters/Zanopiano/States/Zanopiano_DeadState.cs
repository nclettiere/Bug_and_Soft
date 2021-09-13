using Controllers.Movement;
using Controllers.StateMachine;
using UnityEngine;

namespace Controllers.Characters.Zanopiano.States
{
    public class Zanopiano_DeadState : State
    {
        private ZanopianoController zController;

        private bool alerting;

        public Zanopiano_DeadState(BaseController controller, ControllerStateMachine stateMachine, string animBoolName,
            ZanopianoController zController)
            : base(controller, stateMachine, animBoolName)
        {
            this.zController = zController;
        }

        public override void Enter()
        {
            base.Enter();
            controller.GetAnimator().SetBool(animBoolName, true);
        }

        public override void UpdateState()
        {
            base.UpdateState();

            zController.GetMovementController<BaseMovementController>()
                .Speed = 0f;
        }
    }
}