using UnityEngine;
using Controllers.StateMachine.States.Data;

namespace Controllers.StateMachine.States
{
    public class DeadState : State
    {
        private float deadWaitTime;
        private DeadStateData stateData;

        public DeadState(BaseController controller, ControllerStateMachine stateMachine, string animBoolName,
            DeadStateData stateData) : base(controller, stateMachine, animBoolName)
        {
            this.stateData = stateData;
        }

        public override void Enter()
        {
            base.Enter();

            controller.GetAnimator().SetBool(animBoolName, true);

            controller.rBody.constraints = RigidbodyConstraints2D.None;

            if (stateData.applyTorque)
            {
                controller.rBody.AddForce(new Vector2(3f * -controller.FacingDirection, 3f), ForceMode2D.Impulse);
                controller.rBody.AddTorque(10f * -controller.FacingDirection, ForceMode2D.Impulse);
            }

            deadWaitTime = Time.time + controller.deadMaxWait;
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void UpdateState()
        {
            if (Time.time < deadWaitTime)
            {
                return;
            }

            if (stateData.showLapida)
            {
                controller.DestroyNow(stateData.lapida);
            }
            else
                controller.DestroyNow();
        }

        public override void UpdatePhysics()
        {
            base.UpdatePhysics();
        }
    }
}