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
        private static int current;
        private static void SetupSwitchAbility()
        {
            GameManager.Instance.GetPlayerControls().Gameplay.SwitchAbility.performed += ctx =>
            {
                current++;
                GameManager.PlayerController.powerUps.ChangePowerUp();
                if (current > 3)
                {
                    current = 0;
                }
            };
        }
    }
}