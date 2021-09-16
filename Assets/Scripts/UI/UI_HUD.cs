using System;
using Controllers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UI_HUD : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI krownsValue;
        [SerializeField] private GameObject bossHealth;
        [SerializeField] private TextMeshProUGUI bossNameText;
        [SerializeField] private Slider playerHealthSlider;
        [SerializeField] private Slider bossHealthSlider;

        [Header("HUD especificos")] 
        [SerializeField] private GameObject[] powerUpTeleportIndicator;
        [SerializeField] private GameObject[] powerUpShieldIndicator;
        [SerializeField] private GameObject[] powerUpGodLikeIndicator;

        private bool showBossHealth;
        private BaseController controller;

        private void Update()
        {
            krownsValue.text = GameManager.Instance.PlayerKrowns.ToString();

            UpdatePlayerHelathbar();

            if (showBossHealth)
            {
                UpdateBossHealthbar();
            }

            UpdateAbilities();
        }
        
        // Basicamente hace un clear a las habiliades xd
        public void SwitchAbility()
        {
            ClearAbility(EPowerUpKind.TELEPORT);
            ClearAbility(EPowerUpKind.SHIELD);
            ClearAbility(EPowerUpKind.GODLIKE);
        }

        private void ClearAbility(EPowerUpKind kind)
        {
            switch (kind)
            {
                case EPowerUpKind.TELEPORT:
                    foreach (var obj in powerUpTeleportIndicator)
                    {
                        obj.SetActive(false);
                    }

                    break;
                case EPowerUpKind.SHIELD:
                    foreach (var obj in powerUpShieldIndicator)
                    {
                        obj.SetActive(false);
                    }

                    break;
                case EPowerUpKind.GODLIKE:
                    foreach (var obj in powerUpGodLikeIndicator)
                    {
                        obj.SetActive(false);
                    }

                    break;
            }
        }

        private void UpdateAbilities()
        {
            switch (GameManager.Instance.GetUIManager().CurrentPowerUp)
            {
                case EPowerUpKind.TELEPORT:
                    UpdateAbility(ref powerUpTeleportIndicator);
                    break;
                case EPowerUpKind.SHIELD:
                    UpdateAbility(ref powerUpShieldIndicator);
                    break;
                case EPowerUpKind.GODLIKE:
                    UpdateAbility(ref powerUpGodLikeIndicator);
                    break;
            }
        }

        private void UpdateAbility(ref GameObject[] powerUpIndicators)
        {
            for (int i = 0; i < 3; i++)
            {
                powerUpIndicators[i].SetActive(i == GameManager.Instance.GetUIManager().PowerUpState);
            }
        }

        private void UpdateBossHealthbar()
        {
            if (controller.IsDead() || controller == null)
                showBossHealth = false;
            bossHealthSlider.value = controller.currentHealth;
        }

        private void UpdatePlayerHelathbar()
        {
            if (playerHealthSlider != null)
            {
                playerHealthSlider.maxValue = GameManager.PlayerController.maxHealth;
                playerHealthSlider.value = GameManager.PlayerController.currentHealth;
            }
        }

        public void ShowBossHealth(string bossName, BaseController controller)
        {
            bossHealth.SetActive(true);
            bossNameText.text = bossName + ":";
            this.controller = controller;
            bossHealthSlider.maxValue = controller.ctrlData.maxHealth;
            showBossHealth = true;
        }

        public void HideBossHealth()
        {
            bossHealth.SetActive(false);
        }


        public void ShowTeleportAbility()
        {
        }
    }
}