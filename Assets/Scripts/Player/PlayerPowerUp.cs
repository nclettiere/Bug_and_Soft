using UnityEngine;

namespace Player
{
    public class PlayerPowerUp
    {
        public EPowerUpKind powerUpKind { get; protected set; } = EPowerUpKind.NONE;
        public bool activated;
        public bool onCooldown;
        
        public PlayerPowerUp(EPowerUpKind powerUpKind)
        {
            this.powerUpKind = powerUpKind;
        }

        public virtual void Enter()
        {
            GameManager.Instance.GetUIManager().ChangePowerUpKind(powerUpKind);
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