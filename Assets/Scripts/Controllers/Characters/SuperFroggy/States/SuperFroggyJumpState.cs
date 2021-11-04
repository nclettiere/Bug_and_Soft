using Controllers.StateMachine;
using Controllers.StateMachine.States.Data;
using UnityEngine;
using UnityEngine.Events;

namespace Controllers.Characters.SuperFroggy.States
{
    public class SuperFroggyJumpState : State
    {
        private SuperFroggyController froggyController;
        private JumpStateData stateData;
        private float lastJumpTime = float.NegativeInfinity;
        private UnityAction onLandAction;

        public SuperFroggyJumpState(BaseController controller, ControllerStateMachine stateMachine, string animBoolName,
            JumpStateData stateData, SuperFroggyController froggyController)
            : base(controller, stateMachine, animBoolName)
        {
            this.stateData = stateData;
            this.froggyController = froggyController;
            
            onLandAction += () =>
            {
                if (!controller.IsDead())
                {
                    GameManager.Instance.GetSoundManager().PlaySoundAtLocation(stateData.landSFX, controller.transform.position);
                    CheckForFlip();
                }
                controller.GetAnimator().SetBool(animBoolName, false);
            };
        }

        public override void Enter()
        {
            base.Enter();
            
            controller.OnLandEvent.AddListener(onLandAction);

            controller.GetAnimator().SetBool(animBoolName, true);
            
            GameManager.Instance.GetSoundManager().PlaySoundAtLocation(stateData.jumpSFX, controller.transform.position);
            //AudioSource.PlayClipAtPoint(stateData.jumpSFX, controller.GetTransfrom().position);
            controller.AddForce(stateData.jumpingForce, true);
        }

        public override void Exit()
        {
            base.Exit();
            controller.OnLandEvent.RemoveListener(onLandAction);
            controller.GetAnimator().SetBool(animBoolName, false);
        }

        private void CheckForFlip()
        {
            if (controller.CheckWall() || !controller.CheckLedge())
                controller.Flip();
        }
    }
}