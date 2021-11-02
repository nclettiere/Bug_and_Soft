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
        public int NumberOfSpearsShot;

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
            
            //if(vController.CheckPlayerInNearRange())
                controller.rBody.AddForce(new Vector2(40 * -controller.FacingDirection, 20),
                    ForceMode2D.Impulse);
            
            NumberOfSpears = Random.Range(1, 5);
            OnVergenShootingSpears.Invoke();
        }

        public override void UpdatePhysics()
        {
            base.UpdatePhysics();

            if (NumberOfSpearsShot >= NumberOfSpears)
            {
                stateMachine.ChangeState(vController.vergenPhaseOneState);
            }
            else
            {
                if (vController.HasSpearAttackFinished && vController.CheckGround())
                {
                    LookAtPlayer();
                    vController.ShootSpears();
                }
            }
        }

        public override void Exit()
        {
            base.Exit();
            NumberOfSpearsShot = 0;
            vController.GetAnimator().SetBool(animBoolName, false);
        }
        
        
        public void OnSpearShot()
        {
            NumberOfSpearsShot++;
        }

        public void OnAttackFinish()
        {
            vController.GetAnimator().SetBool(animBoolName, false);
            vController.PhaseOneAttackWait();
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