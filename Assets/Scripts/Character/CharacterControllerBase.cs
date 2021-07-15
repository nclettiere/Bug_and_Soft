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
        [SerializeField] private float generalSpeed = 40f;
        private bool jump = false;
        [SerializeField] private Animator characterAnimator;


        void Awake()
        {

            characterMovement = gameObject.GetComponent(typeof(CharacterMovement)) as CharacterMovement;
            characterSprite = gameObject.GetComponent(typeof(SpriteRenderer)) as SpriteRenderer;

            if(characterSprite.sprite == null)
                characterSprite.sprite = Resources.Load<Sprite>("Checker");
        }

        void Update()
        {
            if (!GameManager.Instance.isInputEnabled)
                return;

            horizontalMove = Input.GetAxisRaw("Horizontal") * generalSpeed;

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
            characterAnimator.SetBool("Jump", false);
        }
    }
}
