using System.Collections;
using System.Collections.Generic;
using Controllers;
using Controllers.Froggy;
using Controllers.StateMachine;
using Controllers.StateMachine.States;
using Controllers.StateMachine.States.Data;
using UnityEngine;

public class OLC_AttackState : AttackState
{
    private OLC_AttackState stateData;
    private OLC_Controller olcController;

    private bool isDetectingGround;
    
    private bool jumping;
    private float lastJumpTime = float.NegativeInfinity;

    public OLC_AttackState(BaseController controller, ControllerStateMachine stateMachine, string animBoolName,
        AttackStateData stateData, OLC_Controller olcController)
        : base(controller, stateMachine, animBoolName, stateData)
    {
        this.olcController = olcController;
    }

    public override void UpdateState()
    {
        LookAtPlayer();
        
        if(!jumping) {
            controller.GetAnimator().SetBool(animBoolName, true);
            controller.AddForce(new Vector2(12f, 9f), true);
            lastJumpTime = Time.time + 1.25f;
            jumping = true;
        }
        
        if(Time.time > lastJumpTime)
        {
            jumping = false;
        }
        
        // OnLand
        if (!isDetectingGround && controller.CheckGround())
        {
            Debug.Log("SupeprFroggy Landed");
            controller.SetVelocity(0f);
            //AudioSource.PlayClipAtPoint(jumpStateData.landSFX, controller.GetTransfrom().position);
            controller.GetAnimator().SetBool(animBoolName, false);
        }
            
            
        isDetectingGround = controller.CheckGround();
    }

    public override void Enter()
    {
        base.Enter();
        olcController.ShootPlumas();
        controller.GetAnimator().SetBool(animBoolName, true);
        isDetectingGround = controller.CheckGround();
    }

    public override void Exit()
    {
        base.Exit();
    }
    
    protected virtual void LookAtPlayer()
    {
        float playerPositionX = GameManager.Instance.GetPlayerTransform().position.x;
                
        if ((controller.transform.position.x < playerPositionX && controller.FacingDirection == -1) ||
            (controller.transform.position.x > playerPositionX && controller.FacingDirection == 1))
            controller.Flip();
    }
}