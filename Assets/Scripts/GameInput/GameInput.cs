using Player;
using UnityEngine;

namespace Input
{
    public class GameInput
    {
        public static void SetupInputs()
        {
            SetupAbilitiesInput();
            SetupRollInput();
            SetupJumpInput();
            SetupInteractInput();
            SetupSwitchAbility();
        }

        private static void SetupAbilitiesInput()
        {
            GameManager.Instance.GetPlayerControls().Gameplay.SpecialMove.performed += ctx =>
            {
                GameManager.PlayerController.powerUps.currentPowerUp.Activate();
            };
        }

        private static void SetupRollInput()
        {
            GameManager.Instance.GetPlayerControls().Gameplay.Roll.performed += ctxRoll =>
            {
                GameManager.PlayerController.Rollear();
            };
        }
        
        private static void SetupJumpInput()
        {
            GameManager.Instance.GetPlayerControls().Gameplay.Jump.performed += ctx =>
            {
                GameManager.PlayerController.Jumpear();
            };
        }
        
        private static void SetupInteractInput()
        {
            GameManager.Instance.GetPlayerControls().Gameplay.Interact.performed += ctx =>
            {
                GameManager.PlayerController.Interactear();
            };
        }

        // TEMP
        private static int current = 0;
        private static void SetupSwitchAbility()
        {
            GameManager.Instance.GetPlayerControls().Gameplay.SwitchAbility.performed += ctx =>
            {
                PlayerPowerUp pw;
                
                if (current > 2)
                {
                    current = 0;
                }

                switch (current)
                {
                    case 0:
                        pw = GameManager.PlayerController.teleportPowerUp;
                        Debug.Log("Ability switched to: ROMHOPP");
                        break;
                    case 1:
                        pw = GameManager.PlayerController.shieldPowerUp;
                        Debug.Log("Ability switched to: SHIELD");
                        break;
                    case 2:
                        pw = GameManager.PlayerController.godLikePowerUp;
                        Debug.Log("Ability switched to: GODLIKE");
                        break;
                    default:
                        pw = GameManager.PlayerController.teleportPowerUp;
                        Debug.Log("Ability switched to: ROMHOPP");
                        break;
                }
                GameManager.PlayerController.powerUps.ChangePowerUp(pw);
                
                current++;
            };
        }
    }
}