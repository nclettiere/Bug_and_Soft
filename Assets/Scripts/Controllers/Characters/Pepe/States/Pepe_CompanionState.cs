using Controllers.StateMachine;
using UnityEngine;

namespace Controllers.States
{
    public class Pepe_CompanionState : State
    {
        private PepeController pController;
        private Vector3 newPosition;

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
                
                newPosition = GameManager
                                .Instance
                                .PlayerController
                                .GetPepeTarget();
                
                controller.GetComponent<SpriteRenderer>()
                    .flipX = false;

                controller.canInteract = false;
        }

        public override void Exit()
        {
            base.Exit();
            controller.GetAnimator().SetBool(
                "Following", false);
            
            controller.GetComponent<SpriteRenderer>()
                .flipX = true;
            
            controller.canInteract = true;
        }

        public override void UpdateState()
        {
            base.UpdateState();
            
            newPosition.Set(
                GameManager.Instance.PlayerController.GetPepeTarget().x,
                GameManager.Instance.PlayerController.GetPepeTarget().y, 0);
        }

        public override void UpdatePhysics()
        {
            base.UpdatePhysics();
            LookAtPlayer();
            pController.transform.position = 
                Vector3.Lerp(new Vector3(pController.transform.position.x, pController.transform.position.y, 0), newPosition, Time.deltaTime * 10f);
        }
        
        private void LookAtPlayer()
        {
            float playerPositionX = GameManager.Instance.GetPlayerTransform().position.x;

            if ((controller.transform.position.x < playerPositionX && controller.FacingDirection == -1) ||
                (controller.transform.position.x > playerPositionX && controller.FacingDirection == 1))
            {
                pController.Flip();
            }
        }
    }
}