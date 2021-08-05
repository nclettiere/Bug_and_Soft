using Controllers.StateMachine.States.Data;

namespace Controllers.StateMachine.States
{
    public class AttackState : State
    {
        protected AttackStateData stateData;

        public AttackState(BaseController controller, ControllerStateMachine stateMachine, string animBoolName, AttackStateData stateData) 
            : base(controller, stateMachine, animBoolName)
        {
            this.stateData = stateData;
        }

        public override void Enter()
        {
            base.Enter();
            controller.GetAnimator().SetBool(animBoolName, true);
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