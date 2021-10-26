using Controllers;
using Controllers.Characters.Vergen;
using Controllers.StateMachine;

namespace a
{
    public class VergenIdleState : State
    {
        private VergenController vController;
        
        public VergenIdleState(BaseController controller, ControllerStateMachine stateMachine, string animBoolName, VergenController vController) 
            : base(controller, stateMachine, animBoolName)
        {
            this.vController = vController;
        }
        
        
    }
}