using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Controllers.StateMachine
{
    /// <summary>
    /// Controla el estado de un enemigo o NPC, desde que spawnea hasta que muere.
    /// Casi todos los metodos son marcados como 'virtual', ya que se implementan en cada PJ.
    /// </summary>
    public class ControllerStateMachine
    {
        public State CurrentState { get; private set; }

        /// <summary>
        /// Setea el <paramref name="currentState"/> al startState e invoca el metodo Enter de dicho estado.
        /// </summary>
        /// <param name="startState">El estado por defecto.</param>
        public void Initialize(State startState)
        {
            CurrentState = startState;
            CurrentState.Enter();
        }

        public void ChangeState(State nextState)
        {
            CurrentState.Exit();
            CurrentState = nextState;
            CurrentState.Enter();
        }
    }
}