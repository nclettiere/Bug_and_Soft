using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Controllers.Damage;
using UnityEngine;
using Controllers.Movement;
using Controllers.StateMachine;
using Controllers.StateMachine.States.Data;
using Dialogues;
using Player;
using Interactions.Enums;
using Interactions.Interfaces;
using Misc;
using UnityEngine.Events;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

namespace Controllers
{
    /// <summary>
    ///     <para>Controller basico para todos los NPCs/Enemigos/Neutrals.</para>
    ///     <para>Esta clase debe ser heredada por clases especificas para cada NPC.</para>
    /// </summary>
    /// <example>
    ///     <para>Vease la clase DummyController</para>
    /// </example>
    /// <remarks>
    ///     \emoji :clock4: Ultima actualizacion: v0.0.9 - 22/7/2021 - Nicolas Cabrera
    /// </remarks>
    public class BaseController :
        /// @cond SKIP_THIS
        MonoBehaviour,
        /// @endcond
        IDamageable,
        IInteractive
    {
        public virtual void Damage(float amount, int attackN)
        {

        }
        
        
        public virtual void Damage(DamageInfo damageInfo)
        {
            if (dead ||
                controllerKind == EControllerKind.NPC ||
                controllerKind == EControllerKind.Neutral)
                return;

            if (Time.time > damagedTimeCD && currentState != ECharacterState.Dead)
            {
                Debug.Log("DamageReceived (" + damageInfo.DamageAmount + ")");
                StopAllCoroutines();
                
                
                // Obtenemos la posicion del ataque
                int direction = damageInfo.GetAttackDirection(transform.position.x);

                playerFacingDirection = playerController.IsFacingRight();

                Instantiate(hitParticles, transform.position, Quaternion.Euler(0.0f, 0.0f, Random.Range(0.0f, 360f)));
                
                if (firstAttack)
                    hitAttackSFX1.Play();
                else
                    hitAttackSFX2.Play();
                
                firstAttack = !firstAttack;

                if (!isInvencible)
                    currentHealth -= damageInfo.DamageAmount;

                if (damageInfo.MoveOnAttack)
                    MoveOnDamaged(direction, damageInfo.MoveOnAttackForce);

                StartCoroutine(DamageEffect());

                if (currentHealth <= 0f)
                    Die();

                damagedTimeCD = Time.time + .15f;
            }
        }

        public void Damage(BaseController controller, DamageInfo damageInfo)
        {
            // Not for Enemies ...
        }

        public void Kill()
        {
            Debug.Log("This enemy is dead xd");
        }

        public bool Interact(BaseController controller, EInteractionKind interactionKind)
        {
            Debug.Log("Interaction of kind '" + interactionKind + "' received.");
            return true;
        }

        public virtual bool Interact(PlayerController controller, EInteractionKind interactionKind)
        {
            return true;
        }

        public void ReadyToInteract(PlayerController controller, bool isReady)
        {
            if (canInteract && isReady && dialogueBubble != null)
                dialogueBubble.gameObject.SetActive(true);
            else
                dialogueBubble.gameObject.SetActive(false);
        }

        public void ReadyToInteract(BaseController controller, bool isReady)
        {
        }

        private void Awake()
        {
            if (OnLandEvent == null)
                OnLandEvent = new UnityEvent();
        }


        protected virtual void Start()
        {
            baseMovementController = GetComponent<BaseMovementController>();
            characterAnimator = GetComponent<Animator>();
            rBody = GetComponent<Rigidbody2D>();
            renderer = GetComponent<SpriteRenderer>();
            playerController = GameObject.Find("Player").GetComponent<PlayerController>();
            currentHealth = ctrlData.maxHealth;

            StateMachine = new ControllerStateMachine();
        }

        protected virtual void Update()
        {
            if (GameManager.Instance.IsGamePaused())
            {
                if (!savedRigidData)
                {
                    lastVelocity = rBody.velocity;
                    lastAngularVelocity = rBody.angularVelocity;
                    rBody.bodyType = RigidbodyType2D.Static;
                    rBody.Sleep();

                    savedRigidData = true;
                }

                return;
            }

            if (savedRigidData)
            {
                rBody.bodyType = RigidbodyType2D.Dynamic;
                rBody.velocity = lastVelocity;
                rBody.angularVelocity = lastAngularVelocity;
                rBody.WakeUp();
                savedRigidData = false;
            }

            if (characterKind != ECharacterKind.Dummy && characterKind != ECharacterKind.Njord)
            {
                if (!dead)
                {
                    CheckPlayerInNearRange();
                    CheckPlayerInLongRange();
                }

                StateMachine.CurrentState.UpdateState();
            }
        }

        private void FixedUpdate()
        {
            if (GameManager.Instance.IsGamePaused() || dead) return;

            if (characterKind != ECharacterKind.Dummy && characterKind != ECharacterKind.Njord)
                StateMachine.CurrentState.UpdatePhysics();
        }

        protected virtual void OnDrawGizmos()
        {
            // World Checks gizmoes ...
            Gizmos.color = Color.yellow;
            if (characterKind != ECharacterKind.Dummy && characterKind != ECharacterKind.Njord)
            {
                // General World Checks
                Gizmos.DrawLine(groundCheck.position,
                    new Vector2(groundCheck.position.x, groundCheck.position.y - ctrlData.groundCheckDistance));
                Gizmos.DrawLine(wallCheck.position,
                    new Vector2(wallCheck.position.x + ctrlData.wallCheckDistance, wallCheck.position.y));
                Gizmos.DrawLine(topCheck.position,
                    new Vector2(topCheck.position.x, topCheck.position.y + ctrlData.topCheckDistance));
                Gizmos.DrawLine(ledgeCheck.position,
                    new Vector2(ledgeCheck.position.x, ledgeCheck.position.y - ctrlData.ledgeCheckDistacne));

                // Touch Damage
                Vector2 botLeft = new Vector2(
                    touchDamageCheck.position.x - (touchDamageWidth / 2),
                    touchDamageCheck.position.y - (touchDamageHeight / 2));
                Vector2 botRight = new Vector2(
                    touchDamageCheck.position.x + (touchDamageWidth / 2),
                    touchDamageCheck.position.y - (touchDamageHeight / 2));
                Vector2 topRight = new Vector2(
                    touchDamageCheck.position.x + (touchDamageWidth / 2),
                    touchDamageCheck.position.y + (touchDamageHeight / 2));
                Vector2 topLeft = new Vector2(
                    touchDamageCheck.position.x - (touchDamageWidth / 2),
                    touchDamageCheck.position.y + (touchDamageHeight / 2));


                Gizmos.DrawLine(botLeft, botRight);
                Gizmos.DrawLine(botRight, topRight);
                Gizmos.DrawLine(topRight, topLeft);
                Gizmos.DrawLine(topLeft, botLeft);


                // Player Detection
                //Near range
                Gizmos.color = Color.blue;
                Gizmos.DrawLine(transform.position,
                    new Vector2((transform.position.x + (ctrlData.playerNearRangeDistance * FacingDirection)),
                        transform.position.y));
                //Long range
                Gizmos.color = Color.cyan;
                Gizmos.DrawLine(transform.position + new Vector3(ctrlData.playerNearRangeDistance, 0f),
                    new Vector2((transform.position.x + (ctrlData.playerLongRangeDistance * FacingDirection)),
                        transform.position.y));
            }

            if (controllerKind == EControllerKind.NPC)
            {
                // Interaction gizmo ...
                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(transform.position, interactionRadius);
            }
        }

        public void Flip()
        {
            FacingDirection *= -1;
            transform.Rotate(0.0f, 180.0f, 0.0f);
        }

        public void SetVelocity(float velocity)
        {
            velocityStorage.Set(velocity * FacingDirection, rBody.velocity.y);
            rBody.velocity = velocityStorage;
        }

        public void AddForce(Vector2 newForce, bool isImpulse)
        {
            forceStorage.Set(newForce.x * FacingDirection, newForce.y);
            rBody.AddForce(forceStorage, (isImpulse ? ForceMode2D.Impulse : ForceMode2D.Force));
        }

        public void AddTorque(float newTorque, bool isImpulse)
        {
            rBody.AddTorque(newTorque * FacingDirection, (isImpulse ? ForceMode2D.Impulse : ForceMode2D.Force));
        }

        /// <summary>
        ///     Obtiene el controller de moviemento del personaje especificando que tipo de moviemnto usa en T
        /// </summary>
        /// <typeparam name="T">Clase derivada de BaseMovementController</typeparam>
        /// <returns>Un objeto de clase T que hereda de BaseMovementController almacenado en el BaseController.</returns>
        public T GetMovementController<T>() where T : BaseMovementController
        {
            return (T) baseMovementController;
        }

        /// <summary>
        ///     Obtiene el animator del character
        /// </summary>
        /// <returns>El animator del character</returns>
        public Animator GetAnimator()
        {
            return characterAnimator;
        }

        public Transform GetTransfrom()
        {
            return transform;
        }

        public GameObject GetDialogueBubble()
        {
            return dialogueBubble;
        }

        public bool CheckWall()
        {
            return Physics2D.Raycast(wallCheck.position, transform.right, ctrlData.wallCheckDistance,
                ctrlData.whatIsGround);
        }

        public bool CheckLedge()
        {
            return Physics2D.Raycast(ledgeCheck.position, Vector2.down, ctrlData.ledgeCheckDistacne,
                ctrlData.whatIsGround);
        }

        public bool CheckGround()
        {
            return Physics2D.Raycast(groundCheck.position, Vector2.down, ctrlData.groundCheckDistance,
                ctrlData.whatIsGround);
        }

        public bool CheckTop()
        {
            return Physics2D.Raycast(topCheck.position, Vector2.up, ctrlData.topCheckDistance,
                ctrlData.whatIsGround);
        }


        public bool CheckPlayerInNearRange()
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, ctrlData.playerNearRangeDistance,
                ctrlData.whatIsPlayer);

            if (hit.collider != null)
            {
                if (hit.collider.CompareTag("Player"))
                {
                    return true;
                }
            }

            return false;
        }
        
        public bool CheckPlayerInLongRange()
        {
            Vector2 longRangePos = transform.position + new Vector3(ctrlData.playerNearRangeDistance, 0f);
            RaycastHit2D hit = Physics2D.Raycast(longRangePos, transform.right, ctrlData.playerLongRangeDistance,
                ctrlData.whatIsPlayer);
        
            if (hit.collider != null)
            {
                if (hit.collider.CompareTag("Player"))
                {
                    return true;
                }
            }
        
            return false;
        }

        protected void MoveOnDamaged(int direction, Vector2 moveOnDamagedForce)
        {
            moveOnDamaged = true;
            moveOnDamagedStartTime = Time.time;
            rBody.AddForce(new Vector2(moveOnDamagedForce.x * direction, moveOnDamagedForce.y));
            //rigidbody2D.velocity = new Vector2(moveOnDamagedForce.x * direction, moveOnDamagedForce.y);
        }

        /// <summary>
        ///     Metodo para checkear si el jugador puede interactual con este personaje 
        /// </summary>
        private void CheckInteractions()
        {
            RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, interactionRadius, new Vector2(0f, 0f));

            foreach (var hit in hits)
            {
                if (hit.collider.tag == "Player")
                {
                    canPlayerInteract = true;
                    // Show interaction bubbles
                    if (dialogueBubble != null)
                        dialogueBubble.gameObject.SetActive(true);
                    return;
                }
            }

            canPlayerInteract = false;
            // Hide interaction bubbles
            if (dialogueBubble != null)
                dialogueBubble.gameObject.SetActive(false);
        }

        /// <summary>
        ///     Checkea si es necesario mover al personaje mientras es atacado.
        /// </summary>
        private void CheckMoveOnAttack()
        {
            if (Time.time >= moveOnAttackStart + moveOnAttackDuration && movedOnAttack)
            {
                movedOnAttack = false;
                rBody.velocity = new Vector2(0.0f, rBody.velocity.y);
            }
        }

        public void CheckTouchDamage()
        {
            if (Time.time >= lastTouchDamageTime + touchDamageCooldown)
            {
                touchDamageBottomLeft.Set(
                    touchDamageCheck.position.x - (touchDamageWidth / 2),
                    touchDamageCheck.position.y - (touchDamageHeight / 2));

                touchDamageTopRight.Set(
                    touchDamageCheck.position.x + (touchDamageWidth / 2),
                    touchDamageCheck.position.y + (touchDamageHeight / 2));

                Collider2D hit = Physics2D.OverlapArea(
                    touchDamageBottomLeft,
                    touchDamageTopRight,
                    ctrlData.whatIsPlayer);

                if (hit != null)
                {
                    lastTouchDamageTime = Time.time;
                    DamageInfo dInfo = new DamageInfo(ctrlData.generalDamage, transform.position.x);

                    PlayerController bctrl = hit.transform.GetComponent<PlayerController>();
                    if (bctrl != null && (bctrl is IDamageable))
                    {
                        bctrl.Damage(this, dInfo);
                    }
                }
            }
        }

        private protected IEnumerator DamageEffect()
        {
            renderer.color = new Color(1f, 0.334f, 0.305f);
            yield return new WaitForSeconds(0.5f);
            renderer.color = Color.white;
        }

        public virtual void Die()
        {
            dead = true;
        }

        public void DestroyNow()
        {
            StopAllCoroutines();
            Destroy(gameObject);
        }

        public void DestroyNow(GameObject lapida)
        {
            StopAllCoroutines();
            renderer.enabled = false;
            Instantiate(lapida, transform.position, Quaternion.Euler(0f, 0f, 0f));
            Destroy(gameObject);
        }

        public bool CanCharacterInteract()
        {
            return canInteract;
        }

        #region UserVariables

        public int FacingDirection { get; private set; } =
            1; // Solo se puede modificar en el codigo, por eso el private set; xd

        public bool IsReadyToAttack { get; set; }
        public ControllerStateMachine StateMachine = new ControllerStateMachine();
        public ControllerData ctrlData;
        public float deadMaxWait = 5f;
        public int currentHealth;

        public int KrownReward = 5;

        public EControllerKind controllerKind = EControllerKind.NPC;
        [SerializeField] private ECharacterKind characterKind;
        [SerializeField] private float interactionRadius = 4f;
        [SerializeField] private float moveOnAttackDuration, touchDamageCooldown, generalSpeed = 40f;
        [SerializeField] private int touchDamage = 12;

        [SerializeField]
        private protected bool isInvencible, canBeStunned, canDamageOnTouch, canBeMovedOnAttack, movedOnAttack;

        [SerializeField] private Vector2 moveOnAttackSpeed;
        [SerializeField] protected GameObject hitParticles;
        [SerializeField] private protected AudioSource hitAttackSFX1;
        [SerializeField] private protected AudioSource hitAttackSFX2;
        [SerializeField] private AudioSource deathSFX;
        [SerializeField] private protected GameObject dialogueBubble;

        [Header("State Options")] [SerializeField]
        private bool canWalk;

        [SerializeField] private bool canJump;
        [SerializeField] private bool canBeDamaged;

        [SerializeField] private float
            touchDamageWidth,
            touchDamageHeight;

        [SerializeField] private Transform
            groundCheck,
            wallCheck,
            topCheck,
            ledgeCheck,
            touchDamageCheck;


        [Header("Events Zone")] public UnityEvent OnLandEvent;

        #endregion

        #region Variables

        protected bool dead;
        protected BaseMovementController baseMovementController;
        protected Animator characterAnimator;
        protected internal Rigidbody2D rBody;
        protected PlayerController playerController;
        private SpriteRenderer renderer;
        protected ECharacterState currentState;
        protected bool playerFacingDirection;
        protected bool canInteract = true;
        private bool moveOnDamaged;
        private float moveOnDamagedStartTime = float.NegativeInfinity;

        public bool wallDetected { get; private set; }
        public bool groundDetected { get; private set; }
        public bool topDetected { get; private set; }

        protected bool
            lastGroundDetected,
            lapidaIntance = false,
            firstAttack,
            canPlayerInteract = false,
            savedRigidData;

        private Vector2
            movement,
            lastVelocity,
            touchDamageBottomLeft,
            touchDamageTopRight,
            velocityStorage,
            forceStorage;

        protected float
            moveOnAttackStart,
            damagedTimeCD = float.NegativeInfinity,
            lastAngularVelocity,
            lastTouchDamageTime;

        #endregion

        public bool IsDead()
        {
            return dead;
        }
    }
}