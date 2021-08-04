using UnityEngine;

namespace Controllers.StateMachine
{
    /// <summary>
    /// Clase base para los estados de cada personaje.
    /// Controla el ciclo de vida de cada estado (Enter, Update y Exit)
    /// Esta clase esta pensada para que otras hereden de ella y la customizen a sus necesidades.
    /// </summary>
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
        
        /// <summary>
        /// Este metodo es ejecutado una vez cuando entra al estado.
        /// </summary>
        public virtual void Enter()
        { 
            startTime = Time.time;
        }
        
        /// <summary>
        /// Este metodo es ejecutado cuando se cambia de estado, solo una vez.
        /// </summary>
        public virtual void Exit()
        {   
            controller.GetAnimator().SetBool(animBoolName, false);
        }

        /// <summary>
        /// Ejecuta la logica principal del estado, es ejecutado cada frame.
        /// </summary>
        public virtual void UpdateState()
        {
        }
           
        /// <summary>
        /// Es ejecutado en FixedUpdate().
        /// </summary>
        public virtual void UpdatePhysics()
        {
        }
    }
}