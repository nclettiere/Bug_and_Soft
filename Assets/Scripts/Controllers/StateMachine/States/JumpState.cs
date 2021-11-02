using Controllers.Froggy;
using UnityEngine;
using Controllers.StateMachine.States.Data;

namespace Controllers.StateMachine.States
{
    public class JumpState : State
    {
        private FroggyController froggyController;
        private JumpStateData stateData;
        private float lastJumpTime = float.NegativeInfinity;


        public JumpState(BaseController controller, ControllerStateMachine stateMachine, string animBoolName,
            JumpStateData stateData, FroggyController froggyController)
            : base(controller, stateMachine, animBoolName)
        {
            this.stateData = stateData;
            this.froggyController = froggyController;
        }

        public override void Enter()
        {
            base.Enter();
            
            controller.OnLandEvent.AddListener(() =>
            {
                if (!controller.IsDead())
                {
                    //AudioSource.PlayClipAtPoint(stateData.landSFX, controller.GetTransfrom().position);
                    GameManager.Instance.GetSoundManager().PlaySoundAtLocation(stateData.landSFX, controller.transform.position);
                    CheckForFlip();
                    stateMachine.ChangeState(froggyController._idleState);
                }
            });
            
            controller.GetAnimator().SetBool(animBoolName, true);
            
            GameManager.Instance.GetSoundManager().PlaySoundAtLocation(stateData.jumpSFX, controller.transform.position);
            //AudioSource.PlayClipAtPoint(stateData.jumpSFX, controller.GetTransfrom().position);
            controller.AddForce(stateData.jumpingForce, true);
        }

        public override void Exit()
        {
            base.Exit();
            controller.GetAnimator().SetBool(animBoolName, false);
        }

        private void CheckForFlip()
        {
            if (controller.CheckWall() || !controller.CheckLedge())
                controller.Flip();
        }
    }
}