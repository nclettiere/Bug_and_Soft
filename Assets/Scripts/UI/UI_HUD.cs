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
        [SerializeField] private Slider bossHealthSlider;
        private bool showBossHealth;
        private BaseController controller;

        private void Update()
        {
            krownsValue.text = GameManager.Instance.PlayerKrowns.ToString();

            if (showBossHealth)
            {
                if (controller.IsDead() || controller == null)
                    showBossHealth = false;
                bossHealthSlider.value = controller.currentHealth;
            }
        }

        public void ShowBossHealth(string bossName, BaseController controller)
        {
            bossHealth.SetActive(true);
            bossNameText.text = bossName + ":";
            this.controller = controller;
            bossHealthSlider.maxValue = controller.maxHealth;
            showBossHealth = true;
        }

        public void HideBossHealth()
        {
            bossHealth.SetActive(false);
        }
    }
}