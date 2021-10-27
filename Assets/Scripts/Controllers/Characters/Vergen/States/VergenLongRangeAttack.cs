using System.Collections;
using Controllers.StateMachine;
using UnityEngine;
using UnityEngine.Events;


namespace Controllers.Characters.Vergen.States
{
    public class VergenLongRangeAttack : State
    {
        private VergenController vController;

        public UnityEvent OnVergenShootingSpears;
        public int NumberOfSpears;

        public VergenLongRangeAttack(BaseController controller, ControllerStateMachine stateMachine, string animBoolName,
            VergenController vController)
            : base(controller, stateMachine, animBoolName)
        {
            this.vController = vController;

            OnVergenShootingSpears = new UnityEvent();
        }

        public override void Enter()
        {
            base.Enter();
            NumberOfSpears = Random.Range(1, 5);
            OnVergenShootingSpears.Invoke();
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