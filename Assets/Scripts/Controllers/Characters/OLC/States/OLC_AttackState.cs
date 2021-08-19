using System.Collections;
using System.Collections.Generic;
using Controllers;
using Controllers.Froggy;
using Controllers.Froggy.States.Data;
using Controllers.StateMachine;
using Controllers.StateMachine.States;
using Controllers.StateMachine.States.Data;
using UnityEngine;

public class OLC_AttackState : AttackState
{
    private OLC_AttackStateData olcStateData;
    private OLC_Controller olcController;

    private bool isDetectingGround;

    private GameObject projectile;
    private OLC_Projectile projectileScript;
    
    private bool jumping;
    private float lastJumpTime = float.NegativeInfinity;
    private float projectileCD = float.NegativeInfinity;

    public OLC_AttackState(BaseController controller, ControllerStateMachine stateMachine, string animBoolName,
        AttackStateData stateData, OLC_AttackStateData olcStateData, OLC_Controller olcController)
        : base(controller, stateMachine, animBoolName, stateData)
    {
        this.olcController = olcController;
        this.olcStateData = olcStateData;
    }

    public override void UpdateState()
    {
        LookAtPlayer();
        
        if(!jumping) {
            controller.GetAnimator().SetBool(animBoolName, true);
            controller.AddForce(new Vector2(60f, 20f), true);

            TriggerAttack(Random.Range(1, 3));
            
            lastJumpTime = Time.time + 1.5f;
            jumping = true;
        }

        // OnLand
        if (!isDetectingGround && controller.CheckGround())
        {
            controller.SetVelocity(0f);
            //AudioSource.PlayClipAtPoint(jumpStateData.landSFX, controller.GetTransfrom().position);
            controller.GetAnimator().SetBool(animBoolName, false);
        }
        
        if(Time.time > lastJumpTime)
        {
            jumping = false;
        }
            
            
        isDetectingGround = controller.CheckGround();
    }

    private void TriggerAttack(int number)
    {
        for (int i = 0; i < number; i++)
        {
            if (Time.time > projectileCD)
            {
                projectile = GameObject.Instantiate(olcStateData.projectile, olcController.projectilePos.position + new Vector3(i, i),
                    olcController.transform.rotation);
                projectileScript = projectile.GetComponent<OLC_Projectile>();
                projectileScript.FireProjectile(30f, 25f);
                projectileCD = Time.time + .85f;
            }
        }
    }

    public override void Enter()
    {
        base.Enter();
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