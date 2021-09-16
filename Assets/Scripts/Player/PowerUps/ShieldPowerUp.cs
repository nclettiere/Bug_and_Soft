using System;
using UnityEngine;

namespace Player
{
    public class ShieldPowerUp : PlayerPowerUp
    {
        private volatile float shieldCooldownTime = float.NegativeInfinity;
        private const float shieldCooldown = 5f;

        private bool isActive;

        public ShieldPowerUp()
        {
            _powerUpKind = EPowerUpKind.SHIELD;
        }

        public override void Enter()
        {
            base.Enter();
            shieldCooldownTime = float.NegativeInfinity;
            isActive = true;
        }

        public override void Exit()
        {
            shieldCooldownTime = float.NegativeInfinity;
        }

        public override void OnUpdate()
        {
            if (Time.time >= shieldCooldownTime)
            {
                isActive = true;
            }
        }

        public bool IsActive()
        {
            return isActive;
        }

        public void BrokeShield()
        {
            Debug.Log("Shield Broke!");
            isActive = false;
            shieldCooldownTime = Time.time + shieldCooldown;
        }
    }
}