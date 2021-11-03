using Controllers.StateMachine;
using UnityEngine;

namespace Controllers.Characters.SuperFroggy.States
{
    public class SuperFroggyIdleState : State
    {
        private SuperFroggyController froggyController;
        private float jumpRatio;
        private float jumpTimeWait;
        private Vector2 jumpForce;

        public SuperFroggyIdleState(BaseController controller, ControllerStateMachine stateMachine, string animBoolName,
            SuperFroggyController froggyController)
            : base(controller, stateMachine, animBoolName)
        {
            this.froggyController = froggyController;
        }

        public override void UpdateState()
        {
            base.UpdateState();

            if (controller.controllerKind == EControllerKind.Boss &&
                controller.currentHealth <= controller.ctrlData.maxHealth / 2)
            {
                froggyController.EnterPhaseTwo();
            }
//
            //if (controller.CheckPlayerInLongRange())
            //{
            //    controller.ShowTauntIndicator();
            //    stateMachine.ChangeState(froggyController._attackState);
            //}
//
            if (controller.CheckPlayerInNearRange())
            {
                stateMachine.ChangeState(froggyController._nearAttackState);
            }


            if (Time.time >= jumpTimeWait && !controller.IsDead())
            {
                stateMachine.ChangeState(froggyController._jumpState);
                jumpTimeWait = Time.time + jumpRatio;
            }
        }

        public override void Enter()
        {
            base.Enter();
            jumpForce = new Vector2(3, 5);
            jumpRatio = Random.Range(2f, 3f);
            jumpTimeWait = startTime + jumpRatio;


            controller.OnLandEvent.AddListener(() =>
            {
                if (!controller.IsDead())
                {
                    if (controller.CheckWall() || !controller.CheckLedge())
                        controller.Flip();
                }
            });
        }
    }
}