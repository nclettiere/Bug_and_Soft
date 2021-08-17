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
        private float nextAttackDuration;
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
            nextAttackDuration = Time.time + stateData.duration;

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
            if (controller.currentHealth <= controller.ctrlData.maxHealth / 2)
            {
                froggyController.EnterPhaseTwo();
            }
            if (attackStarted && !attacking)
            {
                froggyController.SpecialAttack();
                attacking = true;
            }
            
            if (tongueFinished)
            {
                // Animacion termino !!!
                stateMachine.ChangeState(froggyController._idleState);
            }
        }

        public void OnTongeFinished()
        {
            Debug.Log("OnTongeFinished CALLED");
            tongueFinished = true;
        }
    }
}