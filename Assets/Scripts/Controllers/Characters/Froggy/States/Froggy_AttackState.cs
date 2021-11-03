using System;
using Controllers.StateMachine;
using Controllers.StateMachine.States;
using Controllers.StateMachine.States.Data;
using UnityEngine;
using UnityEngine.Events;

namespace Controllers.Froggy
{
    public class Froggy_AttackState : AttackState
    {
        public bool attackStarted;
        
        private FroggyController froggyController;
        private float attackCooldown = float.NegativeInfinity;
        private bool attacking;
        private bool tongueFinished;

        public Froggy_AttackState(BaseController controller, ControllerStateMachine stateMachine, string animBoolName, AttackStateData stateData, FroggyController froggyController) 
            : base(controller, stateMachine, animBoolName, stateData)
        {
            this.froggyController = froggyController;
        }

        public override void Enter()
        {
            base.Enter();

            if (Time.time >= attackCooldown)
            {
                froggyController.SpecialAttack();
                controller.GetAnimator().SetBool("IsReadyToAttack", true);
                attackCooldown = Time.time + 3f;
            }
            else
                stateMachine.ChangeState(froggyController._idleState);
        }

        public override void Exit()
        {
            base.Exit();
            attackStarted = false;
            attacking = false;
            tongueFinished = false;
        }

        public override void UpdateState()
        {
            //if (controller.controllerKind == EControllerKind.Boss && controller.currentHealth <= controller.ctrlData.maxHealth / 2)
            //{
            //    froggyController.EnterPhaseTwo();
            //}
            
            //if(controller.currentHealth <= 0)
            //    stateMachine.ChangeState(froggyController._deadState);
        }

        public void OnTongeFinished()
        {
            tongueFinished = true;
            controller.GetAnimator().SetBool("Attacking", false);
            stateMachine.ChangeState(froggyController._idleState);
        }
    }
}