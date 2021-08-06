using Controllers.StateMachine.States.Data;
using UnityEngine;

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
            controller.GetAnimator().SetBool(animBoolName, false);
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