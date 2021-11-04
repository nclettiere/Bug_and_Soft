using System.Collections;
using System.Collections.Generic;
using Controllers;
using Controllers.Froggy;
using Controllers.StateMachine;
using Controllers.StateMachine.States;
using Controllers.StateMachine.States.Data;
using UnityEngine;

public class Mortadelo_IdleState : State
{
    private MortadeloController mController;
    
    private float rotateSpeed = 3f;
    private float radio = 0.1f;
    private float angulo;

    public Mortadelo_IdleState(BaseController controller, ControllerStateMachine stateMachine, string animBoolName, MortadeloController mController)
        : base(controller, stateMachine, animBoolName)
    {
        this.mController = mController;
    }

    public override void UpdateState()
    {
        base.UpdateState();

        if (mController.currentHealth <= 0)
        {
            stateMachine.ChangeState(mController.DeadState);
        }

        CheckPlayerInRange();
        
        // (Aviso) seteo la pos Z a 0 pq a veces hay un que el mortadelo
        // se va al carajo en ese eje
        // Bugueadisima esta libreria no la recomiendo xd
        
        //if (mController.GetTransfrom().position.z != 0f)
        //{
        //    mController.GetTransfrom().position.Set(mController.GetTransfrom().position.x,
        //        mController.GetTransfrom().position.y, 0f);
        //}
    }

    public override void UpdatePhysics()
    {
        base.UpdatePhysics();

        MoviemeintoMortadelo();
    }

    private void CheckPlayerInRange()
    {
        RaycastHit2D hit = 
            Physics2D.CircleCast(mController.transform.position, 
                mController.ctrlData.playerNearRangeDistance, 
                Vector2.zero, 
                mController.ctrlData.whatIsPlayer);

        if (hit != null)
            if (hit.collider.transform.CompareTag("Player"))
                stateMachine.ChangeState(mController.AttackState);
    }

    private void MoviemeintoMortadelo()
    {
        // calculamos el angulo para rotar y el offset
        // se lo agregamos a la pos actual !!!
        angulo += rotateSpeed * Time.deltaTime;
        
        var offset = new Vector3(Mathf.Sin(angulo), Mathf.Cos(angulo), 0f) * radio;
        mController.transform.position += offset;
    }
}