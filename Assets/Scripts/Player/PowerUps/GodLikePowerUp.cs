using System;
using Controllers.Damage;
using UnityEngine;

namespace Player
{
    public class GodLikePowerUp : PlayerPowerUp
    {
        private volatile float tpCooldownTime = float.NegativeInfinity;
        private const float tpCooldown = 5f;
        private Vector2 targetWarp;

        public GodLikePowerUp() : base(EPowerUpKind.GODLIKE)
        {
        }

        public override void Enter()
        {
            base.Enter();
            tpCooldownTime = float.NegativeInfinity;
        }

        public override void Exit()
        {
            tpCooldownTime = float.NegativeInfinity;
        }

        public override void OnUpdate()
        {
            if (activated)
            {
                if (Time.time >= tpCooldownTime)
                {
                    BurnEnemies();
                    tpCooldownTime = Time.time + tpCooldown;
                }

                activated = false;
            }
        }

        private void BurnEnemies()
        {
            DamageInfo damageInfo = new DamageInfo(1000, 0f, true);
            foreach (var controller in GameManager.Instance.EnemiesInScreen)
            {
                controller.Damage(damageInfo);
            }
        }
    }
}