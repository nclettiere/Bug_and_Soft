using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Controllers.Damage;
using UnityEngine;
using Controllers.Movement;
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
        #region UserVariables

        [SerializeField] private protected EControllerKind controllerKind = EControllerKind.NPC;
        [SerializeField] private ECharacterKind characterKind;
        [SerializeField] private float interactionRadius = 4f;
        [SerializeField] private float maxHealth, moveOnAttackDuration, generalSpeed = 40f;
        [SerializeField] private bool isInvencible, canBeStunned, canDamageOnTouch, canBeMovedOnAttack, movedOnAttack;
        [SerializeField] private Vector2 moveOnAttackSpeed;
        [SerializeField] private GameObject hitParticles;
        [SerializeField] private AudioSource hitAttackSFX1;
        [SerializeField] private AudioSource hitAttackSFX2;
        [SerializeField] private AudioSource deathSFX;
        [SerializeField] private GameObject dialogueBubble;
        [SerializeField] private GameObject lapida;


        [Header("State Options")] [SerializeField]
        private bool canWalk;

        [SerializeField] private bool canJump;
        [SerializeField] private bool canBeDamaged;

        [Header("World Checks")] [SerializeField]
        private float wallCheckDistance,
            groundCheckDistance,
            topCheckDistance;

        [SerializeField] private Transform
            groundCheck,
            wallCheck,
            topCheck,
            touchDamageCheck;

        [SerializeField] private LayerMask whatIsGround, whatIsPlayer;


        [Header("Events Zone")] public UnityEvent OnLandEvent;

        #endregion

        public int facingDirection { get; private set; } =
            1; // Solo se puede modificar en el codigo, por eso el private set; xd

        #region Variables

        protected BaseMovementController baseMovementController;
        protected Animator characterAnimator;
        protected Rigidbody2D rigidbody2D;
        protected PlayerController playerController;
        private SpriteRenderer renderer;
        private ECharacterState currentState;
        protected bool playerFacingDirection;
        protected private bool canCharacterDie = false;

        private bool
            groundDetected,
            wallDetected,
            topDetected,
            lastGroundDetected,
            lapidaIntance = false,
            firstAttack,
            canPlayerInteract = false,
            savedRigidData;

        private Vector2
            movement,
            lastVelocity,
            touchDamageBottomLeft,
            touchDamageTopRight;

        private float
            currentHealth,
            moveOnAttackStart,
            damagedTimeCD = float.NegativeInfinity,
            lastAngularVelocity,
            lastTouchDamageTime,
            touchDamageCooldown,
            touchDamageWidth,
            touchDamageHeight;

        private int touchDamage = 12;

        #endregion

        /// <summary>
        ///     <para>Este metodo se ejecuta en el Start() del MonoBehaviour luego inicializar las variables necesarias.</para>
        ///     <para>Al ser un metodo virtual, se encuentra vacio para que los super class lo implementen.</para>
        /// </summary>
        protected virtual void OnStart()
        {
        }

        /// <summary>
        ///     <para>Este metodo se ejecuta en el Update() del MonoBehaviour luego inicializar las variables necesarias.</para>
        ///     <para>Al ser un metodo virtual, se encuentra vacio para que los super class lo implementen.</para>
        /// </summary>
        protected virtual void OnUpdate()
        {
            // Debe ser llenado en una clase heredada
            // Ej: DummyController.cs
        }

        /// <summary>
        ///     <para>Este metodo se ejecuta en el FixedUpdate() del MonoBehaviour luego inicializar las variables necesarias.</para>
        ///     <para>Al ser un metodo virtual, se encuentra vacio para que los super class lo implementen.</para>
        /// </summary>
        protected virtual void OnFixedUpdate()
        {
            // Debe ser llenado en una clase heredada
            // Ej: DummyController.cs
        }

        /// <summary>
        ///     <para>Este metodo se ejecuta en el OnDrawGizmos() del MonoBehaviour luego inicializar las variables necesarias.</para>
        ///     <para>Al ser un metodo virtual, se encuentra vacio para que los super class lo implementen.</para>
        /// </summary>
        protected virtual void DrawGizmos()
        {
            // Debe ser llenado en una clase heredada
            // Ej: DummyController.cs
        }

        /// <summary>
        ///     <para>Este metodo se ejecuta en el Awake() del MonoBehaviour luego inicializar las variables necesarias.</para>
        ///     <para>Al ser un metodo virtual, se encuentra vacio para que los super class lo implementen.</para>
        /// </summary>
        protected virtual void OnAwake()
        {
            // Debe ser llenado en una clase heredada
            // Ej: DummyController.cs
        }

        private void Awake()
        {
            if (OnLandEvent == null)
                OnLandEvent = new UnityEvent();

            OnAwake();
        }


        private void Start()
        {
            baseMovementController = GetComponent<BaseMovementController>();
            characterAnimator = GetComponent<Animator>();
            rigidbody2D = GetComponent<Rigidbody2D>();
            renderer = GetComponent<SpriteRenderer>();
            playerController = GameObject.Find("Player").GetComponent<PlayerController>();
            currentHealth = maxHealth;

            OnStart();
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

            if (characterKind != ECharacterKind.Dummy && characterKind != ECharacterKind.Njord)
            {
                // Llamamos al evento OnLandEvent si es necesario!
                bool wasGrounded = groundDetected;
                groundDetected =
                    Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
                if (!wasGrounded && groundDetected)
                    OnLandEvent.Invoke();

                wallDetected = Physics2D.Raycast(wallCheck.position, transform.right, wallCheckDistance, whatIsGround);
                topDetected = Physics2D.Raycast(topCheck.position, Vector2.up, topCheckDistance, whatIsGround);

                characterAnimator.SetBool("OnLand", groundDetected);

                // State Machine
                switch (currentState)
                {
                    case ECharacterState.Idle:
                        UpdateIdleState();
                        break;
                    case ECharacterState.Walking:
                        UpdateWalkingState();
                        break;
                    case ECharacterState.Jumping:
                        UpdateJumpingState();
                        break;
                    case ECharacterState.Hit:
                        UpdateHitState();
                        break;
                    case ECharacterState.Dead:
                        UpdateDeadState();
                        break;
                }
            }

            //CheckInteractions();
            CheckTouchDamage();
            CheckMoveOnAttack();
            OnUpdate();
        }

        private void FixedUpdate()
        {
            if (GameManager.Instance.IsGamePaused()) return;

            OnFixedUpdate();
        }

        private void OnDrawGizmos()
        {
            // World Checks gizmoes ...
            Gizmos.color = Color.yellow;
            if (characterKind != ECharacterKind.Dummy && characterKind != ECharacterKind.Njord)
            {
                Gizmos.DrawLine(groundCheck.position,
                    new Vector2(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
                Gizmos.DrawLine(wallCheck.position,
                    new Vector2(wallCheck.position.x + wallCheckDistance, wallCheck.position.y));
                Gizmos.DrawLine(topCheck.position,
                    new Vector2(topCheck.position.x, topCheck.position.y + topCheckDistance));
            }

            // Interaction gizmo ...
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, interactionRadius);
            // other ...
            DrawGizmos();
        }

        #region IAStates

        // IDLE STATE -------------------------------------------------------
        private void EnterIdleState()
        {
        }

        private void UpdateIdleState()
        {
        }

        private void ExitIdleState()
        {
        }

        // WALKING STATE -------------------------------------------------------
        private void EnterWalkingState()
        {
        }

        private void UpdateWalkingState()
        {
            if (!groundDetected || wallDetected)
            {
                Flip();
            }
            else
            {
                // Moves character
                movement.Set(generalSpeed * facingDirection, rigidbody2D.velocity.y);
                rigidbody2D.velocity = movement;
            }
        }

        private void ExitWalkingState()
        {
        }

        // JUMPING STATE -------------------------------------------------------
        private void EnterJumpingState()
        {
        }

        private void UpdateJumpingState()
        {
            if (!OnUpdateJumpingStateStart()) return;

            if (wallDetected)
            {
                Flip();
            }
            else
            {
                // El personaje salta en la direccion que mira.
                //rigidbody2D.velocity = new Vector2(10f, 0f);
                rigidbody2D.AddForce(new Vector2(3f * facingDirection, 5f), ForceMode2D.Impulse);
            }

            OnUpdateJumpingStateEnd();
        }


        protected virtual bool OnUpdateJumpingStateStart()
        {
            return true;
        }

        protected virtual void OnUpdateJumpingStateEnd()
        {
        }

        private void ExitJumpingState()
        {
        }

        // HIT STATE -------------------------------------------------------
        private void EnterHitState()
        {
        }

        private void UpdateHitState()
        {
        }

        private void ExitHitState()
        {
        }

        // DEAD STATE -------------------------------------------------------
        private void EnterDeadState()
        {
            OnEnterDeadStateStart();
            deathSFX.Play();
            characterAnimator.SetBool("Dead", true);
        }


        protected virtual void OnEnterDeadStateStart()
        {
        }

        private void UpdateDeadState()
        {
            if (!canCharacterDie) return;

            if (groundDetected || topDetected)
            {
                if (lapida != null && !lapidaIntance)
                {
                    Instantiate(lapida, transform.position, Quaternion.Euler(0f, 0f, 0f));
                    renderer.enabled = false;
                    lapidaIntance = true;
                }
            }
        }

        private void ExitDeadState()
        {
        }

        /// <summary>
        /// Cambia el estado actual del character. Primero llama al EXIT function del estado actual, luego llama al ENTER del siguiente !!!
        /// </summary>
        /// <param name="nextState">El estado al que se quiere cambiar</param>
        public void SwitchStates(ECharacterState nextState)
        {
            switch (currentState)
            {
                case ECharacterState.Idle:
                    ExitIdleState();
                    break;
                case ECharacterState.Walking:
                    ExitWalkingState();
                    break;
                case ECharacterState.Jumping:
                    ExitJumpingState();
                    break;
                case ECharacterState.Hit:
                    ExitHitState();
                    break;
                case ECharacterState.Dead:
                    ExitDeadState();
                    break;
            }

            switch (nextState)
            {
                case ECharacterState.Idle:
                    EnterIdleState();
                    break;
                case ECharacterState.Walking:
                    EnterWalkingState();
                    break;
                case ECharacterState.Jumping:
                    EnterJumpingState();
                    break;
                case ECharacterState.Hit:
                    EnterHitState();
                    break;
                case ECharacterState.Dead:
                    EnterDeadState();
                    break;
            }

            currentState = nextState;
        }

        #endregion

        private void Flip()
        {
            facingDirection *= -1;
            transform.Rotate(0.0f, 180.0f, 0.0f);
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

        /// <summary>
        ///     Obtiene el animator del character
        /// </summary>
        /// <returns>El animator del character</returns>
        public GameObject GetDialogueBubble()
        {
            return dialogueBubble;
        }

        /// <summary>
        ///     Calcula y adiciona el movimiento necesario para dat un efecto de movimento cuando recibe danio
        /// </summary>
        /// <param name="multiplier">
        ///     Multiplicador del movimiento. Se recomienda utilizar el attackN del player.
        /// </param>
        private void MoveOnAttack(int multiplier)
        {
            movedOnAttack = true;
            moveOnAttackStart = Time.time;

            if (playerFacingDirection)
                rigidbody2D.velocity = new Vector2((moveOnAttackSpeed.x * multiplier), moveOnAttackSpeed.y);
            else
                rigidbody2D.velocity = new Vector2((moveOnAttackSpeed.x * multiplier) * -1, moveOnAttackSpeed.y);
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
                rigidbody2D.velocity = new Vector2(0.0f, rigidbody2D.velocity.y);
            }
        }

        private void CheckTouchDamage()
        {
            if (canDamageOnTouch &&
                Time.time >= lastTouchDamageTime + touchDamageCooldown)
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
                    whatIsPlayer);

                if (hit != null)
                {
                    lastTouchDamageTime = Time.time;
                    DamageInfo dInfo = new DamageInfo(touchDamage, transform.position.x);

                    PlayerController bctrl = hit.transform.GetComponent<PlayerController>();
                    if (bctrl != null && (bctrl is IDamageable))
                    {
                        bctrl.Damage(this, dInfo);
                    }
                }
            }
        }

        public virtual void Damage(float amount, int attackN)
        {
            if (controllerKind == EControllerKind.NPC ||
                controllerKind == EControllerKind.Neutral)
                return;

            if (Time.time > damagedTimeCD && currentState != ECharacterState.Dead)
            {
                StopAllCoroutines();

                playerFacingDirection = playerController.IsFacingRight();

                Instantiate(hitParticles, transform.position, Quaternion.Euler(0.0f, 0.0f, Random.Range(0.0f, 360f)));
                if (firstAttack)
                    hitAttackSFX1.Play();
                else
                    hitAttackSFX2.Play();
                firstAttack = !firstAttack;

                if (!isInvencible)
                    currentHealth -= amount;

                if (canBeMovedOnAttack && currentHealth >= 0.0f)
                {
                    MoveOnAttack(attackN);
                }

                StartCoroutine(DamageEffect());

                if (currentHealth <= 0f)
                    SwitchStates(ECharacterState.Dead);

                damagedTimeCD = Time.time + 0.15f;
            }
        }

        public void Damage(ref BaseController controller, DamageInfo damageInfo)
        {
            // Not for Enemies ...
        }

        private IEnumerator DamageEffect()
        {
            renderer.color = new Color(1f, 0.334f, 0.305f);
            yield return new WaitForSeconds(0.5f);
            renderer.color = Color.white;
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

        public bool Interact(PlayerController controller, EInteractionKind interactionKind)
        {
            Debug.Log("Interaction of kind '" + interactionKind + "' received.");
            // disables the interaction bubble !!
            dialogueBubble.gameObject.SetActive(false);

            // Open the dialogue canvas
            DialogueManager.Instance.ShowDialogues();

            return true;
        }

        public void ReadyToInteract(PlayerController controller, bool isReady)
        {
            if (isReady && dialogueBubble != null)
                dialogueBubble.gameObject.SetActive(true);
            else
                dialogueBubble.gameObject.SetActive(false);
        }

        public void ReadyToInteract(BaseController controller, bool isReady)
        {
            throw new System.NotImplementedException();
        }
    }
}