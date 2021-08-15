using System;
using TMPro;
using UnityEngine;

namespace UI
{
    public class UI_HUD : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI krownsValue;

        private void Update()
        {
            krownsValue.text = GameManager.Instance.PlayerKrowns.ToString();
        }
    }
}