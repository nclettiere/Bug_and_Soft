using System.Collections;
using Controllers.Froggy;
using Controllers.StateMachine;
using UnityEngine;

namespace Controllers.Characters.OLC.States
{
    public class OLC_DeadState : State
    {
        private OLC_Controller oController;
        
        private float Speed = 5f;
        private float delaySeconds;
        private float animChangeSeconds = float.NegativeInfinity;
        private float deadWaitSeconds;
        private bool isAnimChange = true;

        private float maxY;

        public OLC_DeadState(BaseController controller, ControllerStateMachine stateMachine, string animBoolName,
            OLC_Controller oController)
            : base(controller, stateMachine, animBoolName)
        {
            this.oController = oController;
        }

        public override void Enter()
        {
            base.Enter();
            
            GameManager.Instance.PlayerController.RefillHealth();
            GameManager.Instance.GetDynamicCamera().ChangeTarget(oController.transform);
            GameManager.Instance.gameInput.DisableInput();
            
            oController.rBody.gravityScale = 0;
            oController.rBody.velocity = Vector2.zero;

            maxY = oController.transform.position.y + 10;
            
            delaySeconds = startTime + 3f;
            deadWaitSeconds = startTime + 8f;
        }

        public override void UpdatePhysics()
        {
            base.UpdatePhysics();

            if (Time.time >= delaySeconds)
            {
                if (oController.transform.position.y <= maxY)
                {
                    oController.transform.position =
                        oController.transform.position + new Vector3(0, Speed * Time.deltaTime);
                }
                else
                {
                    controller.rBody.velocity = Vector2.zero;
                    
                    if (Time.time >= animChangeSeconds)
                    {
                        controller.GetAnimator().SetBool("Dead", isAnimChange);
                        isAnimChange = !isAnimChange;
                        animChangeSeconds = Time.time + 0.05f;
                    }
                    
                    // Reached Max : Do Once
                    if (Time.time >= deadWaitSeconds)
                    {
                        GameManager.Instance.HideBossHealth();
                        //GameObject.Instantiate(oController.DeadExplosion, oController.transform.position,
                        //    Quaternion.identity);
                        oController.OnLifeTimeEnded.Invoke();
                        GameManager.Instance.GetDynamicCamera().ChangeTarget(GameManager.Instance.PlayerController.gameObject.transform);
                        GameManager.Instance.gameInput.EnableInput();
                        GameObject.Destroy(oController.gameObject);
                    }
                }
            }
        }
    }
}