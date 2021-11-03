using System;
using System.Collections.Generic;
using Player;
using UnityEngine;

[Serializable]
public class GameData
{
    public int Level;
    public int PlayerHealth;
    public int PlayerMaxHealth;
    public float[] playerPosition;
    public int[] unlockedPowerUps;
    public int   currentPowerUp = -1;
    public int   currentKrones;
    public int   currentExp;

    public GameData()
    {
        playerPosition = new float[3];
        unlockedPowerUps = new[] {0, 0, 0};
    }

    public void SetPlayerPosition(Vector3 transformPosition)
    {
        playerPosition[0] = transformPosition.x;
        playerPosition[1] = transformPosition.y;
        playerPosition[2] = 0;
    }

    public void SetPowerUps(List<PlayerPowerUp> playerControllerUnlockedPowerUps)
    {
        foreach (var powerUp in playerControllerUnlockedPowerUps)
        {
            switch (powerUp.powerUpKind)
            {
                case EPowerUpKind.TELEPORT:
                    unlockedPowerUps[0] = 1;
                    break;
                case EPowerUpKind.SHIELD:
                    unlockedPowerUps[1] = 1;
                    break;
                case EPowerUpKind.GODLIKE:
                    unlockedPowerUps[2] = 1;
                    break;
            }
        }
    }

    public void SetCurrentPowerUp(PlayerPowerUp powerUpsCurrentPowerUp)
    {
        if (powerUpsCurrentPowerUp != null)
        {
            switch (powerUpsCurrentPowerUp.powerUpKind)
            {
                case EPowerUpKind.TELEPORT:
                    Debug.Log("Current pw is: Teleport");
                    currentPowerUp = 0;
                    break;
                case EPowerUpKind.SHIELD:
                    Debug.Log("Current pw is: SHIELD");
                    currentPowerUp = 1;
                    break;
                case EPowerUpKind.GODLIKE:
                    Debug.Log("Current pw is: GODLIKE");
                    currentPowerUp = 2;
                    break;
            }
        }
        else
        {
            Debug.Log("powerUpsCurrentPowerUp: null");
        }
    }
}