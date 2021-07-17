using UnityEngine;
using UnityEngine.Events;

namespace Character
{
	public class CharacterMovement : MonoBehaviour
	{
		[SerializeField] private float JumpForce = 400f;                        
		[Range(0, 1)] [SerializeField] private float CrouchSpeed = .36f;        
		[Range(0, .3f)] [SerializeField] private float MovementSmoothing = .05f;
		[SerializeField] private LayerMask WhatIsGround;                        
		[SerializeField] private Transform GroundCheck;                         
		[SerializeField] private Transform CeilingCheck;                        
		[SerializeField] private Transform WallCheck;                        
		[SerializeField] private Collider2D CrouchDisableCollider;
		[SerializeField] private bool ShouldPlayerFlip = true;
		[SerializeField] private float WallCheckDistance;
		
		const float GroundedRadius = .2f;
		private bool Grounded;
		const float CeilingRadius = .2f;
		private Rigidbody2D Rigidbody2D;
		private bool FacingRight = true;
		private Vector3 Velocity = Vector3.zero;
		
        private bool IsTouchingWall;

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
		}

		/// <summary>
		/// Checkea si se esta en el ground, si se esta contra una pared, etc
		/// </summary>
		private void CheckSurroundings()
		{
			// GroundCheck
			bool wasGrounded = Grounded;
			Grounded = false;

			Collider2D[] colliders = Physics2D.OverlapCircleAll(GroundCheck.position, GroundedRadius, WhatIsGround);
			for (int i = 0; i < colliders.Length; i++)
			{
				if (colliders[i].gameObject != gameObject)
				{
					Grounded = true;
					if (!wasGrounded)
						OnLandEvent.Invoke();
				}
			}

			// WallCheck : Raycast
			IsTouchingWall = Physics2D.Raycast(WallCheck.position, transform.right, WallCheckDistance, WhatIsGround);
		}

		public void Move(float moveH, bool crouch, bool jump)
		{
			if (!crouch)
			{
				if (Physics2D.OverlapCircle(CeilingCheck.position, CeilingRadius, WhatIsGround))
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

			if(!Grounded)
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

		private void OnDrawGizmos() {
			Gizmos.DrawLine(WallCheck.position, new Vector3(WallCheck.position.x + WallCheckDistance, WallCheck.position.y, WallCheck.position.z));
		}
	}
}