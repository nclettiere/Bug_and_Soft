using Controllers.StateMachine;
using Controllers.StateMachine.States;
using Controllers.StateMachine.States.Data;
using UnityEngine;

namespace Controllers.Froggy
{
    public class Froggy_PrepareAttackState : PrepareAttackState
    {
        private FroggyController froggyController;
        public Froggy_PrepareAttackState(BaseController controller, ControllerStateMachine stateMachine, string animBoolName, PrepareAttackStateData stateData, FroggyController froggyController) 
            : base(controller, stateMachine, animBoolName, stateData)
        {
            this.froggyController = froggyController;
        }

        public override void UpdateState()
        {
            //if(controller.currentHealth <= 0)
            //    stateMachine.ChangeState(froggyController._deadState);

            if (controller.CheckPlayerInNearRange())
                stateMachine.ChangeState(froggyController._nearAttackState);

            if (controller.CheckPlayerInLongRange())
            {
                LookAtPlayer();
                
                controller.GetAnimator().SetBool(animBoolName, true);

                    if (!attackRequested)
                    {
                        controller.GetAnimator().SetBool("IsReadyToAttack", true);
                        controller.GetAnimator().SetBool(animBoolName, false);

                        attackWaitTime = Time.time + waitTimetoInitAttack;
                        attackRequested = true;
                    }
                    else
                    {
                        if (Time.time >= attackWaitTime)
                        {
                            stateMachine.ChangeState(froggyController._attackState);
                        }
                    }

                attackCooldown = Time.time + 2f;
            }
            else
            {
                LookAtPlayer();
            }

            if (Time.time >= attackCooldown)
                stateMachine.ChangeState(froggyController._idleState);
        }
    }
}