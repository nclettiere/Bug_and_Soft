using Controllers.StateMachine;
using UnityEngine;

namespace Controllers.States
{
    public class Pepe_CompanionState : State
    {
        private PepeController pController;


        public Pepe_CompanionState(BaseController controller, ControllerStateMachine stateMachine, string animBoolName,
            PepeController pController)
            : base(controller, stateMachine, animBoolName)
        {
            this.pController = pController;
        }

        public override void Enter()
        {
            base.Enter();
            controller.GetAnimator().SetBool(
                "Following", true);
        }

        public override void Exit()
        {
            base.Exit();
            controller.GetAnimator().SetBool(
                "Following", false);
        }

        public override void UpdatePhysics()
        {
            base.UpdatePhysics();
            pController.SetTargetDestination(
                GameManager
                    .Instance
                    .PlayerController
                    .GetPepeTarget()
            );
        }
    }
}