using System.Collections;
using System.Collections.Generic;
using Controllers;
using Controllers.Froggy;
using Controllers.StateMachine;
using Controllers.StateMachine.States;
using Controllers.StateMachine.States.Data;
using UnityEngine;

public class Froggy_IdleState : IdleState
{
    private FroggyController _froggyController;
    
    public Froggy_IdleState(BaseController controller, ControllerStateMachine stateMachine, string animBoolName, IdleStateData stateData, FroggyController froggyController) 
        : base(controller, stateMachine, animBoolName, stateData)
    {
        _froggyController = froggyController;
    }

    public override void UpdateState()
    {
        base.UpdateState();

        if (isIdleTimeOver)
            stateMachine.ChangeState(_froggyController._jumpState);
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void UpdatePhysics()
    {
        base.UpdatePhysics();
    }
}
