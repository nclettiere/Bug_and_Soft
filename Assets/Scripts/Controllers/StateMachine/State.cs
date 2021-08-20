using UnityEngine;

namespace Controllers.StateMachine
{
    public class State
    {
        protected ControllerStateMachine stateMachine;
        protected BaseController controller;
        protected float startTime;
        protected string animBoolName;
        
        public State(BaseController controller, ControllerStateMachine stateMachine, string animBoolName)
        {
            this.stateMachine = stateMachine;
            this.controller = controller;
            this.animBoolName = animBoolName;
        }
        
        public virtual void Enter()
        { 
            startTime = Time.time;
        }

        public virtual void Exit()
        {   
            controller.GetAnimator().SetBool(animBoolName, false);
        }

        public virtual void UpdateState()
        {
        }

        public virtual void UpdatePhysics()
        {
        }
    }
}