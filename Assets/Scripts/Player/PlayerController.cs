using System.Collections;
using Controllers;
using Controllers.Damage;
using Enums;
using Interactions.Enums;
using Interactions.Interfaces;
using Misc;
using UnityEngine;

namespace Player
{
    public class PlayerController :
        /// @cond SKIP_THIS
        MonoBehaviour,
        /// @endcond
        IDamageable
    {
        private EffectController _effectController;
        [SerializeField] private Animator characterAnimator;

        private PlayerCombatController combatCtrl;
        [SerializeField] private AudioSource footStep1;
        [SerializeField] private AudioSource footStep2;
        [SerializeField] private float generalSpeed = 40f;
        private float horizontalMove;
        private bool jump;
        private float lastAngularVelocity;
        private Vector2 lastVelocity;

        [SerializeField] private int maxHealth = 150, currentHealth;

        internal bool moveOnDamaged;
        private float moveOnDamagedStartTime;
        [SerializeField] private float moveOnDamageDuration = 1f;
        private PlayerMovementController playerMovementCtrl;
        internal bool praying = true;
        internal bool respawning;
        private Rigidbody2D rigidbody2D;
        internal bool roll;
        private bool savedRigidData;
        private float verticalMove;

        public void Damage(float amount, int attackN)
        {
            // old
        }

        public void Damage(BaseController controller, DamageInfo damageInfo)
        {
            if (!roll)
            {
                // Obtenemos la posicion del ataque
                int direction = damageInfo.GetAttackDirection(transform.position.x);

                currentHealth -= damageInfo.DamageAmount;

                if (damageInfo.MoveOnAttack)
                    MoveOnDamaged(direction, damageInfo.MoveOnAttackForce);

                if (damageInfo.Slow)
                {
                    AddEffect(EEffectKind.SLOWDOWN, damageInfo.slowDuration);
                    _effectController.SetEffectSlowDownActive(damageInfo.slowDuration);
                }
            }
        }

        public void Kill()
        {
            // old
        }


        private void Start()
        {
            var SpawnPoint = GameObject.Find("SpawnPoint");
            rigidbody2D = GetComponent<Rigidbody2D>();
            _effectController = GetComponent<EffectController>();
            transform.position = SpawnPoint.transform.position;
            respawning = true;
            AnimStartPrayingEvt();

            currentHealth = maxHealth;

            GameManager.Instance.GetPlayerControls().Gameplay.Roll.performed += ctxRoll =>
            {
                if (horizontalMove != 0f && !GameManager.Instance.IsGamePaused())
                {
                    roll = true;
                    characterAnimator.SetBool("Roll", true);
                }
            };
            GameManager.Instance.GetPlayerControls().Gameplay.Jump.performed += ctxRoll =>
            {
                if (!GameManager.Instance.IsGamePaused())
                    jump = true;
            };

            // Interaction input
            GameManager.Instance.GetPlayerControls().Gameplay.Interact.performed += ctx =>
            {
                if (canPlayerInteract && playerMovementCtrl.Grounded)
                {
                    //    Si el jugador puede interactuar llama a la interfaz del jugador
                    if (lastInteractiveController != null && canPlayerInteract)
                    {
                        EnterInteractionMode();
                        lastInteractiveController.Interact(this, EInteractionKind.Dialogue);
                    }
                }
            };
        }

        public void ExitInteractionMode()
        {
            isEnrolledInDialogue = false;
            GameManager.Instance.SetCameraSize(10f, 0.3f);
        }

        private void Awake()
        {
            playerMovementCtrl = GetComponent<PlayerMovementController>();
            combatCtrl = GetComponent<PlayerCombatController>();
        }

        internal void OnEnable()
        {
            GameManager.Instance.GetPlayerControls().Enable();
        }

        internal void OnDisable()
        {
            GameManager.Instance.GetPlayerControls().Disable();
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

            CheckInteractions();
            CheckMoveOnDamaged();

            horizontalMove = GameManager.Instance.GetPlayerControls().Gameplay.Horizontal.ReadValue<float>() *
                             generalSpeed;
            verticalMove = GameManager.Instance.GetPlayerControls().Gameplay.Vertical.ReadValue<float>() * generalSpeed;

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

            jump = false;
            roll = false;
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
            GameManager.Instance.SetCameraSize(6.5f, 0.5f);
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
            OnDisable();

            AnimStartPrayingEvt();

            respawning = true;
            var SpawnPoint = GameObject.Find("SpawnPoint");
            combatCtrl.ResetAttackNow();
            yield return new WaitForSeconds(1f);
            playerMovementCtrl.PrepareRespawn(SpawnPoint.transform);
            yield return new WaitForSeconds(2f);

            GameManager.Instance.RespawnPlayer();
            respawning = false;

            OnEnable();
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
            roll = false;
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
    }
}