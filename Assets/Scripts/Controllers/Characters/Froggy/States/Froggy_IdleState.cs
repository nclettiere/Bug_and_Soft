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
    private FroggyController froggyController;

    public Froggy_IdleState(BaseController controller, ControllerStateMachine stateMachine, string animBoolName,
        IdleStateData stateData, FroggyController froggyController)
        : base(controller, stateMachine, animBoolName, stateData)
    {
        this.froggyController = froggyController;
    }

    public override void UpdateState()
    {
        if (controller.currentHealth <= controller.ctrlData.maxHealth / 2)
        {
            froggyController.EnterPhaseTwo();
        }
        
        if (controller.CheckPlayerInNearRange())
        {
            stateMachine.ChangeState(froggyController._nearAttackState);
        }        
        
        if (controller.CheckPlayerInLongRange())
        {
            stateMachine.ChangeState(froggyController._prepareAttackState);
        }

        if (isIdleTimeOver)
            stateMachine.ChangeState(froggyController._jumpState);
        
        base.UpdateState();
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }
}