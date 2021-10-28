using System.Collections;
using Controllers.StateMachine;
using UnityEngine;

namespace Controllers.Characters.Vergen.States
{
    public class VergenTeleportState : State
    {
        private VergenController vController;

        public VergenTeleportState(BaseController controller, ControllerStateMachine stateMachine, string animBoolName,
            VergenController vController)
            : base(controller, stateMachine, animBoolName)
        {
            this.vController = vController;
        }

        public override void Enter()
        {
            base.Enter();
            
            vController.GetAnimator().SetBool(animBoolName, true);
        }

        public void OnDisappearFinished()
        {
            if (stateMachine.CurrentState == this)
            {
                vController.GetAnimator().SetBool(animBoolName, false);
                vController.GetAnimator().SetBool("DisappearWait", true);
            }
        }
    }
}