using Controllers.StateMachine;
using Controllers.StateMachine.States;
using Controllers.StateMachine.States.Data;
using UnityEngine;

namespace Controllers.Froggy
{
    public class Froggy_AttackState : AttackState
    {
        private FroggyController froggyController;
        private float attackDuration;
        
        public Froggy_AttackState(BaseController controller, ControllerStateMachine stateMachine, string animBoolName, AttackStateData stateData, FroggyController froggyController) 
            : base(controller, stateMachine, animBoolName, stateData)
        {
            this.froggyController = froggyController;
        }

        public override void Enter()
        {
            base.Enter();
            attackDuration = startTime + stateData.duration;
        }

        public override void UpdateState()
        {
            if ((Time.time > attackDuration))
            {
                stateMachine.ChangeState(froggyController._idleState);
            }
        }
    }
}