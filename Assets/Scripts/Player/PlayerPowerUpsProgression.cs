using UnityEngine;

namespace Player
{
    public class PlayerPowerUpsProgression
    {
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