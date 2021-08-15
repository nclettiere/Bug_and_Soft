﻿using UnityEngine;
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

            GameManager.Instance.AddPlayerKrowns(controller.KrownReward);

            controller.GetAnimator().SetBool(animBoolName, true);

            controller.rBody.constraints = RigidbodyConstraints2D.None;

            if (stateData.applyTorque)
            {
                controller.AddForce(new Vector2(3f * -controller.FacingDirection, 3f), true);
                controller.AddTorque(10f * -controller.FacingDirection, true);
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