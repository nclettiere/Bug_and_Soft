using System;
using UnityEngine;
using UnityEngine.Events;

namespace Player
{
    public class PlayerMovementController : MonoBehaviour
    {
        const float GroundedRadius = .2f;
        const float CeilingRadius = .2f;

        private float beforeLedgeClimbOffsetY;

        private bool CanClimbLedge = false;
        [SerializeField] private Transform CeilingCheck;
        [SerializeField] private Animator characterAnimator;
        [SerializeField] private LayerMask ClimableLayers;
        [SerializeField] private Collider2D CrouchDisableCollider;
        [Range(0, 1)] [SerializeField] private float CrouchSpeed = .36f;
        public bool FacingRight { get; private set; } = true;
        [SerializeField] private Transform GroundCheck;
        internal bool Grounded;
        [SerializeField] private LayerMask GroundLayers;
        internal bool IsTouchingLedge;

        internal bool IsTouchingWall;

        [SerializeField] private float JumpForce = 400f;
        [SerializeField] private Transform LedgeCheck;

        public float LedgeClimbXOffset1;
        public float LedgeClimbXOffset2;
        public float LedgeClimbYOffset1;
        public float LedgeClimbYOffset2;
        private bool LedgeDetected;
        private Vector2 LedgePos1;
        private Vector2 LedgePos2;

        private Vector2 LedgePosBott;
        [Range(0, .3f)] [SerializeField] private float MovementSmoothing = .05f;
        public UnityEvent OnAirEvent;

        public BoolEvent OnCrouchEvent;

        [Header("Events")] [Space] public UnityEvent OnLandEvent;

        private PlayerController pCtrl;
        private Rigidbody2D Rigidbody2D;
        [SerializeField] private bool ShouldPlayerFlip = true;


        private float slowDownPenalty;
        internal Vector3 Velocity = Vector3.zero;
        [SerializeField] private Transform WallCheck;
        [SerializeField] private float WallCheckDistance;
        private bool wasCrouching = false;

        private void Start()
        {
            pCtrl = GetComponent<PlayerController>();
        }

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
                IsTouchingWall =
                    Physics2D.Raycast(WallCheck.position, transform.right, WallCheckDistance, GroundLayers);
                LedgeHit = Physics2D.Raycast(LedgeCheck.position, transform.right, WallCheckDistance, ClimableLayers);
            }
            else
            {
                IsTouchingWall = Physics2D.Raycast(WallCheck.position, transform.right, WallCheckDistance * -1,
                    GroundLayers);
                LedgeHit = Physics2D.Raycast(LedgeCheck.position, transform.right, WallCheckDistance * -1,
                    ClimableLayers);
            }

            IsTouchingLedge = LedgeHit;

            if (IsTouchingLedge && !LedgeDetected)
            {
                if (FacingRight)
                    LedgePos1 = LedgeHit.collider.transform.position - new Vector3(0.7f, 1.2f, 0f);
                else
                    LedgePos1 = LedgeHit.collider.transform.position + new Vector3(0.7f, -1.2f, 0f);
                LedgeDetected = true;
                LedgePosBott = LedgeCheck.position;
            }
        }

        internal void AttackMovement(int power)
        {
            if (FacingRight)
                Rigidbody2D.AddForce(new Vector2(150f * power, 100f));
            else
                Rigidbody2D.AddForce(new Vector2((150f * power) * -1, 100f));
        }

        private void CheckLedgeClimb()
        {
            if (LedgeDetected && !CanClimbLedge)
            {
                CanClimbLedge = true;

                ShouldPlayerFlip = false;
                characterAnimator.SetBool("CanClimbLedge", true);

                beforeLedgeClimbOffsetY = GameManager.Instance.GetCameraOffset().y;
                GameManager.Instance.SetCameraOffsetY(beforeLedgeClimbOffsetY + 4f);

                transform.position = LedgePos1;
                Rigidbody2D.constraints = RigidbodyConstraints2D.FreezeAll;
            }
        }

        public void Move(float moveH, float moveV, bool crouch, bool jump, bool roll, bool attacking)
        {
            if (!GameManager.Instance.GetIsInputEnabled())
            {
                Debug.Log("Input is not enabled");
                return;
            }

            if (CanClimbLedge)
            {
                if (jump)
                {
                    Rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
                    ShouldPlayerFlip = true;
                    transform.position = LedgePos1 + new Vector2(0f, 1f);
                    Rigidbody2D.AddForce(new Vector2(250f, 1000f));
                    characterAnimator.SetBool("CanClimbLedge", false);
                    GameManager.Instance.SetCameraOffsetY(2.5f);
                    CanClimbLedge = false;
                    LedgeDetected = false;
                }

                if (moveV < 0f)
                {
                    Rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
                    ShouldPlayerFlip = true;
                    transform.position = LedgePos1 - new Vector2(0f, 1.2f);
                    characterAnimator.SetBool("CanClimbLedge", false);
                    GameManager.Instance.SetCameraOffsetY(2.5f);
                    CanClimbLedge = false;
                    LedgeDetected = false;
                }

                return;
            }

            if (Grounded)
            {
                // Only roll if moving right/left
                if (roll)
                {
                    if (FacingRight)
                        Rigidbody2D.AddForce(new Vector2(1000f, 100f));
                    else
                        Rigidbody2D.AddForce(new Vector2(-1000f, 100f));
                }
            }

            if (!attacking)
            {
                Vector3 targetVelocity;
                if (Time.time >= slowDownPenalty)
                    targetVelocity = new Vector2(moveH * 10f, Rigidbody2D.velocity.y);
                else
                    targetVelocity = new Vector2(moveH * 3f, Rigidbody2D.velocity.y);

                Rigidbody2D.velocity =
                    Vector3.SmoothDamp(Rigidbody2D.velocity, targetVelocity, ref Velocity, MovementSmoothing);
            }

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

        internal void PrepareRespawn(Vector3 SpawnPoint)
        {
            if (!FacingRight)
                Flip();
            transform.position = SpawnPoint;
        }

        public bool IsFacingRight()
        {
            return FacingRight;
        }

        public void EnableFlip()
        {
            ShouldPlayerFlip = true;
        }

        public void DisableFlip()
        {
            ShouldPlayerFlip = false;
        }

        public void Flip()
        {
            if (!ShouldPlayerFlip || pCtrl.moveOnDamaged) return;
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
            Gizmos.DrawLine(WallCheck.position,
                new Vector3(WallCheck.position.x + WallCheckDistance, WallCheck.position.y, WallCheck.position.z));
            Gizmos.DrawLine(LedgeCheck.position,
                new Vector3(LedgeCheck.position.x + WallCheckDistance, LedgeCheck.position.y, LedgeCheck.position.z));
        }

        public void AddSpeedPenalty(float lifetime)
        {
            slowDownPenalty = Time.time + lifetime;
        }

        [System.Serializable]
        public class BoolEvent : UnityEvent<bool>
        {
        }

        public void StopAllMovement()
        {
            Velocity = Vector3.zero;
            Grounded = true;
        }
    }
}