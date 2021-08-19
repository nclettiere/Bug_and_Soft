using System.Collections;
using System.Collections.Generic;
using Controllers;
using Controllers.Froggy;
using Controllers.StateMachine;
using Controllers.StateMachine.States;
using Controllers.StateMachine.States.Data;
using UnityEngine;

public class OLC_StunnedState : IdleState
{
    private OLC_Controller olcController;

    public OLC_StunnedState(BaseController controller, ControllerStateMachine stateMachine, string animBoolName,
        IdleStateData stateData, OLC_Controller olcController)
        : base(controller, stateMachine, animBoolName, stateData)
    {
        this.olcController = olcController;
    }

    public override void UpdateState()
    {
        base.UpdateState();

        if (isIdleTimeOver)
            stateMachine.ChangeState(olcController._attackState);
    }

    public override void Enter()
    {
        base.Enter();
        controller.GetAnimator().SetBool(animBoolName, true);
        idleTime = 1f;
    }

    public override void Exit()
    {
        base.Exit();
        controller.GetAnimator().SetBool(animBoolName, false);
    }
}