using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character
{
    public class CharacterControllerBase : MonoBehaviour
    {
        protected CharacterMovement characterMovement;
        protected SpriteRenderer characterSprite;
        private float horizontalMove = 0f;
        private float verticalMove = 0f;
        private bool jump = false;

        [SerializeField] private float generalSpeed = 40f;
        [SerializeField] private Animator characterAnimator;

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

            if (characterSprite.sprite == null)
                characterSprite.sprite = Resources.Load<Sprite>("Checker");
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
            if(!GameManager.Instance.IsPlayerAlive &&
                GameManager.Instance.PlayerDeathCount > 0) {
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

            playerControls.Gameplay.Jump.performed += ctx =>
            {
                jump = true;
                characterAnimator.SetBool("Jump", true);
            };
            
        }

        void FixedUpdate()
        {
            characterMovement.Move(horizontalMove * Time.fixedDeltaTime, verticalMove * Time.fixedDeltaTime, false, jump);
            jump = false;
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
            characterAnimator.SetBool("StartPraying", false);
            characterAnimator.SetBool("Praying", false);
        }

        private IEnumerator Respawn()
        {
            yield return new WaitForSeconds(1);

            GameObject SpawnPoint = GameObject.Find("SpawnPoint");
            transform.position = SpawnPoint.transform.position;
            AnimStartPrayingEvt();

            yield return new WaitForSeconds(3);

            recentlyRespawned = true;
            GameManager.Instance.RespawnPlayer();
        }
    }
}