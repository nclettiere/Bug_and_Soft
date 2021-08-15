using Controllers.StateMachine.States.Data;
using UnityEngine;

namespace Controllers.StateMachine.States
{
    public class DamageState : State
    {
        private float damageWaitTime;
        private DamageStateData stateData;

        public DamageState(BaseController controller, ControllerStateMachine stateMachine, string animBoolName,
            DamageStateData stateData) : base(controller, stateMachine, animBoolName)
        {
            this.stateData = stateData;
        }

        public override void Enter()
        {
            base.Enter();

            controller.GetAnimator().SetBool(animBoolName, true);
            damageWaitTime = Time.time + Random.Range(0.5f, 0.8f);
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void UpdateState()
        {
            if (Time.time < damageWaitTime)
                return;
        }

        public override void UpdatePhysics()
        {
            base.UpdatePhysics();
        }
    }
}