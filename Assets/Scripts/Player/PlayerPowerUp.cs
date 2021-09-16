using UnityEngine;

namespace Player
{
    public class PlayerPowerUp
    {
        protected EPowerUpKind _powerUpKind = EPowerUpKind.NONE;
        protected bool activated;
        
        public PlayerPowerUp()
        { 
        }

        public virtual void Enter()
        {
            Debug.Log("YEEE " + _powerUpKind);
            GameManager.Instance.GetUIManager().ChangePowerUpKind(_powerUpKind);
        }

        public virtual void Exit()
        {
            // to implement
        }
        
        public virtual void OnUpdate()
        {
            // to implement
        }

        public void Activate()
        {
            activated = true;
        }
    }
}