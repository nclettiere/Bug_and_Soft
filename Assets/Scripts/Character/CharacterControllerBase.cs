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

        private int deathCount = 0;

        private void Start()
        {
            GameObject SpawnPoint = GameObject.Find("SpawnPoint");
            transform.position = SpawnPoint.transform.position;
            recentlyRespawned = true;
            AnimStartPrayingEvt();
        }

        void Awake()
        {

            characterMovement = gameObject.GetComponent(typeof(CharacterMovement)) as CharacterMovement;
            characterSprite = gameObject.GetComponent(typeof(SpriteRenderer)) as SpriteRenderer;

            if (characterSprite.sprite == null)
                characterSprite.sprite = Resources.Load<Sprite>("Checker");
        }

        void Update()
        {
            if(!GameManager.Instance.IsPlayerAlive &&
                GameManager.Instance.PlayerDeathCount > 0) {
                Respawn();
                horizontalMove = 0f;
                AnimStartPrayingEvt();
                return;
            }
            if (!GameManager.Instance.isInputEnabled)
                return;

            horizontalMove = Input.GetAxisRaw("Horizontal") * generalSpeed;

            if(recentlyRespawned && horizontalMove > 0f)
            {
                AnimStoppedPrayingEvt();
                recentlyRespawned = false;
            }

            characterAnimator.SetFloat("Speed", Mathf.Abs(horizontalMove));

            if (Input.GetButtonDown("Jump"))
            {
                jump = true;
                characterAnimator.SetBool("Jump", true);
            }

            
        }

        void FixedUpdate()
        {
            characterMovement.Move(horizontalMove * Time.fixedDeltaTime, false, jump);
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

        public void Respawn()
        {
            GameObject SpawnPoint = GameObject.Find("SpawnPoint");
            transform.position = SpawnPoint.transform.position;
            recentlyRespawned = true;
            GameManager.Instance.RespawnPlayer();
        }
    }
}