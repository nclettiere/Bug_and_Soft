using System;
using UnityEngine;

namespace Items
{
    public class HealthPotion : Item
    {
        [SerializeField] private bool isPercenatage;
        [SerializeField] private int healingAmount = 50;
        
        public override void Use()
        {
            base.Use();
            GameManager.Instance.PlayerController
                .Heal(healingAmount);
        }
    }
}