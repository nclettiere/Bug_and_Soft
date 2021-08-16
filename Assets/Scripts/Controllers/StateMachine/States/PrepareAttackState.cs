using UnityEngine;
using Controllers.StateMachine.States.Data;

namespace Controllers.StateMachine.States
{
    public class PrepareAttackState : State
    {
        protected PrepareAttackStateData stateData;
        protected float attackCooldown;
        protected float waitTimetoInitAttack = 0.5f;
        protected float attackWaitTime = float.NegativeInfinity;
        protected bool attackRequested = false;

        public PrepareAttackState(BaseController controller, ControllerStateMachine stateMachine, string animBoolName, PrepareAttackStateData stateData) 
            : base(controller, stateMachine, animBoolName)
        {
            this.stateData = stateData;
        }

        public override void Enter()
        {
            base.Enter();
        }

        public override void Exit()
        {
            base.Exit();
            attackWaitTime = float.NegativeInfinity;
            attackRequested = false;
            controller.IsReadyToAttack = false;
            controller.GetAnimator().SetBool("IsReadyToAttack", false);
            controller.GetAnimator().SetBool("IsReadyToAttack", false);
        }

        public override void UpdateState()
        {
            base.UpdateState();
            
            if (controller.CheckPlayerInLongRange())
            {
                LookAtPlayer();
                
                controller.GetAnimator().SetBool(animBoolName, true);

                if (controller.IsReadyToAttack)
                {
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
                            controller.GetAnimator().SetBool("Attacking", true);
                        }
                    }
                }

                attackCooldown = Time.time + 2f;
            }
            else
            {
                LookAtPlayer();
            }
        }

        public override void UpdatePhysics()
        {
            base.UpdatePhysics();
        }

        protected virtual void LookAtPlayer()
        {
            if (stateData.followPlayerDirection)
            {
                float playerPositionX = GameManager.Instance.GetPlayerTransform().position.x;
                    
                if ((controller.transform.position.x < playerPositionX && controller.FacingDirection == -1) ||
                    (controller.transform.position.x > playerPositionX && controller.FacingDirection == 1))
                    controller.Flip();
            }
        }
    }
}