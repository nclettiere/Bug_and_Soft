using System;
using UnityEngine;

namespace Player
{
    public class TeleportPowerUp : PlayerPowerUp
    {
        private volatile float tpCooldownTime = float.NegativeInfinity;
        private const float tpCooldown = 5f;
        private const float tpCooldownFirstPhase = .5f;

        private uint currentPhase;

        private Vector2 targetWarp;
        
        public TeleportPowerUp()
        {
            _powerUpKind = EPowerUpKind.TELEPORT;
        }

        public override void Enter()
        {
            tpCooldownTime = float.NegativeInfinity;
            currentPhase = 0;
        }

        public override void Exit()
        {
            tpCooldownTime = float.NegativeInfinity;
            currentPhase = 0;
        }
        
        public override void OnUpdate()
        {
            if (activated)
            {
                if (Time.time >= tpCooldownTime)
                {
                    if (currentPhase == 0)
                    {
                        targetWarp = GameManager.PlayerController.transform.position;
                        tpCooldownTime = Time.time + tpCooldownFirstPhase;
                        
                        currentPhase = 1;
                    }else if (currentPhase == 1)
                    {
                        GameManager.PlayerController.transform.position = targetWarp;
                        tpCooldownTime = Time.time + tpCooldown;
                        
                        currentPhase = 0;
                    }
                }

                activated = false;
            } 
        }
    }
}