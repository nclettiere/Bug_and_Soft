using Controllers.StateMachine;
using Controllers.StateMachine.States;
using Controllers.StateMachine.States.Data;

namespace Controllers.Froggy
{
    public class Froggy_DamageState : DamageState
    {
        private FroggyController froggyController;

        public Froggy_DamageState(BaseController controller, ControllerStateMachine stateMachine, string animBoolName,
            DamageStateData stateData, FroggyController froggyController)
            : base(controller, stateMachine, animBoolName, stateData)
        {
            this.froggyController = froggyController;
        }

        public override void UpdateState()
        {
            //if (controller.currentHealth <= controller.ctrlData.maxHealth / 2)
            //{
            //    froggyController.EnterPhaseTwo();
            //}
            
            base.UpdateState();
            stateMachine.ChangeState(froggyController._idleState);
        }
    }
}