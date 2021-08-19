using System.Collections;
using System.Collections.Generic;
using Controllers;
using Controllers.Froggy;
using Controllers.Froggy.States.Data;
using Controllers.StateMachine;
using Controllers.StateMachine.States;
using Controllers.StateMachine.States.Data;
using UnityEngine;

public class OLC_SecondPhase : AttackState
{
    private OLC_AttackStateData olcStateData;
    private OLC_Controller olcController;

    private bool isDetectingGround;

    private GameObject projectile;
    private OLC_Projectile projectileScript;
    
    private bool jumping;
    private bool arrowsShot;
    private float secondPhaseAttackWaitTime;
    private float plumasAttackWaitTime;
    private float projectileCD = float.NegativeInfinity;
    private int numberOfShoots = 1;
    private int currentNumberOfShoots = 1;

    public OLC_SecondPhase(BaseController controller, ControllerStateMachine stateMachine, string animBoolName,
        AttackStateData stateData, OLC_AttackStateData olcStateData, OLC_Controller olcController)
        : base(controller, stateMachine, animBoolName, stateData)
    {
        this.olcController = olcController;
        this.olcStateData = olcStateData;
    }

    public override void UpdateState()
    {
        if (currentNumberOfShoots >= numberOfShoots)
        {
            stateMachine.ChangeState(olcController._attackState);   
        }
        
        if (Time.time > plumasAttackWaitTime)
        {
            arrowsShot = false;
        }

        // OnLand
        if (!isDetectingGround && controller.CheckGround())
        {
            controller.SetVelocity(0f);
            controller.GetAnimator().SetBool(animBoolName, false);
        }

        isDetectingGround = controller.CheckGround();
    }

    public override void UpdatePhysics()
    {
        base.UpdatePhysics();
        if (Time.time >= secondPhaseAttackWaitTime)
        {
            if (!arrowsShot && currentNumberOfShoots < numberOfShoots)
            {
                olcController.transform.position = olcController.secondPhasePos.position;
                olcController.rBody.gravityScale = 0.0f;
                olcController.rBody.velocity = new Vector2(0.0f, 0.0f);
                TriggerAttacks();
                plumasAttackWaitTime = Time.time + 3f;
                
                arrowsShot = true;
                currentNumberOfShoots++;
            }
        }
    }

    private void TriggerAttacks()
    {
        for (int i = 0; i < olcController.arrowsPos.Length; i++)
        {
            if (Time.time > projectileCD)
            {
                projectile = GameObject.Instantiate(olcStateData.projectile, olcController.arrowsPos[i].position, 
                    olcController.transform.rotation);
                projectileScript = projectile.GetComponent<OLC_Projectile>();
                projectileScript.FireProjectile(30f, 25f, true);
            }
        }
        
        projectileCD = Time.time + .85f;
    }

    public override void Enter()
    {
        base.Enter();
        controller.GetAnimator().SetBool(animBoolName, true);
        secondPhaseAttackWaitTime = Time.time + 0.15f;
        isDetectingGround = controller.CheckGround();
        olcController.rBody.gravityScale = 0.0f;
        olcController.rBody.velocity = new Vector2(0.0f, 0.0f);
        numberOfShoots = Random.Range(1, 4);
        GameManager.Instance.ShowBossHealth("Odinn's Lost Crow - Phase 2", olcController);
        Vector2 camOffs = GameManager.Instance.GetCameraOffset();
        GameManager.Instance.SetCameraOffset(camOffs + new Vector2(0f, 3f));
    }

    public override void Exit()
    {
        base.Exit();
        Vector2 camOffs = GameManager.Instance.GetCameraOffset();
        GameManager.Instance.SetCameraOffset(camOffs - new Vector2(0f, 3f));
        olcController.rBody.gravityScale = 8.0f;
    }
    
    protected virtual void LookAtPlayer()
    {
        float playerPositionX = GameManager.Instance.GetPlayerTransform().position.x;
                
        if ((controller.transform.position.x < playerPositionX && controller.FacingDirection == -1) ||
            (controller.transform.position.x > playerPositionX && controller.FacingDirection == 1))
            controller.Flip();
    }
}