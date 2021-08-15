using Controllers.Froggy;
using UnityEngine;
using Controllers.StateMachine.States.Data;

namespace Controllers.StateMachine.States
{
    //TODO: Change this class to FroggyJumpState and create a base class JumpState for general use. (public JumpState(BaseController controller, ControllerStateMachine stateMachine, string animBoolName, JumpStateData stateData, FroggyController froggyController) )
    public class JumpState : State
    {
        private FroggyController froggyController;

        protected JumpStateData stateData;

        private float jumpCooldownTime;
        private float lastJumpTime = float.NegativeInfinity;

        protected bool isDetectingWall;
        protected bool isDetectingLedge;
        protected bool isDetectingGround;

        public JumpState(BaseController controller, ControllerStateMachine stateMachine, string animBoolName,
            JumpStateData stateData, FroggyController froggyController)
            : base(controller, stateMachine, animBoolName)
        {
            this.stateData = stateData;
            this.froggyController = froggyController;
        }

        public override void Enter()
        {
            startTime = Time.time;

            jumpCooldownTime = Random.Range(stateData.jumpingCooldownRandomRangeFrom,
                stateData.jumpingCooldownRandomRangeTo);

            isDetectingWall = controller.CheckWall();
            isDetectingLedge = controller.CheckLedge();
            isDetectingGround = controller.CheckGround();
        }

        public override void Exit()
        {
            base.Exit();

            controller.GetAnimator().SetBool(animBoolName, false);
        }

        public override void UpdateState()
        {
            base.UpdateState();

            if (controller.CheckPlayerInRange())
                stateMachine.ChangeState(froggyController._prepareAttackState);

            if (Time.time >= lastJumpTime)
            {
                controller.GetAnimator().SetBool(animBoolName, true);
                // SFX de saltar !!!
                AudioSource.PlayClipAtPoint(stateData.jumpSFX, controller.GetTransfrom().position);

                controller.AddForce(stateData.jumpingForce, true);

                lastJumpTime = Time.time + jumpCooldownTime;
            }

            // Ledge or Wall detected !
            if (controller.CheckWall() || !controller.CheckLedge())
            {
                controller.Flip();
                stateMachine.ChangeState(froggyController._idleState);
            }

            // OnLand
            if (!isDetectingGround && controller.CheckGround())
            {
                AudioSource.PlayClipAtPoint(stateData.landSFX, controller.GetTransfrom().position);
                controller.GetAnimator().SetBool(animBoolName, false);
            }

            isDetectingWall = controller.CheckWall();
            isDetectingLedge = controller.CheckLedge();
            isDetectingGround = controller.CheckGround();
        }
    }
}