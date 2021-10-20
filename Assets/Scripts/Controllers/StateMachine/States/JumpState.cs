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
                AudioSource.PlayClipAtPoint(stateData.landSFX, controller.GetTransfrom().position);
                stateMachine.ChangeState(froggyController._idleState);
            });
            
            controller.GetAnimator().SetBool(animBoolName, true);
            AudioSource.PlayClipAtPoint(stateData.jumpSFX, controller.GetTransfrom().position);
            controller.AddForce(stateData.jumpingForce, true);
        }

        public override void Exit()
        {
            base.Exit();
            controller.GetAnimator().SetBool(animBoolName, false);
        }
        
    }
}