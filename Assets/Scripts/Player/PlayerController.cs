using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;

namespace Player
{
    public class PlayerController : MonoBehaviour
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

        protected PlayerMovementController playerMovementCtrl;
        private PlayerLadderClimbingController ladderClimbing;
        private PlayerCombatController combatCtrl;
        private float horizontalMove = 0f; 
        private float verticalMove = 0f;
        private bool jump = false;
        internal bool roll = false;
        internal bool praying = true;
        internal bool awaitingAttack = true;
        internal bool respawning = false;

        private void Start()
        {
            GameObject SpawnPoint = GameObject.Find("SpawnPoint");
            transform.position = SpawnPoint.transform.position;
            respawning = true;
            AnimStartPrayingEvt();
        }

        void Awake()
        {
            playerMovementCtrl = GetComponent<PlayerMovementController>();
            combatCtrl = GetComponent<PlayerCombatController>();
            ladderClimbing = GetComponent<PlayerLadderClimbingController>();

            GameManager.Instance.playerControls.Gameplay.Roll.performed += ctxRoll =>
            {
                if (horizontalMove != 0f)
                {
                    roll = true;
                    characterAnimator.SetBool("Roll", true);
                }
            };
            GameManager.Instance.playerControls.Gameplay.Jump.performed += ctxRoll =>
            {
                jump = true;
            };
        }

        internal void OnEnable()
        {
            GameManager.Instance.playerControls.Enable();
        }

        internal void OnDisable()
        {
            GameManager.Instance.playerControls.Disable();
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

            horizontalMove = GameManager.Instance.playerControls.Gameplay.Horizontal.ReadValue<float>() * generalSpeed;
            verticalMove = GameManager.Instance.playerControls.Gameplay.Vertical.ReadValue<float>() * generalSpeed;

            characterAnimator.SetFloat("Speed", Mathf.Abs(horizontalMove));
        }

        void FixedUpdate()
        {
            playerMovementCtrl.Move(
                horizontalMove * Time.fixedDeltaTime,
                verticalMove * Time.fixedDeltaTime,
                false,
                jump,
                roll,
                combatCtrl.attacking);

            jump = false;
            roll = false;
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

        private IEnumerator Respawn()
        {
            this.OnDisable();

            AnimStartPrayingEvt();

            respawning = true;
            GameObject SpawnPoint = GameObject.Find("SpawnPoint");
            combatCtrl.ResetAttackNow();
            yield return new WaitForSeconds(1f);
            playerMovementCtrl.PrepareRespawn(SpawnPoint.transform);
            yield return new WaitForSeconds(2f);

            GameManager.Instance.RespawnPlayer();
            respawning = false;

            this.OnEnable();
        }
    }
}