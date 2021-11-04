using Controllers;
using Controllers.StateMachine;
using Controllers.StateMachine.States;
using Controllers.StateMachine.States.Data;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Controllers.Characters.SuperFroggy.States
{
    public class SuperFroggyNearAttackState : State
    {
        private SuperFroggyController froggyController;

        protected JumpStateData jumpStateData;

        private UnityAction onLandAction;
        
        private float lastJumpTime = float.NegativeInfinity;
        private float attackCooldown = float.NegativeInfinity;
        private float jumpCooldownTime;

        protected bool isDetectingGround;

        public SuperFroggyNearAttackState(BaseController controller, ControllerStateMachine stateMachine, string animBoolName, JumpStateData jumpStateData, SuperFroggyController froggyController)
            : base(controller, stateMachine, animBoolName)
        {
            this.froggyController = froggyController;
            this.jumpStateData = jumpStateData;
            
            onLandAction += () =>
            {
                if (!controller.IsDead())
                {
                    GameManager.Instance.GetSoundManager().PlaySoundAtLocation(jumpStateData.landSFX, controller.transform.position);
                }
                controller.GetAnimator().SetBool(animBoolName, false);
            };
        }

        public override void Enter()
        {
            base.Enter();
            controller.OnLandEvent.AddListener(onLandAction);
            GameManager.Instance.GetSoundManager().PlaySoundAtLocation(jumpStateData.jumpSFX, controller.transform.position);
        }

        public override void Exit()
        {
            base.Exit();
            controller.OnLandEvent.RemoveListener(onLandAction);
            controller.GetAnimator().SetBool(animBoolName, false);
        }

        public override void UpdateState()
        {
            if (controller.CheckPlayerInLongRange())
            {
                Debug.Log("LONG RANGE DETECTED");
                froggyController.StateMachine.ChangeState(froggyController._longAttackState);
            }
            
            LookAtPlayer();
            
            if (Time.time >= lastJumpTime)
            {
                controller.GetAnimator().SetBool(animBoolName, true);
                controller.AddForce(jumpStateData.jumpingForce, true);
                lastJumpTime = Time.time + 0.8f;
            }

            if (!controller.CheckGround() && Time.time >= attackCooldown)
            {
                controller.CheckTouchDamage();
                attackCooldown = Time.time + 0.8f;
            }
        }

        private void CheckForFlip()
        {
            if (controller.CheckWall() || !controller.CheckLedge())
                controller.Flip();
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