using Controllers.StateMachine;
using Controllers.StateMachine.States;
using Controllers.StateMachine.States.Data;
using Items;
using UnityEngine;

namespace Controllers.Characters.SuperFroggy.States
{
    public class SuperFroggyDeadState : State
    {

        private SuperFroggyController _froggyController;

        private float enterPhaseTwoWaitTime = float.NegativeInfinity;

        public SuperFroggyDeadState(BaseController controller, ControllerStateMachine stateMachine, string animBoolName, SuperFroggyController froggyController)
            : base(controller, stateMachine, animBoolName)
        {
            _froggyController = froggyController;
        }


        public override void Enter()
        {
            _froggyController.DropItems();

            if (_froggyController.GetComponent<ItemGiver>() != null)
            {
                _froggyController.GetComponent<ItemGiver>().Run();
            }
            
            GameManager.Instance.AddPlayerKrowns(controller.KrownReward);
            _froggyController.GetAnimator().SetBool("SuperFroggy_Transforming", true);
            enterPhaseTwoWaitTime = Time.time + 3f;
        }

        public override void UpdateState()
        {

            LookAtPlayer();
            if (Time.time > enterPhaseTwoWaitTime)
                _froggyController.Explode();
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