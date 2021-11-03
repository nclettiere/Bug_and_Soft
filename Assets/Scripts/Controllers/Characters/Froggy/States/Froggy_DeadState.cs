using System.Collections;
using System.Collections.Generic;
using Controllers;
using Controllers.Froggy;
using Controllers.StateMachine;
using Controllers.StateMachine.States;
using Controllers.StateMachine.States.Data;
using Items;
using UnityEngine;

public class Froggy_DeadState : DeadState
{
    private FroggyController _froggyController;

    private float enterPhaseTwoWaitTime = float.NegativeInfinity;

    public Froggy_DeadState(BaseController controller, ControllerStateMachine stateMachine, string animBoolName,
        DeadStateData stateData, FroggyController froggyController)
        : base(controller, stateMachine, animBoolName, stateData)
    {
        _froggyController = froggyController;
    }
    
    
    public override void Enter()
    {
        _froggyController.DropItems();
        
        if (_froggyController.GetComponent<ItemGiver>() != null)
        {
            _froggyController.GetComponent<ItemGiver>().Run();
        }
        
        if (_froggyController.controllerKind != EControllerKind.Boss)
        {
            base.Enter();
        }
        else
        {
            GameManager.Instance.AddPlayerKrowns(controller.KrownReward);
            _froggyController.GetAnimator().SetBool("SuperFroggy_Transforming", true);
            enterPhaseTwoWaitTime = Time.time + 3f;
        }
    }

    public override void UpdateState()
    {
        if (_froggyController.controllerKind != EControllerKind.Boss)
        {
            base.UpdateState();
            return;
        }

        // SuperFroggy
        //LookAtPlayer();
        //if (Time.time > enterPhaseTwoWaitTime)
        //    _froggyController.Explode();
    }
        
    protected virtual void LookAtPlayer()
    {
        float playerPositionX = GameManager.Instance.GetPlayerTransform().position.x;
                
        if ((controller.transform.position.x < playerPositionX && controller.FacingDirection == -1) ||
            (controller.transform.position.x > playerPositionX && controller.FacingDirection == 1))
            controller.Flip();
    }
}