using System;
using Controllers.Damage;
using Controllers.Movement;
using Controllers.StateMachine;
using UnityEngine;

namespace Controllers.Characters.Zanopiano.States
{
    public class Zanopiano_AttackState : State
    {

        private ZanopianoController zController;
        private bool attackFinished;
        private float cooldownTime = float.NegativeInfinity;
        private bool cooldownTimeSetted;
        
        private DamageInfo dInfo;
        
        public Zanopiano_AttackState(BaseController controller, ControllerStateMachine stateMachine, string animBoolName, ZanopianoController zController) 
            : base(controller, stateMachine, animBoolName)
        {
            this.zController = zController;
        }

        public override void Enter()
        {
            if (zController.currentHealth <= 0)
            {
                zController.StateMachine.ChangeState(zController.DeadState);
            }
            base.Enter();
            controller.GetAnimator().SetBool("Alerting", false);
            controller.GetAnimator().SetBool("Alerting_Completed", false);
            controller.GetAnimator().SetBool(animBoolName, true);
            
            controller.AddForce(new Vector2(20f, 1f), true);
        }

        public override void Exit()
        {
            base.Exit();
            controller.GetAnimator().SetBool(animBoolName, false);
            controller.GetAnimator().SetBool("Attacking_Cooldown", false);
            attackFinished = false;
            cooldownTimeSetted = false;
        }

        public override void UpdateState()
        {
            if (zController.currentHealth <= 0)
            {
                zController.StateMachine.ChangeState(zController.DeadState);
            }

            if (attackFinished)
            {
                if (!cooldownTimeSetted)
                {
                    cooldownTime = Time.time + 1f;
                    cooldownTimeSetted = true;
                }

                if (Time.time >= cooldownTime)
                {
                    stateMachine.ChangeState(zController.WalkState);
                }
            }
        }

        public void AttackedFinished()
        {
            controller.GetAnimator().SetBool(animBoolName, false);
            controller.GetAnimator().SetBool("Attacking_Cooldown", true);
            attackFinished = true;
        }

        public void AttackNow()
        {
            RaycastHit2D hit =
                Physics2D.CircleCast(zController.attackPosition.position,
                    zController.attackRadius,
                    Vector2.zero,
                    zController.ctrlData.whatIsPlayer);

            if (hit != null)
            {
                dInfo = new DamageInfo(35, zController.GetTransfrom().position.x, true);
                GameManager.Instance.PlayerController.Damage(dInfo);
                GameManager.Instance.GetPepeController().ShowQuickChat(
                    new Tuple<Sprite, int>(zController.PepeQC, 2));
            }
        }
    }
}