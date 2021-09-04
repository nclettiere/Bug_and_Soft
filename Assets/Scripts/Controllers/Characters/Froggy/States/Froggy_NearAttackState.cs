using Controllers.StateMachine;
using Controllers.StateMachine.States;
using Controllers.StateMachine.States.Data;
using UnityEngine;

namespace Controllers.Froggy
{
    public class Froggy_NearAttackState : State
    {
        private FroggyController froggyController;
        
        protected JumpStateData jumpStateData;

        private float lastJumpTime = float.NegativeInfinity;
        private float jumpCooldownTime;
        
        protected bool isDetectingGround;
        
        public Froggy_NearAttackState(BaseController controller, ControllerStateMachine stateMachine, string animBoolName, JumpStateData jumpStateData, FroggyController froggyController) 
            : base(controller, stateMachine, animBoolName)
        {
            this.froggyController = froggyController;
            this.jumpStateData = jumpStateData;
        }

        public override void Enter()
        {
            base.Enter();

            jumpCooldownTime = 1f;
            isDetectingGround = controller.CheckGround();
        }

        public override void Exit()
        {
            base.Exit(); 
            lastJumpTime = float.NegativeInfinity;
            froggyController.GetAnimator().SetBool("NearAttackAlert", false);
            froggyController.GetAnimator().SetBool("Idle", true);
            froggyController.GetAnimator().SetBool("Jumping", false);
        }

        public override void UpdateState()
        {
            
            if (controller.currentHealth <= controller.ctrlData.maxHealth / 2)
            {
                froggyController.EnterPhaseTwo();
            }
            
            if (controller.CheckPlayerInLongRange())
            {
                stateMachine.ChangeState(froggyController._prepareAttackState);
            }

            LookAtPlayer();
            
            if (Time.time >= lastJumpTime)
            {
                controller.GetAnimator().SetBool(animBoolName, true);
                // SFX de saltar !!!
                AudioSource.PlayClipAtPoint(jumpStateData.jumpSFX, controller.GetTransfrom().position);

                controller.AddForce(new Vector2( 10f, 5f), true);

                lastJumpTime = Time.time + 1.25f;
            }else if ((lastJumpTime - Time.time) < jumpCooldownTime)
            {
                froggyController.GetAnimator().SetBool("NearAttackAlert", true);
            }
            else
            {
                if (!isDetectingGround)
                {
                    // Hace que danie al player si lo toca
                    controller.CheckTouchDamage();
                }
            }

            // OnLand
            if (!isDetectingGround && controller.CheckGround())
            {
                Debug.Log("SupeprFroggy Landed");
                controller.SetVelocity(0f);
                AudioSource.PlayClipAtPoint(jumpStateData.landSFX, controller.GetTransfrom().position);
                controller.GetAnimator().SetBool(animBoolName, false);
            }
            
            
            isDetectingGround = controller.CheckGround();
        }
        
        protected virtual void LookAtPlayer()
        {
            float playerPositionX = GameManager.Instance.GetPlayerTransform().position.x;
                
            if ((controller.transform.position.x < playerPositionX && controller.FacingDirection == -1) ||
                (controller.transform.position.x > playerPositionX && controller.FacingDirection == 1))
                controller.Flip();
        }
    }
}