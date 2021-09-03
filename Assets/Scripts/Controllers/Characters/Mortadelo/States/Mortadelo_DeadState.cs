using System.Collections;
using System.Collections.Generic;
using Controllers;
using Controllers.Froggy;
using Controllers.StateMachine;
using Controllers.StateMachine.States;
using Controllers.StateMachine.States.Data;
using UnityEngine;

public class Mortadelo_DeadState : DeadState
{
    private MortadeloController mController;

    private float enterPhaseTwoWaitTime = float.NegativeInfinity;

    public Mortadelo_DeadState(BaseController controller, ControllerStateMachine stateMachine, string animBoolName,
        DeadStateData stateData, MortadeloController mController)
        : base(controller, stateMachine, animBoolName, stateData)
    {
        this.mController = mController;
    }

    public override void Enter()
    {
        mController.rBody.gravityScale = 8f;
        base.Enter();
    }

    protected virtual void LookAtPlayer()
    {
        float playerPositionX = GameManager.Instance.GetPlayerTransform().position.x;
                
        if ((controller.transform.position.x < playerPositionX && controller.FacingDirection == -1) ||
            (controller.transform.position.x > playerPositionX && controller.FacingDirection == 1))
            controller.Flip();
    }
}