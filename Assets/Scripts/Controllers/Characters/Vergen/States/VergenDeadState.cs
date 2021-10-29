using System.Collections;
using Controllers.StateMachine;
using UnityEngine;

namespace Controllers.Characters.Vergen.States
{
    public class VergenDeadState : State
    {
        private VergenController vController;
        
        private float Speed = 5f;
        private float delaySeconds;
        private float animChangeSeconds = float.NegativeInfinity;
        private float deadWaitSeconds;
        private bool isAnimChange = true;

        private float maxY;

        public VergenDeadState(BaseController controller, ControllerStateMachine stateMachine, string animBoolName,
            VergenController vController)
            : base(controller, stateMachine, animBoolName)
        {
            this.vController = vController;
        }

        public override void Enter()
        {
            base.Enter();
            
            GameManager.Instance.PlayerController.RefillHealth();
            GameManager.Instance.GetDynamicCamera().ChangeTarget(vController.transform);
            GameManager.Instance.gameInput.DisableInput();
            
            vController.rBody.gravityScale = 0;
            vController.rBody.velocity = Vector2.zero;

            maxY = vController.transform.position.y + 10;
            
            delaySeconds = startTime + 3f;
            deadWaitSeconds = startTime + 8f;
        }

        public override void UpdatePhysics()
        {
            base.UpdatePhysics();

            if (Time.time >= delaySeconds)
            {
                if (vController.transform.position.y <= maxY)
                {
                    vController.transform.position =
                        vController.transform.position + new Vector3(0, Speed * Time.deltaTime);
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
                        GameObject.Instantiate(vController.DeadExplosion, vController.transform.position,
                            Quaternion.identity);
                        vController.OnLifeTimeEnded.Invoke();
                        GameObject.Destroy(vController.gameObject);
                    }
                }
            }
        }
    }
}