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
            controller.DestroyNow();
            base.UpdateState();

            if (Time.time < deadWaitTime)
            {
                Debug.Log("Not in dead time");
                return;
            }

            if (stateData.showLapida)
            {
                Debug.Log("Waiting to ground");
                if (controller.groundDetected || controller.topDetected)
                    controller.DestroyNow(stateData.lapida);
            }
            else
                controller.DestroyNow();


            Debug.Log("Final Reached");
        }

        public override void UpdatePhysics()
        {
            base.UpdatePhysics();
        }
    }
}