using UnityEngine;

namespace Player
{
    public class PlayerPowerUp
    {
        protected static EPowerUpKind _powerUpKind = EPowerUpKind.NONE;
        protected bool activated;
        
        public PlayerPowerUp()
        { 
        }

        public virtual void Enter()
        {
            // to implement
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