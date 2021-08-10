using System.Collections;
using System.Collections.Generic;
using Controllers;
using Controllers.Froggy;
using Controllers.StateMachine;
using Controllers.StateMachine.States;
using Controllers.StateMachine.States.Data;
using UnityEngine;

public class Froggy_DeadState : DeadState
{
    private FroggyController _froggyController;

    public Froggy_DeadState(BaseController controller, ControllerStateMachine stateMachine, string animBoolName,
        DeadStateData stateData, FroggyController froggyController)
        : base(controller, stateMachine, animBoolName, stateData)
    {
        _froggyController = froggyController;
    }
}