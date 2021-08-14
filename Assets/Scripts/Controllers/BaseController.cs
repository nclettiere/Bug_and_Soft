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
            if (dead ||
                controllerKind == EControllerKind.NPC ||
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
                    Die();

                damagedTimeCD = Time.time + 0.15f;
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
            if (isReady && dialogueBubble != null)
                dialogueBubble.gameObject.SetActive(true);
            else
                dialogueBubble.gameObject.SetActive(false);
        }

        public void ReadyToInteract(BaseController controller, bool isReady)
        {
            throw new System.NotImplementedException();
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
            currentHealth = maxHealth;

            StateMachine = new ControllerStateMachine();
        }

        private void Update()
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
                    CheckPlayerInRange();
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
                Gizmos.color = Color.blue;
                Gizmos.DrawLine(wallCheck.position,
                    new Vector2((wallCheck.position.x + (ctrlData.playerDetectionDistance * FacingDirection)),
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

        public bool CheckPlayerInRange()
        {
            RaycastHit2D hit = Physics2D.Raycast(wallCheck.position, transform.right, ctrlData.playerDetectionDistance,
                ctrlData.whatIsPlayer);

            if (hit.collider != null)
            {
                if (hit.collider.CompareTag("Player"))
                    return true;
            }

            return false;
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
                rBody.velocity = new Vector2((moveOnAttackSpeed.x * multiplier), moveOnAttackSpeed.y);
            else
                rBody.velocity = new Vector2((moveOnAttackSpeed.x * multiplier) * -1, moveOnAttackSpeed.y);
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

        private void CheckTouchDamage()
        {
            //if (canDamageOnTouch &&
            //    Time.time >= lastTouchDamageTime + touchDamageCooldown)
            //{
            //    touchDamageBottomLeft.Set(
            //        touchDamageCheck.position.x - (touchDamageWidth / 2),
            //        touchDamageCheck.position.y - (touchDamageHeight / 2));
//
            //    touchDamageTopRight.Set(
            //        touchDamageCheck.position.x + (touchDamageWidth / 2),
            //        touchDamageCheck.position.y + (touchDamageHeight / 2));
//
            //    Collider2D hit = Physics2D.OverlapArea(
            //        touchDamageBottomLeft,
            //        touchDamageTopRight,
            //        whatIsPlayer);
//
            //    if (hit != null)
            //    {
            //        lastTouchDamageTime = Time.time;
            //DamageInfo dInfo = new DamageInfo(touchDamage, transform.position.x);
//
            //        PlayerController bctrl = hit.transform.GetComponent<PlayerController>();
            //        if (bctrl != null && (bctrl is IDamageable))
            //        {
            //            bctrl.Damage(this, dInfo);
            //        }
            //    }
            //}
        }

        private IEnumerator DamageEffect()
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
            Destroy(gameObject);
        }

        public void DestroyNow(GameObject lapida)
        {
            renderer.enabled = false;
            Instantiate(lapida, transform.position, Quaternion.Euler(0f, 0f, 0f));
            Destroy(gameObject);
        }

        #region UserVariables

        public int FacingDirection { get; private set; } =
            1; // Solo se puede modificar en el codigo, por eso el private set; xd

        public bool IsReadyToAttack { get; set; }
        public ControllerStateMachine StateMachine = new ControllerStateMachine();
        public ControllerData ctrlData;
        public float deadMaxWait = 5f;

        [SerializeField] private protected EControllerKind controllerKind = EControllerKind.NPC;
        [SerializeField] private ECharacterKind characterKind;
        [SerializeField] private float interactionRadius = 4f;
        [SerializeField] private float maxHealth, moveOnAttackDuration, touchDamageCooldown, generalSpeed = 40f;
        [SerializeField] private int touchDamage = 12;
        [SerializeField] private bool isInvencible, canBeStunned, canDamageOnTouch, canBeMovedOnAttack, movedOnAttack;
        [SerializeField] private Vector2 moveOnAttackSpeed;
        [SerializeField] private GameObject hitParticles;
        [SerializeField] private AudioSource hitAttackSFX1;
        [SerializeField] private AudioSource hitAttackSFX2;
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

        private bool dead;
        protected BaseMovementController baseMovementController;
        protected Animator characterAnimator;
        protected internal Rigidbody2D rBody;
        protected PlayerController playerController;
        private SpriteRenderer renderer;
        private ECharacterState currentState;
        protected bool playerFacingDirection;
        public bool canCharacterDie = false;

        public bool wallDetected { get; private set; }
        public bool groundDetected { get; private set; }
        public bool topDetected { get; private set; }

        private bool
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

        private float
            currentHealth,
            moveOnAttackStart,
            damagedTimeCD = float.NegativeInfinity,
            lastAngularVelocity,
            lastTouchDamageTime;

        #endregion
    }
}