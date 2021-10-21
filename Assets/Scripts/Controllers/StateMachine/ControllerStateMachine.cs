using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Controllers.StateMachine
{
    public class ControllerStateMachine
    {
        public State CurrentState { get; private set; }

        public void Initialize(State startState)
        {
            CurrentState = startState;
            CurrentState.Enter();
        }

        public void ChangeState(State nextState)
        {
            if (nextState == null)
            {
                CurrentState = null;
                return;
            }

            CurrentState?.Exit();
            CurrentState = nextState;
            CurrentState.Enter();
        }
    }
}