using Controllers.StateMachine;
using Controllers.StateMachine.States;
using Controllers.StateMachine.States.Data;
using UnityEngine;

namespace Controllers.Froggy
{
    public class SuperFroggy_SecondPhase : State
    {
        private SuperFroggyController froggyController;
        
        protected SuperFroggy_SecondPhaseStateData stateData;

        private float lastJumpTime = float.NegativeInfinity;
        private float bombTimeWait = float.NegativeInfinity;
        private float jumpCooldownTime;
        private float enterPhaseTwoWaitTime;
        private bool onTarget;
        
        protected bool isDetectingGround;

        private int initialFacingPos;
        private float bombPlacementWaitTime = float.NegativeInfinity;
        
        public SuperFroggy_SecondPhase(BaseController controller, ControllerStateMachine stateMachine, string animBoolName, SuperFroggy_SecondPhaseStateData stateData, SuperFroggyController froggyController) 
            : base(controller, stateMachine, animBoolName)
        {
            this.froggyController = froggyController;
            this.stateData = stateData;
        }

        public override void Enter()
        {
            base.Enter();
            
            Debug.Log("Froggy is in secondphase");
            
            froggyController.GetAnimator().SetBool("NearAttackAlert", true);
            froggyController.transforming = true;
            enterPhaseTwoWaitTime = Time.time + 1f;
            jumpCooldownTime = Time.time + .25f;
            isDetectingGround = controller.CheckGround();

            initialFacingPos = froggyController.FacingDirection;
        }

        public override void Exit()
        {
            base.Exit(); 
        }

        public override void UpdateState()
        {
            LookAtPlayer();
            
            if (Time.time > enterPhaseTwoWaitTime)
            {
                if (!froggyController.transformed)
                {
                    froggyController.GetAnimator().SetBool("NearAttackAlert", false);
                    froggyController.GetAnimator().SetBool("SuperFroggy_Transforming", false);
                    froggyController.GetAnimator().SetBool("SuperFroggy_Transformed", true);
                    froggyController.transformed = true;
                    froggyController.transforming = false;
                }
                
                froggyController.CheckTouchDamage();

                if (!onTarget && Time.time > bombTimeWait)
                {
                        froggyController.AddForce(new Vector2(10f, 5f), true);
                        onTarget = true;
                        bombTimeWait = Time.time + 1f;
                }else
                {
                    if (!controller.CheckGround())
                    {
                        if (Time.time > bombPlacementWaitTime)
                        {
                            froggyController.PlaceBomb();
                            bombPlacementWaitTime = Time.time + 0.15f;
                        }
                    }
                    isDetectingGround = controller.CheckGround();
                }
                
                // OnLand
                if (controller.CheckGround())
                {
                    onTarget = false;
                    initialFacingPos = -initialFacingPos;
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