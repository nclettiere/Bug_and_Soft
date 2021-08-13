using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class PlayerLadderClimbingController : MonoBehaviour
    {
        private float vertical;
        private float speed = 8f;
        private bool isLadder;
        internal bool isClimbing;

        private float normalGravity;

        private Rigidbody2D rb;
        
        private Animator playerAnimator;

        private PlayerControls playerControls;

        private PlayerController playerController;
        private PlayerCombatController playerCombatController;

        private void Awake()
        {
            playerControls = new PlayerControls();
            rb = GetComponent<Rigidbody2D>();
            playerController = GetComponent<PlayerController>();
            playerCombatController = GetComponent<PlayerCombatController>();
            playerAnimator = GetComponent<Animator>();
        }

        private void Start()
        {
            normalGravity = rb.gravityScale;
        }

        // Update is called once per frame
        void Update()
        {
            vertical = playerControls.Gameplay.Vertical.ReadValue<float>();

            if (isLadder && !playerCombatController.attacking && Mathf.Abs(vertical) > 0f)
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
                playerAnimator.SetBool("Climbing", true);
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
                playerAnimator.SetBool("Climbing", false);
            }
        }
    }
}