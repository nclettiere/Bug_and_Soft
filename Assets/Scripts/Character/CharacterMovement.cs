using UnityEngine;
using UnityEngine.Events;

namespace Character
{
    public class CharacterMovement : MonoBehaviour
    {
        [SerializeField] private float JumpForce = 400f;
        [Range(0, 1)] [SerializeField] private float CrouchSpeed = .36f;
        [Range(0, .3f)] [SerializeField] private float MovementSmoothing = .05f;
        [SerializeField] private LayerMask GroundLayers;
        [SerializeField] private LayerMask ClimableLayers;
        [SerializeField] private Transform GroundCheck;
        [SerializeField] private Transform CeilingCheck;
        [SerializeField] private Transform WallCheck;
        [SerializeField] private Transform LedgeCheck;
        [SerializeField] private Collider2D CrouchDisableCollider;
        [SerializeField] private Animator characterAnimator;
        [SerializeField] private bool ShouldPlayerFlip = true;
        [SerializeField] private float WallCheckDistance;

        const float GroundedRadius = .2f;
        private bool Grounded;
        const float CeilingRadius = .2f;
        private Rigidbody2D Rigidbody2D;
        private bool FacingRight = true;
        private Vector3 Velocity = Vector3.zero;

        private bool IsTouchingWall;
        private bool IsTouchingLedge;

        private bool CanClimbLedge = false;
        private bool LedgeDetected;

        private Vector2 LedgePosBott;
        private Vector2 LedgePos1;
        private Vector2 LedgePos2;

        public float LedgeClimbXOffset1;
        public float LedgeClimbYOffset1;
        public float LedgeClimbXOffset2;
        public float LedgeClimbYOffset2;

        [Header("Events")]
        [Space]

        public UnityEvent OnLandEvent;
        public UnityEvent OnAirEvent;

        [System.Serializable]
        public class BoolEvent : UnityEvent<bool> { }

        public BoolEvent OnCrouchEvent;
        private bool wasCrouching = false;

        private void FixedUpdate()
        {
            CheckSurroundings();
            CheckLedgeClimb();
        }

        /// <summary>
        /// Checkea si se esta en el ground, si se esta contra una pared, etc
        /// </summary>
        private void CheckSurroundings()
        {
            // GroundCheck
            bool wasGrounded = Grounded;
            Grounded = false;

            Collider2D[] colliders = Physics2D.OverlapCircleAll(GroundCheck.position, GroundedRadius, GroundLayers);
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].gameObject != gameObject)
                {
                    Grounded = true;
                    if (!wasGrounded)
                        OnLandEvent.Invoke();
                }
            }

            // WallCheck & LedgeCheck : Raycast
            RaycastHit2D LedgeHit;
            if (FacingRight)
            {
                IsTouchingWall = Physics2D.Raycast(WallCheck.position, transform.right, WallCheckDistance, GroundLayers);
                LedgeHit = Physics2D.Raycast(LedgeCheck.position, transform.right, WallCheckDistance, ClimableLayers);
            }
            else
            {
                IsTouchingWall = Physics2D.Raycast(WallCheck.position, transform.right, WallCheckDistance * -1, GroundLayers);
                LedgeHit = Physics2D.Raycast(LedgeCheck.position, transform.right, WallCheckDistance * -1, ClimableLayers);
            }

            IsTouchingLedge = LedgeHit;

            if (IsTouchingLedge && !LedgeDetected)
            {
                if (FacingRight)
                    LedgePos1 = LedgeHit.collider.transform.position - new Vector3(0.5f, 1f, 0f);
                else
                    LedgePos1 = LedgeHit.collider.transform.position + new Vector3(0.8f, -1f, 0f);
                LedgeDetected = true;
                LedgePosBott = LedgeCheck.position;
            }
        }

        private void CheckLedgeClimb()
        {
            if (LedgeDetected && !CanClimbLedge)
            {
                CanClimbLedge = true;

                ShouldPlayerFlip = false;
                characterAnimator.SetBool("CanClimbLedge", true);
                //Rigidbody2D.MovePosition(LedgePos1);
                transform.position = LedgePos1;
                Rigidbody2D.constraints = RigidbodyConstraints2D.FreezeAll;
            }
        }

        public void FinishLedgeClimb()
        {

        }

        public void Move(float moveH, float moveV, bool crouch, bool jump)
        {

            if (!GameManager.Instance.isInputEnabled)
                return;

            if (CanClimbLedge)
            {
                if (jump)
                {
                    Rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
                    ShouldPlayerFlip = true;
                    transform.position = LedgePos1 + new Vector2(0f, 1f);
                    Rigidbody2D.AddForce(new Vector2(250f, 1000f));
                    characterAnimator.SetBool("CanClimbLedge", false);
                    CanClimbLedge = false;
                    LedgeDetected = false;
                }
                if (moveV < 0f)
                {
                    Rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
                    ShouldPlayerFlip = true;
                    transform.position = LedgePos1 - new Vector2(0f, 1.2f);
                    characterAnimator.SetBool("CanClimbLedge", false);
                    CanClimbLedge = false;
                    LedgeDetected = false;
                }
                return;
            }


            if (!crouch)
            {
                if (Physics2D.OverlapCircle(CeilingCheck.position, CeilingRadius, GroundLayers))
                {
                    crouch = true;
                }
            }

            // If crouching
            if (crouch)
            {
                if (!wasCrouching)
                {
                    wasCrouching = true;
                    OnCrouchEvent.Invoke(true);
                }

                moveH *= CrouchSpeed;

                if (CrouchDisableCollider != null)
                    CrouchDisableCollider.enabled = false;
            }
            else
            {
                if (CrouchDisableCollider != null)
                    CrouchDisableCollider.enabled = true;

                if (wasCrouching)
                {
                    wasCrouching = false;
                    OnCrouchEvent.Invoke(false);
                }
            }

            Vector3 targetVelocity = new Vector2(moveH * 10f, Rigidbody2D.velocity.y);

            Rigidbody2D.velocity = Vector3.SmoothDamp(Rigidbody2D.velocity, targetVelocity, ref Velocity, MovementSmoothing);

            if (moveH > 0 && !FacingRight)
            {
                Flip();
            }
            else if (moveH < 0 && FacingRight)
            {
                Flip();
            }

            // If the player should jump...
            if (Grounded && jump)
            {
                // Add a vertical force to the player.
                Grounded = false;
                Rigidbody2D.AddForce(new Vector2(250f, JumpForce));
            }

            if (!Grounded)
                OnAirEvent.Invoke();
        }

        private void Flip()
        {
            if (!ShouldPlayerFlip) return;
            FacingRight = !FacingRight;

            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }

        private void Awake()
        {
            Rigidbody2D = GetComponent<Rigidbody2D>();

            if (OnLandEvent == null)
                OnLandEvent = new UnityEvent();

            if (OnAirEvent == null)
                OnAirEvent = new UnityEvent();

            if (OnCrouchEvent == null)
                OnCrouchEvent = new BoolEvent();
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawLine(WallCheck.position, new Vector3(WallCheck.position.x + WallCheckDistance, WallCheck.position.y, WallCheck.position.z));
            Gizmos.DrawLine(LedgeCheck.position, new Vector3(LedgeCheck.position.x + WallCheckDistance, LedgeCheck.position.y, LedgeCheck.position.z));
        }
    }
}