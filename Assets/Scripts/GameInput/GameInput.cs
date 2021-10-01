using Player;
using UnityEngine;

namespace Input
{
    public class GameInput
    {
        public static PlayerControls playerControls;
        
        public void SetupInputs()
        {
            playerControls = new PlayerControls();
            playerControls.Enable();
            
            SetupAbilitiesInput();
            SetupRollInput();
            SetupJumpInput();
            SetupInteractInput();
            SetupSwitchAbility();
            SetupStatsInput();
            SetupPlayerMovementInput();
            SetupMiscInput();
            SetupCameraInput();
            SetupPlayerAttack();
        }

        private void SetupMiscInput()
        {        
            playerControls.Gameplay.Pause.performed += ctx =>
            {
                if (GameManager.Instance.isMainMenuOn) return;

                if (GameManager.Instance.isGamePaused)
                    GameManager.Instance.ResumeGame();
                else
                    GameManager.Instance.PauseGame();
            };

           playerControls.Gameplay.QuickSave.performed += ctx =>
           {
               GameManager.Instance.QuickSave();
           };

           playerControls.Gameplay.QuickLoad.performed += ctx =>
           {
               GameManager.Instance.QuickLoad();
           };

           playerControls.Gameplay.UseItem.performed += ctx =>
           {
               GameManager.Instance.
                   GetInventorySlotManager().UseItem();
           };
           
        }

        private void SetupAbilitiesInput()
        {
            playerControls.Gameplay.SpecialMove.performed += ctx =>
            {
                if(GameManager.Instance.PlayerController.powerUps.currentPowerUp != null)
                    GameManager.Instance.PlayerController.powerUps.currentPowerUp.Activate();
            };
        }

        private void SetupRollInput()
        {
            playerControls.Gameplay.Roll.performed += ctxRoll =>
            {
                GameManager.Instance.PlayerController.Rollear();
            };
        }
        
        private void SetupJumpInput()
        {
            playerControls.Gameplay.Jump.performed += ctx =>
            {
                GameManager.Instance.PlayerController.Jumpear();
            };
        }
        
        private void SetupInteractInput()
        {
            playerControls.Gameplay.Interact.performed += ctx =>
            {
                GameManager.Instance.PlayerController.Interactear();
            };
        }

        // TEMP
        private int current = 0;
        private void SetupSwitchAbility()
        {
            playerControls.Gameplay.SwitchAbility.performed += ctx =>
            {
                GameManager.Instance.PlayerController.CyclePowerUps();
            };
        }

        private void SetupStatsInput()
        {
            playerControls.Gameplay.ViewStats.performed += ctx =>
            {
                GameManager.Instance.GetHUD()
                    .ShowStats();
            };
            
            playerControls.Gameplay.ViewStats.canceled += ctx =>
            {
                GameManager.Instance.GetHUD()
                    .HideStats();
            };
        }

        public void SetupPlayerMovementInput()
        {
            // Horizontal
            playerControls.Gameplay.Horizontal.performed += ctx =>
            {
                GameManager.Instance.PlayerController.SetHorizontalSpeed(
                    playerControls.Gameplay.Horizontal.ReadValue<float>());
            };
            playerControls.Gameplay.Horizontal.canceled += ctx =>
            {
                GameManager.Instance.PlayerController.SetHorizontalSpeed(0f);
            };
            
            // Vertical
            playerControls.Gameplay.Vertical.performed += ctx =>
            {
                GameManager.Instance.PlayerController.SetVerticalSpeed(
                    playerControls.Gameplay.Horizontal.ReadValue<float>());
            };
            playerControls.Gameplay.Vertical.canceled += ctx =>
            {
                GameManager.Instance.PlayerController.SetVerticalSpeed(0f);
            };
        }

        private void SetupCameraInput()
        {
            playerControls.Gameplay.Camera.performed += ctx =>
            {
                var value = ctx.ReadValue<Vector2>();
                GameManager.Instance.GetDynamicCamera()
                    .UpdateOffsetJoystick(value);
            };
            
            playerControls.Gameplay.Camera.canceled += ctx => 
                GameManager.Instance.GetDynamicCamera()
                    .UpdateOffsetJoystick(Vector2.zero);
        }

        private void SetupPlayerAttack()
        {
            playerControls.Gameplay.Attack.performed += ctx =>
                GameManager.Instance.PlayerController.combatCtrl.AttackPerformed();
        }
    }
}