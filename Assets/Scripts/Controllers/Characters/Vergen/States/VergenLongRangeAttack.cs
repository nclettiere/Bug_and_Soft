using System.Collections;
using Controllers.StateMachine;
using UnityEngine;


namespace Controllers.Characters.Vergen.States
{
    public class VergenLongRangeAttack : State
    {
        private VergenController vController;

        public VergenLongRangeAttack(BaseController controller, ControllerStateMachine stateMachine, string animBoolName,
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

        public override void Exit()
        {
            base.Exit();
            vController.GetAnimator().SetBool(animBoolName, false);
        }

        public void OnAttackFinish()
        {
            vController.GetAnimator().SetBool(animBoolName, false);
            vController.PhaseOneAttackWait();
        }
    }
}