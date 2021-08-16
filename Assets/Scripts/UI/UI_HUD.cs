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
        
        [SerializeField] private GameObject[] abilities;
        [SerializeField] private GameObject[] abilitiesEmpty;
        
        private bool showBossHealth;
        private BaseController controller;
        
        
        private int ROMHOPP_state = -1;

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

        private void UpdateAbilities()
        {
            switch (ROMHOPP_state)
            {
                case -1:
                    abilities[0].SetActive(false);
                    abilities[1].SetActive(false);
                    abilities[2].SetActive(false);
                    abilitiesEmpty[0].SetActive(true);
                    break;
                case 0:
                    abilitiesEmpty[0].SetActive(false);
                    abilities[0].SetActive(true);
                    abilities[1].SetActive(false);
                    abilities[2].SetActive(false);
                    break;
                case 1:
                    abilitiesEmpty[0].SetActive(false);
                    abilities[0].SetActive(false);
                    abilities[1].SetActive(true);
                    abilities[2].SetActive(false);
                    break;
                case 2:
                    abilitiesEmpty[0].SetActive(false);
                    abilities[0].SetActive(false);
                    abilities[1].SetActive(false);
                    abilities[2].SetActive(true);
                    break;
            }
            
        }

        public void SetRomhoppState(int newState)
        {
            ROMHOPP_state = newState;
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
                playerHealthSlider.maxValue = GameManager.Instance.PlayerController.maxHealth;
                playerHealthSlider.value = GameManager.Instance.PlayerController.currentHealth;
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
        
        
    }
}