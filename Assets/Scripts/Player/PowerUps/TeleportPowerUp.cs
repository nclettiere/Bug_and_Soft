using System;
using UnityEngine;

namespace Player
{
    public class TeleportPowerUp : PlayerPowerUp
    {
        private volatile float tpCooldownTime = float.NegativeInfinity;
        private volatile float tpMaxWaitTime = float.NegativeInfinity;
        private const float tpCooldown = 5f;
        private const float tpCooldownFirstPhase = .5f;

        private uint currentPhase;
        private Vector2 targetWarp;

        private bool awaitingConfirmation;

        public TeleportPowerUp() : base(EPowerUpKind.TELEPORT)
        {
        }

        public override void Enter()
        {
            base.Enter();
            tpCooldownTime = float.NegativeInfinity;
            currentPhase = 0;
            awaitingConfirmation = false;
        }

        public override void Exit()
        {
            tpCooldownTime = float.NegativeInfinity;
            currentPhase = 0;
        }

        public override void OnUpdate()
        {
            if (Time.time >= tpMaxWaitTime && currentPhase == 1)
            {
                tpCooldownTime = Time.time + tpCooldown;
                GameManager.Instance.GetUIManager().ChangePowerUpState(2);
            }else if (Time.time <= tpMaxWaitTime && currentPhase == 1)
            {
                GameManager.Instance.GetUIManager().ChangePowerUpState(1);
            }

            if (Time.time >= tpCooldownTime)
            {
                onCooldown = false;
                if (currentPhase == 0)
                    GameManager.Instance.GetUIManager().ChangePowerUpState(0);
            }

            if (awaitingConfirmation)
                GameManager.Instance.GetUIManager().ChangePowerUpState(1);
            
            // Si se excede el limite de tiempo ..
            if (awaitingConfirmation && Time.time >= tpMaxWaitTime)
            {
                GameManager.Instance.GetUIManager().ChangePowerUpState(2);
                tpCooldownTime = Time.time + tpCooldown;
                tpMaxWaitTime = float.NegativeInfinity;
                currentPhase = 0;
                awaitingConfirmation = false;
                onCooldown = true;
            }
            
            if (currentPhase == 0 && Time.time >= tpCooldownTime)
            {
                onCooldown = false;
                GameManager.Instance.GetUIManager().ChangePowerUpState(0);
            }

            if (activated)
            {
                if (Time.time >= tpCooldownTime)
                {
                    if (currentPhase == 0)
                    {
                        onCooldown = true;
                        targetWarp = GameManager.Instance.PlayerController.transform.position;
                        tpCooldownTime = Time.time + tpCooldownFirstPhase;
                        tpMaxWaitTime = Time.time + tpCooldown;
                        awaitingConfirmation = true;

                        GameManager.Instance.GetUIManager().ChangePowerUpState(1);
                        currentPhase = 1;
                    }
                    else if (currentPhase == 1)
                    {
                        onCooldown = true;
                        GameManager.Instance.PlayerController.transform.position = targetWarp;
                        tpCooldownTime = Time.time + tpCooldown;
                        awaitingConfirmation = false;
                        
                        GameManager.Instance.GetUIManager().ChangePowerUpState(2);
                        currentPhase = 0;
                    }
                }

                activated = false;
            }
        }
    }
}