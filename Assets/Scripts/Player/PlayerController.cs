using System.Collections;
using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        internal bool awaitingAttack = true;

        [SerializeField] private Animator characterAnimator;

        private PlayerCombatController combatCtrl;

        [SerializeField] private AudioSource footStep1;

        [SerializeField] private AudioSource footStep2;

        [SerializeField] private float generalSpeed = 40f;

        private float horizontalMove;

        [SerializeField] private float inputCooldown;

        private bool jump;
        private PlayerLadderClimbingController ladderClimbing;
        private float lastAngularVelocity;
        private Vector2 lastVelocity;

        [SerializeField] [Range(0.15f, 1f)] public float NextAttackMaxTime = 0.75f;

        protected PlayerMovementController playerMovementCtrl;
        internal bool praying = true;
        internal bool respawning;
        private Rigidbody2D rigidbody2D;
        internal bool roll;
        private bool savedRigidData;

        [SerializeField] private AudioSource[] swordSwingSFX;

        [SerializeField] private int UnlockedAttacksCount = 2;

        private float verticalMove;

        private void Start()
        {
            var SpawnPoint = GameObject.Find("SpawnPoint");
            rigidbody2D = GetComponent<Rigidbody2D>();
            transform.position = SpawnPoint.transform.position;
            respawning = true;
            AnimStartPrayingEvt();
        }

        private void Awake()
        {
            playerMovementCtrl = GetComponent<PlayerMovementController>();
            combatCtrl = GetComponent<PlayerCombatController>();
            ladderClimbing = GetComponent<PlayerLadderClimbingController>();

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
            if (GameManager.Instance.IsGamePaused())
            {
                if (!savedRigidData)
                {
                    lastVelocity = rigidbody2D.velocity;
                    lastAngularVelocity = rigidbody2D.angularVelocity;
                    rigidbody2D.bodyType = RigidbodyType2D.Static;
                    rigidbody2D.Sleep();

                    savedRigidData = true;
                }

                return;
            }

            if (savedRigidData)
            {
                rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
                rigidbody2D.velocity = lastVelocity;
                rigidbody2D.angularVelocity = lastAngularVelocity;
                rigidbody2D.WakeUp();
                savedRigidData = false;
            }

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
                if (horizontalMove > 0.01f || horizontalMove < 0.01f)
                {
                    praying = false;
                    characterAnimator.SetBool("Praying", false);
                    respawning = false;
                }

            horizontalMove = GameManager.Instance.GetPlayerControls().Gameplay.Horizontal.ReadValue<float>() *
                             generalSpeed;
            verticalMove = GameManager.Instance.GetPlayerControls().Gameplay.Vertical.ReadValue<float>() * generalSpeed;

            characterAnimator.SetFloat("Speed", Mathf.Abs(horizontalMove));
        }

        private void FixedUpdate()
        {
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