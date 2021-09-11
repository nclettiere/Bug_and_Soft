using Controllers.Movement;
using Controllers.StateMachine;
using UnityEngine;

namespace Controllers.Characters.Zanopiano.States
{
    public class Zanopiano_AttackState : State
    {

        private ZanopianoController zController;
        
        public Zanopiano_AttackState(BaseController controller, ControllerStateMachine stateMachine, string animBoolName, ZanopianoController zController) 
            : base(controller, stateMachine, animBoolName)
        {
            this.zController = zController;
        }

        public override void Enter()
        {
            base.Enter();
            controller.GetAnimator().SetBool("Alerting", false);
            controller.GetAnimator().SetBool("Alerting_Completed", false);
            controller.GetAnimator().SetBool(animBoolName, true);
            
            controller.AddForce(new Vector2(20f, 1f), true);
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void UpdateState()
        {
            base.UpdateState();
        }

        //private void CheckearLedge()
        //{
        //    if (controller.CheckWall() || controller.CheckGround() && !controller.CheckLedge())
        //        controller.Flip();
        //}
        //
        //private void CheckLongRange()
        //{
        //    if(controller.CheckPlayerInLongRange())
        //    {
        //        if (!controller.GetAnimator().GetBool("Alerting_Completed"))
        //        {
        //            controller.GetAnimator().SetBool("Alerting", true);
        //            alerting = true;
        //        }
        //        else
        //        {
        //            alerting = false;
        //        }
        //    }
        //}
        //
        //private void CheckNearRange()
        //{
        //    if(controller.CheckPlayerInNearRange())
        //    {
        //        alerting = false;
        //        if (!controller.GetAnimator().GetBool("Attacking_Completed"))
        //        {
        //            controller.GetAnimator().SetBool("Alerting_Completed", false);
        //            controller.GetAnimator().SetBool("Alerting", false);
        //            controller.GetAnimator().SetBool("Attacking", true);
        //            stateMachine.ChangeState(zController.AttackState);
        //        }
        //    }
        //}
//
        //public override void UpdatePhysics()
        //{
        //    base.UpdatePhysics();
//
        //    if (!controller.CheckPlayerInLongRange() && !controller.CheckPlayerInNearRange())
        //    {
        //        controller.GetAnimator().SetBool("Alerting_Completed", false);
        //        controller.GetAnimator().SetBool("Alerting", false);
        //        controller.GetAnimator().SetBool("Attacking", false);
        //        
        //        controller.GetMovementController<BaseMovementController>()
        //            .Move();
        //    }
        //}
    }
}