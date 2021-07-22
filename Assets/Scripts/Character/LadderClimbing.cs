using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character
{
    public class LadderClimbing : MonoBehaviour
    {
        private float vertical;
        private float speed = 8f;
        private bool isLadder;
        internal bool isClimbing;

        private float normalGravity;

        [SerializeField] private Rigidbody2D rb;
        [SerializeField] private Animator characterAnimator;

        private PlayerControls playerControls;
        private CharacterControllerBase characterController;

        private void Awake()
        {
            playerControls = new PlayerControls();
            characterController = GetComponent<CharacterControllerBase>();
        }

        private void Start()
        {
            normalGravity = rb.gravityScale;
        }

        // Update is called once per frame
        void Update()
        {
            vertical = playerControls.Gameplay.Vertical.ReadValue<float>();

            if (isLadder && !characterController.attacking && Mathf.Abs(vertical) > 0f)
            {
                isClimbing = true;
            }
        }
        private void OnEnable()
        {
            playerControls.Enable();
        }

        private void OnDisable()
        {
            playerControls.Disable();
        }

        private void FixedUpdate()
        {
            if (isClimbing)
            {
                characterAnimator.SetBool("Climbing", true);
                rb.gravityScale = 0f;
                rb.velocity = new Vector2(rb.velocity.x, vertical * speed);
            }else
            {
                rb.gravityScale = normalGravity;
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.CompareTag("Ladder"))
            {
                isLadder = true;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Ladder"))
            {
                isLadder = false;
                isClimbing = false;
                characterAnimator.SetBool("Climbing", false);
            }
        }
    }
}