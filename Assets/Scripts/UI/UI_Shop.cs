using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UI_Shop : MonoBehaviour
    {
        [SerializeField] private Button warpyButton;
        [SerializeField] private Text warpyText;
        [SerializeField] private Button shieldButton;
        [SerializeField] private Text shieldText;
        [SerializeField] private Button godlikeButton;
        [SerializeField] private Text godlikeText;
        [SerializeField] private Button potaButton;
        [SerializeField] private Text potaText;

        private void OnEnable()
        {
            GameManager.Instance.PauseGame();

            foreach (var powerUp in GameManager.Instance.UnlockedPowerUps)
            {
                if (powerUp.powerUpKind == EPowerUpKind.TELEPORT)
                {
                    warpyButton.interactable = false;
                    warpyText.text = "SOLD OUT";
                }else if (powerUp.powerUpKind == EPowerUpKind.SHIELD)
                {
                    shieldButton.interactable = false;
                    shieldText.text = "SOLD OUT"; 
                }else if (powerUp.powerUpKind == EPowerUpKind.GODLIKE)
                {
                    godlikeButton.interactable = false;
                    godlikeText.text = "SOLD OUT";
                }
            }
        }

        public void UnlockWarpy()
        {
            if (GameManager.Instance.PlayerKrowns >= 150)
            {
                // Desbloqueamos la ability para el player
                GameManager.Instance.PlayerController.UnlockPowerUp(EPowerUpKind.TELEPORT, 150);
                // Hacemos que el boton muestre "Sold Out  <-------->    beATCHHH MADAFAKA YEEEE"
                warpyButton.interactable = false;
                warpyText.text = "SOLD OUT";
            }
        }
        
        public void UnlockShield()
        {
            if (GameManager.Instance.PlayerKrowns >= 150)
            {
                // Desbloqueamos la ability para el player
                GameManager.Instance.PlayerController.UnlockPowerUp(EPowerUpKind.SHIELD, 150);
                // Hacemos que el boton muestre "Sold Out  <-------->    beATCHHH MADAFAKA YEEEE"
                shieldButton.interactable = false;
                shieldText.text = "SOLD OUT";
            }
        }
        
        public void UnlockGodlike()
        {
            if (GameManager.Instance.PlayerKrowns >= 150)
            {
                // Desbloqueamos la ability para el player
                GameManager.Instance.PlayerController.UnlockPowerUp(EPowerUpKind.GODLIKE, 750);
                // Hacemos que el boton muestre "Sold Out  <-------->    beATCHHH MADAFAKA YEEEE"
                godlikeButton.interactable = false;
                godlikeText.text = "SOLD OUT";
            }
        }
    }
}