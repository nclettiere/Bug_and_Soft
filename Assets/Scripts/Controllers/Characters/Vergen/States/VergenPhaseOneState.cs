using Controllers;
using Controllers.Characters.Vergen;
using Controllers.Movement;
using Controllers.StateMachine;

namespace Controllers.Characters.Vergen.States
{
    public class VergenPhaseOneState : State
    {
        private VergenController vController;

        private bool hasAttackedNearRange = true;

        public VergenPhaseOneState(BaseController controller, ControllerStateMachine stateMachine, string animBoolName,
            VergenController vController)
            : base(controller, stateMachine, animBoolName)
        {
            this.vController = vController;
        }

        public override void Enter()
        {
            base.Enter();
        }

        public override void Exit()
        {
            base.Exit();
            
            vController.GetAnimator().SetBool(animBoolName, false);
        }

        public override void UpdateState()
        {
            base.UpdateState();
            LookAtPlayer();
        }

        public override void UpdatePhysics()
        {
            base.UpdatePhysics();

            //if (!vController.CheckPlayerInNearRange())
            //{
            //    vController.GetMovementController<BaseMovementController>().Move();
            //    vController.GetAnimator().SetBool(animBoolName, true);
            //}
            //else
            //{
            //    vController.GetAnimator().SetBool(animBoolName, false);
            //    stateMachine.ChangeState(vController.vergenNormalAttackState);
            //    hasAttackedNearRange = true;
            //}
            //
            //if(hasAttackedNearRange) {
                if (vController.CheckPlayerInLongRange())
                {
                    vController.GetAnimator().SetBool(animBoolName, false);
                    stateMachine.ChangeState(vController.vergenLongAttackState);
                }
            //}
        }
        
        protected virtual void LookAtPlayer()
        {
            float playerPositionX = GameManager.Instance.GetPlayerTransform().position.x;
                
            if ((controller.transform.position.x < playerPositionX && controller.FacingDirection == -1) ||
                (controller.transform.position.x > playerPositionX && controller.FacingDirection == 1))
                controller.Flip();
        }
    }
}