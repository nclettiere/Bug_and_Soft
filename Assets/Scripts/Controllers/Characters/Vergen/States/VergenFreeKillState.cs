using System.Collections;
using Controllers.StateMachine;
using UnityEngine;

namespace Controllers.Characters.Vergen.States
{
    public class VergenFreeKillState : State
    {
        private VergenController vController;

        public VergenFreeKillState(BaseController controller, ControllerStateMachine stateMachine, string animBoolName,
            VergenController vController)
            : base(controller, stateMachine, animBoolName)
        {
            this.vController = vController;
        }

        public override void Enter()
        {
            base.Enter();
            
            vController.GetAnimator().SetBool("DisappearWait", false);
        }
    }
}