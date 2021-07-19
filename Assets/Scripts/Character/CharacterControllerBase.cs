using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;

namespace Character
{
    public class CharacterControllerBase : MonoBehaviour
    {
        protected CharacterMovement characterMovement;
        protected SpriteRenderer characterSprite;
        private float horizontalMove = 0f;
        private float verticalMove = 0f;
        private bool jump = false;
        private bool roll = false;
        private bool praying = true;
        private bool attacking = false;
        private bool attackingMov = false; // Used in CharacterMovement
        private bool awaitingAttack = true;
        private bool requestedAttack;
        private int attackN = 0;

        [Range(0.15f, 1f)] public float WaitTimeForAttack = 0.75f;
        public int UnlockedAttacksCount = 2;

        [SerializeField] private float generalSpeed = 40f;
        [SerializeField] private Animator characterAnimator;

        [SerializeField] private AudioSource footStep1;
        [SerializeField] private AudioSource footStep2;

        [SerializeField] private AudioSource[] swordSwingSFX;

        private bool recentlyRespawned = true;

        PlayerControls playerControls;

        private void Start()
        {
            GameObject SpawnPoint = GameObject.Find("SpawnPoint");
            transform.position = SpawnPoint.transform.position;
            recentlyRespawned = true;
            AnimStartPrayingEvt();
        }

        void Awake()
        {
            playerControls = new PlayerControls();

            characterMovement = gameObject.GetComponent(typeof(CharacterMovement)) as CharacterMovement;
            characterSprite = gameObject.GetComponent(typeof(SpriteRenderer)) as SpriteRenderer;

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
                if (!praying && !roll)
                {
                    if (awaitingAttack)
                    {
                        StopAllCoroutines();
                        requestedAttack = false;
                        attacking = true;
                        attackN++;

                        characterAnimator.SetBool("Attacking", true);
                        characterAnimator.SetBool("AwaitingAttack", false);
                        characterAnimator.SetInteger("AttackN", attackN);

                        attackingMov = true;

                        awaitingAttack = false;
                    }
                    else
                    {
                        requestedAttack = true;
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

            // Input.GetAxisRaw("Horizontal") * generalSpeed;
            horizontalMove = playerControls.Gameplay.Horizontal.ReadValue<float>() * generalSpeed;
            verticalMove = playerControls.Gameplay.Vertical.ReadValue<float>() * generalSpeed;

            if (recentlyRespawned && horizontalMove > 0f)
            {
                AnimStoppedPrayingEvt();
                recentlyRespawned = false;
            }

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
            characterAnimator.SetBool("StartPraying", true);
        }

        public void AnimStartPrayingEndEvt()
        {
            characterAnimator.SetBool("Praying", true);
        }

        public void AnimStoppedPrayingEvt()
        {
            praying = false;
            characterAnimator.SetBool("StartPraying", false);
            characterAnimator.SetBool("Praying", false);
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
            awaitingAttack = true;
            characterAnimator.SetBool("AwaitingAttack", true);

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
            if((attackN - 1) >= 0 && (attackN - 1) <= swordSwingSFX.Length)
                swordSwingSFX[attackN - 1].Play();
        }

        private IEnumerator Respawn()
        {
            GameObject SpawnPoint = GameObject.Find("SpawnPoint");
            ResetAttackNow();
            yield return new WaitForSeconds(1f);
            characterMovement.PrepareRespawn(SpawnPoint.transform);
            yield return new WaitForSeconds(2f);

            AnimStartPrayingEvt();

            recentlyRespawned = true;
            GameManager.Instance.RespawnPlayer();
        }

        /// <summary>
        /// Resetea el ataque del jugador si no hace un combo completo !!!
        /// </summary>
        private IEnumerator ResetAttack()
        {
            /// Se le da un delay al jugador para que responda
            yield return new WaitForSeconds(WaitTimeForAttack);

            // Si ha transcurrido el tiempo y no se ha accionado ningun attack,
            // se resetea el ataque
        
                ResetAttackNow();
            
        }

        private void ResetAttackNow()
        {

            attacking = false;
            attackN = 0;
            characterAnimator.SetBool("Attacking", false);
            characterAnimator.SetInteger("AttackN", attackN);

        }
    }
}