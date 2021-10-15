using UnityEngine;
using Controllers.StateMachine.States.Data;

namespace Controllers.StateMachine.States
{
    public class IdleState : State
    {
        private IdleStateData stateData;

        protected bool flipAfterIdle;
        protected bool isIdleTimeOver;
        protected float idleTime;
        protected float newIdleTime;
        
        public IdleState(BaseController controller, ControllerStateMachine stateMachine, string animBoolName, IdleStateData stateData) : base(controller, stateMachine, animBoolName)
        {
            this.stateData = stateData;
        }

        public override void Enter()
        {
            base.Enter();
            controller.SetVelocity(0f);
            isIdleTimeOver = false;
            SetRandomIdleTime();
        }

        public override void Exit()
        {
            base.Exit();
            
            if(flipAfterIdle)
                controller.Flip();
        }

        public override void UpdateState()
        {
            base.UpdateState();

            if (Time.time >= newIdleTime)
            {
                isIdleTimeOver = true;
                newIdleTime = Time.time + idleTime;
            }
        }

        public override void UpdatePhysics()
        {
            base.UpdatePhysics();
        }

        public void SetFlipAfterIdle(bool flip)
        {
            flipAfterIdle = flip;
        }

        public void SetRandomIdleTime()
        {
            newIdleTime = Random.Range(stateData.minIdleTime, stateData.maxIdleTime);
        }
    }
}