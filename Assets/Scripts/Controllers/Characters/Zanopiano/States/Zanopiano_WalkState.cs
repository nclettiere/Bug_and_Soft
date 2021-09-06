using Controllers.Movement;
using Controllers.StateMachine;

namespace Controllers.Characters.Zanopiano.States
{
    public class Zanopiano_WalkState : State
    {

        private ZanopianoController zController;
        
        public Zanopiano_WalkState(BaseController controller, ControllerStateMachine stateMachine, string animBoolName, ZanopianoController zController) 
            : base(controller, stateMachine, animBoolName)
        {
            this.zController = zController;
        }

        public override void Enter()
        {
            base.Enter();
            controller.GetAnimator().SetBool(animBoolName, true);
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void UpdateState()
        {
            base.UpdateState();

            CheckearLedge();
        }

        private void CheckearLedge()
        {
            if (controller.CheckWall() || controller.CheckGround() && !controller.CheckLedge())
                controller.Flip();
        }

        public override void UpdatePhysics()
        {
            base.UpdatePhysics();
            
            controller.GetMovementController<BaseMovementController>()
                .Move();
        }
    }
}