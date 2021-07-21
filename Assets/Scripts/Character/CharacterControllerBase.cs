using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;

namespace Character
{
    public class CharacterControllerBase : MonoBehaviour
    {
        [SerializeField]
        [Range(0.15f, 1f)] public float NextAttackMaxTime = 0.75f;
        [SerializeField]
        private int UnlockedAttacksCount = 2;
        [SerializeField]
        private float generalSpeed = 40f;
        [SerializeField]
        private Animator characterAnimator;
        [SerializeField]
        private AudioSource footStep1;
        [SerializeField]
        private AudioSource footStep2;
        [SerializeField]
        private AudioSource[] swordSwingSFX;
        [SerializeField]
        private float inputCooldown;

        protected CharacterMovement characterMovement;
        private bool climbLadderMechanicEnabled;
        private LadderClimbing ladderClimbing;
        private float horizontalMove = 0f;
        private float verticalMove = 0f;
        private bool jump = false;
        private bool roll = false;
        private bool praying = true;
        private bool attacking = false;
        private bool attackingMov = false; // Used in CharacterMovement
        private bool awaitingAttack = true;
        private bool respawning = false;
        private bool hasAttackAnimationStarted = false;
        private int attackN = 0;
        private float lastInputTime = float.NegativeInfinity;
        private float lastPrayDelay = float.NegativeInfinity;

        PlayerControls playerControls;

        private void Start()
        {
            GameObject SpawnPoint = GameObject.Find("SpawnPoint");
            transform.position = SpawnPoint.transform.position;
            respawning = true;
            AnimStartPrayingEvt();
        }

        void Awake()
        {
            playerControls = new PlayerControls();
            characterMovement = gameObject.GetComponent(typeof(CharacterMovement)) as CharacterMovement;

            playerControls.Gameplay.Roll.performed += ctxRoll =>
            {
                if (horizontalMove != 0f)
                {
                    roll = true;
                    characterAnimator.SetBool("Roll", true);
                }
            };
            playerControls.Gameplay.Jump.performed += ctxRoll =>
            {
                jump = true;
            };
            playerControls.Gameplay.Attack.performed += ctx =>
            {
                if (!respawning && !praying && !roll && !characterMovement.IsTouchingLedge)
                {
                    if (awaitingAttack)// && Time.time >= lastInputTime)
                    {
                        StopAllCoroutines();

                        attacking = true;
                        attackN++;
                        awaitingAttack = false;
                        attackingMov = true;

                        characterAnimator.SetBool("AwaitingAttack", false);
                        characterAnimator.SetBool("Attacking", true);
                        characterAnimator.SetInteger("AttackN", attackN);

                        lastInputTime = Time.time + inputCooldown;
                    }
                }
            };
        }

        private void OnEnable()
        {
            playerControls.Enable();
        }

        private void OnDisable()
        {
            playerControls.Disable();
        }

        void Update()
        {
            if (!GameManager.Instance.IsPlayerAlive &&
                GameManager.Instance.PlayerDeathCount > 0)
            {
                StartCoroutine(Respawn());
                horizontalMove = 0f;
                return;
            }
            if (!GameManager.Instance.isInputEnabled)
                return;

            if (respawning || praying)
            {
                if (horizontalMove > 0.01f)
                {
                    praying = false;
                    characterAnimator.SetBool("Praying", false);
                    respawning = false;
                }
            }

            horizontalMove = playerControls.Gameplay.Horizontal.ReadValue<float>() * generalSpeed;
            verticalMove = playerControls.Gameplay.Vertical.ReadValue<float>() * generalSpeed;

            characterAnimator.SetFloat("Speed", Mathf.Abs(horizontalMove));
        }

        void FixedUpdate()
        {
            characterMovement.Move(
                horizontalMove * Time.fixedDeltaTime,
                verticalMove * Time.fixedDeltaTime,
                false,
                jump,
                roll,
                attackingMov,
                attackN * 2);

            jump = false;
            roll = false;
            attackingMov = false;
        }

        public void LandEvt()
        {
            characterAnimator.SetBool("OnLand", true);
        }

        public void OnAirEvt()
        {
            characterAnimator.SetBool("OnLand", false);
        }

        // ============================
        // Animation event callbacks
        // ==================
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

        /// <summary>
        /// Accionado cuando la animacion del ataque termina
        /// Se pasa a esperar al siguiente ataque en el combo loco (awaitingAttack)
        /// </summary>
        public void AnimAttackEndEvt()
        {
            attacking = false;
            awaitingAttack = true;
            hasAttackAnimationStarted = false;
            characterAnimator.SetBool("Attacking", false);
            characterAnimator.SetBool("AwaitingAttack", awaitingAttack);
            characterAnimator.SetBool("HasAttackAnimationStarted", false);

            if (attackN + 1 > UnlockedAttacksCount)
            {
                ResetAttackNow();
                return;
            }

            StartCoroutine(ResetAttack());
        }

        /// <summary>
        /// Accionado cuando la animacion de la espada hace un 'swing'
        /// </summary>
        public void AnimAttackSwingEvt()
        {
            if ((attackN - 1) >= 0 && (attackN - 1) <= swordSwingSFX.Length)
                swordSwingSFX[attackN - 1].Play();

            hasAttackAnimationStarted = true;
            characterAnimator.SetBool("HasAttackAnimationStarted", hasAttackAnimationStarted);
        }


        public void AnimHasAttackAnimationStartedEvt()
        {
            hasAttackAnimationStarted = true;
            characterAnimator.SetBool("HasAttackAnimationStarted", hasAttackAnimationStarted);
        }

        private IEnumerator Respawn()
        {
            this.OnDisable();

            AnimStartPrayingEvt();

            respawning = true;
            GameObject SpawnPoint = GameObject.Find("SpawnPoint");
            ResetAttackNow();
            yield return new WaitForSeconds(1f);
            characterMovement.PrepareRespawn(SpawnPoint.transform);
            yield return new WaitForSeconds(2f);

            GameManager.Instance.RespawnPlayer();
            respawning = false;

            this.OnEnable();
        }

        /// <summary>
        /// Resetea el ataque del jugador si no hace un combo completo !!!
        /// </summary>
        private IEnumerator ResetAttack()
        {
            /// Se le da un delay al jugador para que responda
            yield return new WaitForSeconds(NextAttackMaxTime);

            // Si ha transcurrido el tiempo y no se ha accionado ningun attack,
            // se resetea el ataque
            ResetAttackNow();
        }

        private void ResetAttackNow()
        {

            attacking = false;
            attackN = 0;
            hasAttackAnimationStarted = false;
            characterAnimator.SetBool("AwaitingAttack", true);
            characterAnimator.SetBool("HasAttackAnimationStarted", false);
            characterAnimator.SetBool("Attacking", false);
            characterAnimator.SetInteger("AttackN", attackN);

        }
    }
}