using Controllers.StateMachine;

namespace Controllers.States
{
    public class Pepe_IdleState : State
    {
        private PepeController pController;
        
        
        public Pepe_IdleState(BaseController controller, ControllerStateMachine stateMachine, string animBoolName, PepeController pController) 
            : base(controller, stateMachine, animBoolName)
        {
            this.pController = pController;
        }

        public override void Enter()
        {
            base.Enter();
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
    }
}