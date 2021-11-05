using System;
using UnityEngine;

namespace Player
{
    public class ShieldPowerUp : PlayerPowerUp
    {
        private volatile float shieldCooldownTime = float.NegativeInfinity;
        private const float shieldCooldown = 5f;

        private bool isActive;
        
        private GameObject shieldPlayerIndicator;

        public ShieldPowerUp(GameObject shieldPlayerIndicator) : base(EPowerUpKind.SHIELD)
        {
            this.shieldPlayerIndicator = shieldPlayerIndicator;
        }

        public override void Enter()
        {
            base.Enter();
            shieldCooldownTime = float.NegativeInfinity;
            isActive = true;
            shieldPlayerIndicator = GameManager.Instance.PlayerController.shieldPlayerIndicator;
            shieldPlayerIndicator.SetActive(true);
        }

        public override void Exit()
        {
            shieldCooldownTime = float.NegativeInfinity;
            shieldPlayerIndicator.SetActive(false);
        }

        public override void OnUpdate()
        {
            if (Time.time >= shieldCooldownTime)
            {
                isActive = true;
                onCooldown = false;
                GameManager.Instance.GetUIManager().ChangePowerUpState(0);
                shieldPlayerIndicator.SetActive(true);
            }
            else
            {
                onCooldown = true;
            }
        }

        public bool IsActive()
        {
            return isActive;
        }

        public void BrokeShield()
        {
            isActive = false;
            shieldCooldownTime = Time.time + shieldCooldown;
            GameManager.Instance.GetUIManager().ChangePowerUpState(2);
            shieldPlayerIndicator.SetActive(false);
        }
    }
}