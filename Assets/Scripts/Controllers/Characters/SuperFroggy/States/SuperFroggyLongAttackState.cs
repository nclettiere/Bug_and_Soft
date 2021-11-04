using Controllers;
using Controllers.StateMachine;
using Controllers.StateMachine.States;
using Controllers.StateMachine.States.Data;
using UnityEngine;
using UnityEngine.Events;

namespace Controllers.Characters.SuperFroggy.States
{
    public class SuperFroggyLongAttackState : State
    {
        private SuperFroggyController froggyController;
        private float attackCooldown = float.NegativeInfinity;
        private bool attacking;
        private bool tongueFinished = true;
        public bool attackStarted;

        public SuperFroggyLongAttackState(BaseController controller, ControllerStateMachine stateMachine, string animBoolName, SuperFroggyController froggyController) 
            : base(controller, stateMachine, animBoolName)
        {
            this.froggyController = froggyController;
        }

        public override void Enter()
        {
            base.Enter();
            
        }

        public override void Exit()
        {
            base.Exit();
            attacking = false;
            tongueFinished = true;
        }

        public override void UpdateState()
        {
            if (controller.CheckPlayerInNearRange() && tongueFinished)
            {
                Debug.Log("NEAR RANGE DETECTED");
                froggyController.StateMachine.ChangeState(froggyController._nearAttackState);
            }
            
            if (!controller.CheckPlayerInNearRange() && !controller.CheckPlayerInLongRange() && tongueFinished)
            {
                Debug.Log("NEAR RANGE DETECTED");
                froggyController.StateMachine.ChangeState(froggyController._nearAttackState);
            }
            
            if (Time.time >= attackCooldown)
            {
                controller.GetAnimator().SetBool("Attacking", true);
                tongueFinished = false;
                froggyController.SpecialAttack();
                attackCooldown = Time.time + 3f;
            }
            //else
            //    stateMachine.ChangeState(froggyController._idleState);
        }

        public void OnTongeFinished()
        {
            tongueFinished = true;
            controller.GetAnimator().SetBool("Attacking", false);
            //froggyController.StateMachine.ChangeState(froggyController._idleState);
        }
    }
}