using Controllers;
using Controllers.Characters.Vergen;
using Controllers.Movement;
using Controllers.StateMachine;
using UnityEngine;

namespace Controllers.Characters.Vergen.States
{
    public class VergenPhaseOneState : State
    {
        private VergenController vController;

        private bool hasAttackedNearRange;

        private int _attackKind;
        public bool attackOnCourse;

        public VergenPhaseOneState(BaseController controller, ControllerStateMachine stateMachine, string animBoolName,
            VergenController vController)
            : base(controller, stateMachine, animBoolName)
        {
            this.vController = vController;
        }

        public override void Enter()
        {
            base.Enter();
            attackOnCourse = false;
        }

        public override void Exit()
        {
            base.Exit();

            vController.GetAnimator().SetBool(animBoolName, false);
        }

        public override void UpdateState()
        {
            base.UpdateState();

            if (vController.currentHealth <= vController.ctrlData.maxHealth / 3)
            {
                stateMachine.ChangeState(vController.vergenPhaseTwoState);
            }
            
            LookAtPlayer();
        }

        public override void UpdatePhysics()
        {
            base.UpdatePhysics();

            if (!attackOnCourse)
            {
                if (_attackKind == 0)
                {
                    if (!vController.CheckPlayerInNearRange())
                    {
                        vController.GetMovementController<BaseMovementController>().Move();
                        vController.GetAnimator().SetBool(animBoolName, true);
                    }
                    else
                    {
                        attackOnCourse = true;
                        vController.GetAnimator().SetBool(animBoolName, false);
                        hasAttackedNearRange = true;
                        _attackKind = Random.Range(0, 2);
                        Debug.Log($"AttackKind is {_attackKind}");
                        stateMachine.ChangeState(vController.vergenNormalAttackState);
                        return;
                    }
                }

                if (_attackKind == 1)
                {
                    vController.GetAnimator().SetBool(animBoolName, false);
                    _attackKind = 0;
                    stateMachine.ChangeState(vController.vergenLongAttackState);
                }
            }
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