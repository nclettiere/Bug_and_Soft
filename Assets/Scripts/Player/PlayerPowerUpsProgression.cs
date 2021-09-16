using UnityEngine;

namespace Player
{
    public class PlayerPowerUpsProgression
    {
        // TODO: Serializable, CurrentPowerUp, CurrentProgressionTree, Switch PowerUp, Increment Progression Tree 
        //                       (que habilidades sube)
        
        public PlayerPowerUp currentPowerUp { get; private set; }

        public void Initialize(PlayerPowerUp powerUp)
        {
            currentPowerUp = powerUp;
            currentPowerUp.Enter();
        }

        public void ChangePowerUp(PlayerPowerUp powerUp)
        {
            if (currentPowerUp != null)
                currentPowerUp.Exit();
            currentPowerUp = powerUp;
            currentPowerUp.Enter();
        }
    }
}