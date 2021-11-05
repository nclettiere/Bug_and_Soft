using System.Collections;
using System.Collections.Generic;
using Controllers;
using Controllers.Damage;
using Enums;
using Interactions.Enums;
using Interactions.Interfaces;
using Misc;
using SaveSystem.Data;
using UnityEngine;

namespace Player
{
    public class PlayerController :
        /// @cond SKIP_THIS
        MonoBehaviour,
        /// @endcond
        IDamageable
    {
        [SerializeField] private Animator characterAnimator;

        public PlayerCombatController combatCtrl { get; private set; }
        [SerializeField] private AudioSource footStep1;
        [SerializeField] private AudioSource footStep2;
        [SerializeField] private float generalSpeed = 40f;
        [SerializeField] public int currentHealth;
        [SerializeField] private float moveOnDamageDuration = 1f;
        [SerializeField] private GameObject onActivateWarpEffect;
        [SerializeField] private GameObject onWarpEffectParticle;
        [SerializeField] private AudioSource onWarpActionedSFX;
        [SerializeField] private AudioSource onWarpedSFX;
        [SerializeField] private AudioSource onRewardAdded;
        [SerializeField] private AudioSource onKrownRemove;
        [SerializeField] private AudioSource onPlayerHurtSFX;
        public GameObject shieldPlayerIndicator;
        [SerializeField] private Transform pepeTarget;
        public Transform VergenTrapTarget;

        public int maxHealth = 150; // health por defecto

        private float horizontalMove;
        private bool jump;
        private float lastAngularVelocity;
        private Vector2 lastVelocity;
        internal bool moveOnDamaged;
        private float moveOnDamagedStartTime;
        private PlayerMovementController playerMovementCtrl;
        internal bool praying = true;
        internal bool respawning;
        private Rigidbody2D rigidbody2D;
        internal bool roll;
        internal bool rollAnim;
        private bool savedRigidData;
        private float verticalMove;
        private EffectController effectController;
        private float rollCooldownWait = float.NegativeInfinity;

        private uint currentLvL;
        private uint maxExperience;
        private uint currentExperience;

        public float hozSpeed { get; private set; }
        public float verSpeed { get; private set; }

        public int MaxMagicka { get; private set; } = 250;
        public int CurrentMagicka { get; private set; } = 250;
        public int MagickaRegenTime { get; private set; } = 12;

        // Power Ups Manager
        public PlayerPowerUpsProgression powerUps { get; private set; }

        // ROMHOPP
        public TeleportPowerUp teleportPowerUp { get; private set; }

        // SHIELD
        public ShieldPowerUp shieldPowerUp { get; private set; }

        // GODLIKE
        public GodLikePowerUp godLikePowerUp { get; private set; }

        public void Damage(BaseController controller, DamageInfo damageInfo)
        {
            if (!rollAnim)
            {
                if (shieldPowerUp.IsActive() && powerUps.currentPowerUp == shieldPowerUp)
                {
                    shieldPowerUp.BrokeShield();
                    return;
                }
                
                // Obtenemos la posicion del ataque
                int direction = damageInfo.GetAttackDirection(transform.position.x);

                currentHealth -= damageInfo.DamageAmount;

                onPlayerHurtSFX.Play();

                StartCoroutine(DamageEffect());

                if (damageInfo.MoveOnAttack)
                    MoveOnDamaged(direction, damageInfo.MoveOnAttackForce);

                if (currentHealth <= 0)
                {
                    GameManager.Instance.GameOver();
                }

                if (damageInfo.Slow)
                {
                    AddEffect(EEffectKind.SLOWDOWN, damageInfo.slowDuration);
                    effectController.SetEffectSlowDownActive(damageInfo.slowDuration);
                }
            }
        }

        public void Damage(DamageInfo damageInfo)
        {
            if (!rollAnim)
            {
                if (!damageInfo.isLava && shieldPowerUp.IsActive() && powerUps.currentPowerUp == shieldPowerUp)
                {
                    shieldPowerUp.BrokeShield();
                    return;
                }

                // Obtenemos la posicion del ataque
                int direction = damageInfo.GetAttackDirection(transform.position.x);

                currentHealth -= damageInfo.DamageAmount;

                onPlayerHurtSFX.Play();
                StartCoroutine(DamageEffect());
                
                if (damageInfo.MoveOnAttack)
                    MoveOnDamaged(direction, damageInfo.MoveOnAttackForce);

                if (currentHealth <= 0)
                {
                    GameManager.Instance.GameOver();
                }

                if (damageInfo.Slow)
                {
                    AddEffect(EEffectKind.SLOWDOWN, damageInfo.slowDuration);
                    effectController.SetEffectSlowDownActive(damageInfo.slowDuration);
                }
            }
        }

        private IEnumerator DamageEffect()
        {
            GetComponent<SpriteRenderer>()
                .color = Color.red;
            yield return new WaitForSeconds(0.15f);
            GetComponent<SpriteRenderer>()
                .color = Color.white;
            yield return 0;
        }

        private void Start()
        {
            var SpawnPoint = GameObject.Find("SpawnPoints/LevelOneDefaultSpawn");
            rigidbody2D = GetComponent<Rigidbody2D>();
            effectController = GetComponent<EffectController>();
            transform.position = SpawnPoint.transform.position;
            respawning = true;
            AnimStartPrayingEvt();

            currentHealth = maxHealth;

            powerUps = new PlayerPowerUpsProgression();
            teleportPowerUp = new TeleportPowerUp();
            shieldPowerUp = new ShieldPowerUp(shieldPlayerIndicator);
            godLikePowerUp = new GodLikePowerUp();

            // A implementar en el tercer sprint
            // Cargar el savegame e inicializar con la habilidad guardada

            if (GameManager.Instance.UnlockedPowerUps.Count > 0)
            {
                powerUps.Initialize(GameManager.Instance.UnlockedPowerUps[0]);
            }

            GameManager.Instance.isGameOver = false;
            
            GameManager.Instance.OnLevelReset.AddListener(ResetStats);
            GameManager.Instance.OnVergenTrappedPlayer.AddListener(VergenTrapStart);
        }

        private void ResetStats()
        {
            currentExperience = 0;
        }

        public void ExitInteractionMode()
        {
            isEnrolledInDialogue = false;
        }

        private void Awake()
        {
            playerMovementCtrl = GetComponent<PlayerMovementController>();
            combatCtrl = GetComponent<PlayerCombatController>();
        }

        private void Update()
        {
            if (GameManager.Instance.IsGamePaused() || isEnrolledInDialogue)
            {
                if (!savedRigidData)
                    FreezePlayer();
                return;
            }

            if (savedRigidData)
                UnfreezePlayer();

            if (!GameManager.Instance.IsPlayerAlive() &&
                GameManager.Instance.GetPlayerDeathCount() > 0)
            {
                StartCoroutine(Respawn());
                horizontalMove = 0f;
                return;
            }

            if (!GameManager.Instance.GetIsInputEnabled())
                return;

            if (respawning || praying)
            {
                if (horizontalMove > 0.01f || horizontalMove < 0.01f)
                {
                    praying = false;
                    characterAnimator.SetBool("Praying", false);
                    respawning = false;
                }
            }

            if (powerUps.currentPowerUp != null)
                powerUps.currentPowerUp.OnUpdate();

            CheckInteractions();
            CheckMoveOnDamaged();

            horizontalMove = hozSpeed * generalSpeed;
            verticalMove = verSpeed * generalSpeed;

            characterAnimator.SetFloat("Speed", Mathf.Abs(horizontalMove));
        }

        private void FixedUpdate()
        {
            if (GameManager.Instance.IsGamePaused() || isEnrolledInDialogue || moveOnDamaged) return;

            playerMovementCtrl.Move(
                horizontalMove * GameManager.Instance.DeltaTime, //Time.fixedDeltaTime,
                verticalMove * GameManager.Instance.DeltaTime, //Time.fixedDeltaTime,
                false,
                jump,
                roll,
                combatCtrl.attacking);

            if (GameManager.Instance.IsGamePaused()) return;
            roll = false;
            jump = false;
        }

        private void OnDrawGizmos()
        {
            // Interaction gizmo
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, interactionRadius);
        }

        private void CheckInteractions()
        {
            RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, interactionRadius, new Vector2(0f, 0f));

            foreach (var hit in hits)
            {
                if (hit.collider.tag.Equals("NPC"))
                {
                    BaseController bctrl = hit.transform.GetComponent<BaseController>();
                    if (bctrl is IInteractive)
                    {
                        lastInteractiveController = bctrl;
                        bctrl.ReadyToInteract(this, true);
                    }

                    canPlayerInteract = true;
                    return;
                }
            }

            // Cancel all ReadyToInteract events
            canPlayerInteract = false;
            if (lastInteractiveController != null)
                lastInteractiveController.ReadyToInteract(this, false);
        }

        private void MoveOnDamaged(int direction, Vector2 moveOnDamagedForce)
        {
            moveOnDamaged = true;
            moveOnDamagedStartTime = Time.time;
            rigidbody2D.AddForce(new Vector2(moveOnDamagedForce.x * direction, moveOnDamagedForce.y));
            //rigidbody2D.velocity = new Vector2(moveOnDamagedForce.x * direction, moveOnDamagedForce.y);
        }

        void CheckMoveOnDamaged()
        {
            if (moveOnDamaged && Time.time >= moveOnDamagedStartTime + moveOnDamageDuration)
            {
                moveOnDamaged = false;
                rigidbody2D.velocity = new Vector2(0.0f, rigidbody2D.velocity.y);
            }
        }

        private void FreezePlayer()
        {
            lastVelocity = rigidbody2D.velocity;
            lastAngularVelocity = rigidbody2D.angularVelocity;
            rigidbody2D.bodyType = RigidbodyType2D.Static;
            rigidbody2D.Sleep();

            savedRigidData = true;
        }

        private void UnfreezePlayer()
        {
            rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
            rigidbody2D.velocity = lastVelocity;
            rigidbody2D.angularVelocity = lastAngularVelocity;
            rigidbody2D.WakeUp();
            savedRigidData = false;
        }

        public void EnterInteractionMode()
        {
            isEnrolledInDialogue = true;
        }

        public int GetFacingDirection()
        {
            return (playerMovementCtrl.IsFacingRight() ? 1 : -1);
        }

        public void LandEvt()
        {
            characterAnimator.SetBool("OnLand", true);
        }

        public void OnAirEvt()
        {
            characterAnimator.SetBool("OnLand", false);
        }

        public bool IsFacingRight()
        {
            return playerMovementCtrl.IsFacingRight();
        }

        private IEnumerator Respawn()
        {
            //OnDisable();

            AnimStartPrayingEvt();

            respawning = true;

            combatCtrl.ResetAttackNow();
            yield return new WaitForSeconds(1f);
            playerMovementCtrl.PrepareRespawn(GameManager.Instance.spawnPoint);
            yield return new WaitForSeconds(2f);

            GameManager.Instance.RespawnPlayer();
            respawning = false;

            //OnEnable();
        }


        public void AddEffect(EEffectKind kind, float lifetime)
        {
            switch (kind)
            {
                case EEffectKind.SLOWDOWN:
                    playerMovementCtrl.AddSpeedPenalty(lifetime);
                    break;
            }
        }

        public void AddExperience(uint experience)
        {
            if(currentExperience + experience >= maxExperience)
            {
                
            }
            
            //private uint currentLvL;
            //private uint maxExperience;
            //private uint currentExperience;
        }

        #region Interaction

        [SerializeField] protected float interactionRadius = 3f;
        private bool canPlayerInteract = false;
        private BaseController lastInteractiveController;
        public bool isEnrolledInDialogue = false;

        #endregion

        #region AnimationCallbacks

        public void AnimStartPrayingEvt()
        {
            praying = true;
            characterAnimator.SetBool("Praying", true);
        }

        public void AnimRollEndEvt()
        {
            rollAnim = false;
            characterAnimator.SetBool("Roll", false);
        }

        public void AnimFootStep1Evt()
        {
            footStep1.Play();
        }

        public void AnimFootStep2Evt()
        {
            footStep2.Play();
        }

        #endregion

        public void KrownsAdded()
        {
            onRewardAdded.Play();
        }

        public void KrownsRemoved()
        {
            onKrownRemove.Play();
        }

        public void SetData(GameData gameData)
        {
            maxHealth = gameData.PlayerMaxHealth;
            currentHealth = gameData.PlayerHealth;
            
            if(powerUps == null)
                Debug.LogWarning("Power ups turns out to be null");
            
            // powerups
            if (gameData.unlockedPowerUps[0] == 1)
            {
                UnlockPowerUp(EPowerUpKind.TELEPORT, 0);
            }
            if (gameData.unlockedPowerUps[1] == 1)
            {
                UnlockPowerUp(EPowerUpKind.SHIELD, 0);
            }
            if (gameData.unlockedPowerUps[2] == 1)
            {
                UnlockPowerUp(EPowerUpKind.GODLIKE, 0);
            }

            if (gameData.currentPowerUp != -1)
            {
                if (gameData.currentPowerUp == 0)
                {
                    Debug.Log("Setting Teleport as current");
                    powerUps.Initialize(teleportPowerUp);
                    powerUps.ChangePowerUp(teleportPowerUp);
                    GameManager.Instance.GetUIManager().ChangePowerUpKind(EPowerUpKind.TELEPORT);
                    
                }
                if (gameData.currentPowerUp == 1)
                {
                    Debug.Log("Setting SHield as current");
                    powerUps.Initialize(shieldPowerUp);
                    powerUps.ChangePowerUp(shieldPowerUp);
                    GameManager.Instance.GetUIManager().ChangePowerUpKind(EPowerUpKind.SHIELD);
                }
                if (gameData.currentPowerUp == 2)
                {
                    Debug.Log("Setting GODLIKE as current");
                    powerUps.Initialize(godLikePowerUp);
                    powerUps.ChangePowerUp(godLikePowerUp);
                    GameManager.Instance.GetUIManager().ChangePowerUpKind(EPowerUpKind.GODLIKE);
                }
            }
            else
            {
                Debug.Log("Current pw is -1");
            }
            
            RefillHealth();
        }

        public void Rollear()
        {
            if (horizontalMove != 0f && !GameManager.Instance.IsGamePaused())
            {
                //if (Time.time >= rollCooldownWait)
                //{
                roll = true;
                rollAnim = true;
                characterAnimator.SetBool("Roll", true);
                //rollCooldownWait = Time.time + rollCooldown;
                //}
            }
        }

        public void Jumpear()
        {
            if (!GameManager.Instance.IsGamePaused())
                jump = true;
        }

        public void Interactear()
        {
            if (canPlayerInteract && playerMovementCtrl.Grounded)
            {
                //    Si el jugador puede interactuar llama a la interfaz del jugador
                if (lastInteractiveController != null && canPlayerInteract &&
                    lastInteractiveController.CanCharacterInteract())
                {
                    EnterInteractionMode();
                    lastInteractiveController.Interact(this, EInteractionKind.Dialogue);
                }
            }
        }

        public void RemoveMagicka(int amount)
        {
            if (CurrentMagicka - amount < 0)
                CurrentMagicka = 0;
            else
                CurrentMagicka -= amount;
        }

        public void RefillHealth()
        {
            currentHealth = maxHealth;
        }

        public void SetHorizontalSpeed(float speed)
        {
            hozSpeed = speed;
        }
        
        public void SetVerticalSpeed(float speed)
        {
            verSpeed = speed;
        }

        public Vector3 GetPepeTarget()
        {
            return pepeTarget.position;
        }

        public void RespawnNow()
        {
            hozSpeed = 0f;
            verSpeed = 0f;
            playerMovementCtrl.StopAllMovement();
            characterAnimator.SetBool("OnLand", true);
            if (!playerMovementCtrl.FacingRight)
                playerMovementCtrl.Flip();
            transform.position = GameManager.Instance.spawnPoint;
            
            
            GameManager.Instance.isGameOver = false;
        }

        public void UnlockPowerUp(EPowerUpKind kind, int krones)
        {
            switch (kind)
            {
                case EPowerUpKind.SHIELD:
                    GameManager.Instance.UnlockedPowerUps.Add(shieldPowerUp);
                    powerUps.ChangePowerUp(shieldPowerUp);
                    break;
                case EPowerUpKind.TELEPORT:
                    GameManager.Instance.UnlockedPowerUps.Add(teleportPowerUp);
                    powerUps.ChangePowerUp(teleportPowerUp);
                    break;
                case EPowerUpKind.GODLIKE:
                    GameManager.Instance.UnlockedPowerUps.Add(godLikePowerUp);
                    powerUps.ChangePowerUp(godLikePowerUp);
                    break;
            }
            
            GameManager.Instance.RemovePlayerKrowns(krones);
        }

        public void CyclePowerUps()
        {
            bool isNext = false;
            for (int i = 0; i < GameManager.Instance.UnlockedPowerUps.Count; i++)
            {
                if (isNext)
                {
                    powerUps.ChangePowerUp(GameManager.Instance.UnlockedPowerUps[i]);
                    break;
                }

                if (powerUps.currentPowerUp.powerUpKind == GameManager.Instance.UnlockedPowerUps[i].powerUpKind)
                {
                    if (i == GameManager.Instance.UnlockedPowerUps.Count - 1)
                    {
                        powerUps.ChangePowerUp(GameManager.Instance.UnlockedPowerUps[0]);
                        break;
                    }
                    isNext = true;
                }
            }
            foreach (PlayerPowerUp powerUp in GameManager.Instance.UnlockedPowerUps)
            {
            }
        }

        public void Heal(int healingAmount)
        {
            if (currentHealth + healingAmount > maxHealth)
                currentHealth = maxHealth;
            else
                currentHealth += healingAmount;
        }

        public void VergenTrapStart()
        {
            GameManager.Instance.gameInput.DisableInput();
        }

        public void OnVergenDie()
        {
            GameManager.Instance.gameInput.EnableInput();
            GameManager.Instance.GetDynamicCamera().ChangeTarget(transform);
        }

        public void OnLevel2_1Finish(Transform newCameraTarget)
        {
            GameManager.Instance.gameInput.DisableInput();
            GameManager.Instance.GetDynamicCamera().ChangeTarget(newCameraTarget);
            hozSpeed = 1;
            StartCoroutine(GameManager.Instance.ShowTransitionThree());
        }
    }
}