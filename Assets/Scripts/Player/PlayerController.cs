using System.Collections;
using Controllers;
using Interactions.Enums;
using Interactions.Interfaces;
using UnityEngine;

namespace Player
{
    public class PlayerController : 
        /// @cond SKIP_THIS
        MonoBehaviour
        /// @endcond
    {
        internal bool awaitingAttack = true;

        [SerializeField] 
        private Animator characterAnimator;
        private PlayerCombatController combatCtrl;
        [SerializeField] 
        private AudioSource footStep1;
        [SerializeField] 
        private AudioSource footStep2;
        [SerializeField] 
        private float generalSpeed = 40f;

        private float horizontalMove;

        private bool jump;
        private float lastAngularVelocity;
        private Vector2 lastVelocity;

        protected PlayerMovementController playerMovementCtrl;
        internal bool praying = true;
        internal bool respawning;
        private Rigidbody2D rigidbody2D;
        internal bool roll;
        private bool savedRigidData;
        private float verticalMove;

        #region Interaction
        [SerializeField]
        protected float interactionRadius = 3f;
        private bool canPlayerInteract = false;
        private BaseController lastInteractiveController;
        public bool isEnrolledInDialogue = false;
        #endregion

        private void Start()
        {
            var SpawnPoint = GameObject.Find("SpawnPoint");
            rigidbody2D = GetComponent<Rigidbody2D>();
            transform.position = SpawnPoint.transform.position;
            respawning = true;
            AnimStartPrayingEvt();
            
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

            horizontalMove = GameManager.Instance.GetPlayerControls().Gameplay.Horizontal.ReadValue<float>() *
                             generalSpeed;
            verticalMove = GameManager.Instance.GetPlayerControls().Gameplay.Vertical.ReadValue<float>() * generalSpeed;

            characterAnimator.SetFloat("Speed", Mathf.Abs(horizontalMove));
        }

        private void FixedUpdate()
        {
            if (GameManager.Instance.IsGamePaused() || isEnrolledInDialogue) return;
            
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
            if(lastInteractiveController != null)
                lastInteractiveController.ReadyToInteract(this, false);
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