using Controllers;
using Controllers.Characters.Vergen;
using Controllers.StateMachine;

namespace Controllers.Characters.Vergen.States
{
    public class VergenIdleState : State
    {
        private VergenController vController;

        public VergenIdleState(BaseController controller, ControllerStateMachine stateMachine, string animBoolName,
            VergenController vController)
            : base(controller, stateMachine, animBoolName)
        {
            this.vController = vController;
        }

        public override void Enter()
        {
            base.Enter();
            vController.Flip();
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void UpdateState()
        {
            base.UpdateState();
        }

        public override void UpdatePhysics()
        {
            base.UpdatePhysics();
        }

        public void TriggerCombat()
        {
            stateMachine.ChangeState(null);
        }
    }
}