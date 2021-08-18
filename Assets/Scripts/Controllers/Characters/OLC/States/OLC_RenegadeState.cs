using System.Collections;
using System.Collections.Generic;
using Controllers;
using Controllers.Froggy;
using Controllers.StateMachine;
using Controllers.StateMachine.States;
using Controllers.StateMachine.States.Data;
using UnityEngine;

public class OLC_RenegadeState : State
{
    private OLC_Controller olcController;

    private float renegadeTime;
    
    public OLC_RenegadeState(BaseController controller, ControllerStateMachine stateMachine, string animBoolName, OLC_Controller olcController)
        : base(controller, stateMachine, animBoolName)
    {
        this.olcController = olcController;
    }

    public override void UpdateState()
    {
        if(Time.time > renegadeTime)
            stateMachine.ChangeState(olcController._attackState);
    }

    public override void Enter()
    {
        base.Enter();
        SetRandomRenegadeTime();
        controller.GetAnimator().SetBool(animBoolName, true);
    }

    public override void Exit()
    {
        base.Exit();
    }
    
    public void SetRandomRenegadeTime()
    {
        renegadeTime = Time.time + Random.Range(2f, 3f);
    }
}