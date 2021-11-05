using Controllers;
using Controllers.Characters.Vergen;
using Controllers.Movement;
using Controllers.StateMachine;
using UnityEngine;

namespace Controllers.Characters.Vergen.States
{
    public class VergenPhaseTwoState : State
    {
        private VergenController vController;

        private bool hasAttackedNearRange;

        private int _attackKind;
        public bool attackOnCourse;
        public bool enteredRageMode;
        
        public bool freeKillChance;
        public float freeKillChanceTime;

        public VergenPhaseTwoState(BaseController controller, ControllerStateMachine stateMachine, string animBoolName,
            VergenController vController)
            : base(controller, stateMachine, animBoolName)
        {
            this.vController = vController;
        }

        public override void Enter()
        {
            base.Enter();

            // First time phase 2
            if (vController.CurrentPhase == 1)
            {
                attackOnCourse = false;
                _attackKind = 2;

                int result = Random.Range(0, 100);

                if (result >= 10 && result <= 45 || (vController.AlwaysEnterRageMode && !enteredRageMode))
                {
                    freeKillChance = true;
                    freeKillChanceTime = startTime + Random.Range(0, 20);

                    GameManager.Instance.ShowBossHealth("VERGEN - RAGE MODE", vController);
                }
                else
                {
                    GameManager.Instance.ShowBossHealth("VERGEN - FASE 2", vController);
                }

                vController.CurrentPhase = 2;
            }
            else
            {
                attackOnCourse = false;
            }
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

            if (freeKillChance)
            {
                if (Time.time >= freeKillChanceTime)
                {
                    vController.VergenGhostTriggers[0].Run();//Random.Range(0, 2)].Run();
                    freeKillChance = false;
                    enteredRageMode = true;
                    stateMachine.ChangeState(vController.vergenTeleportState);
                    return;
                }
            }

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
                        _attackKind = Random.Range(0, 3);
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
                
                if (_attackKind == 2)
                {
                    vController.GetAnimator().SetBool(animBoolName, false);
                    _attackKind = 0;
                    stateMachine.ChangeState(vController.vergenTorbeshinoState);
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