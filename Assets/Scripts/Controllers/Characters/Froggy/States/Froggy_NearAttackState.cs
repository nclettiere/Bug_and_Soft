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
        }

        public override void UpdateState()
        {
            if (controller.CheckPlayerInLongRange())
            {
            }
            
            LookAtPlayer();
            
            if (Time.time >= lastJumpTime)
            {
                controller.GetAnimator().SetBool(animBoolName, true);
                // SFX de saltar !!!
                AudioSource.PlayClipAtPoint(jumpStateData.jumpSFX, controller.GetTransfrom().position);

                controller.AddForce(jumpStateData.jumpingForce, true);

                lastJumpTime = Time.time + jumpCooldownTime;
            }
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