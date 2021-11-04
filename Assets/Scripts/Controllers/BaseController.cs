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
using UnityEngine.AI;
using UnityEngine.Events;
using World;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

namespace Controllers
{
    public class BaseController :
        MonoBehaviour,
        IDamageable,
        IInteractive
    {
        public virtual void Damage(DamageInfo damageInfo)
        {
            if (dead ||
                controllerKind == EControllerKind.NPC ||
                controllerKind == EControllerKind.Neutral)
                return;

            if (Time.time > damagedTimeCD)
            {
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
                {
                    GivePlayerExp();
                    OnLifeTimeEnded.Invoke();
                    Die();
                }

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
            if (OnBodyGroundCheckEvent == null)
                OnBodyGroundCheckEvent = new UnityEvent();
            if (OnLifeTimeEnded == null)
                OnLifeTimeEnded = new UnityEvent();
        }

        protected virtual void Start()
        {
            baseMovementController = GetComponent<BaseMovementController>();
            characterAnimator = GetComponent<Animator>();
            if(characterKind != ECharacterKind.Mortadelo)
                rBody = GetComponent<Rigidbody2D>();
            renderer = GetComponent<SpriteRenderer>();
            playerController = GameObject.Find("Player").GetComponent<PlayerController>();
            currentHealth = ctrlData.maxHealth;

            StateMachine = new ControllerStateMachine();

            if (characterKind != ECharacterKind.Dummy && characterKind != ECharacterKind.Njord &&
                characterKind != ECharacterKind.Pepe)
            {
                cachedGroundCheck = CheckGround();
                cachedBodyGroundCheck = CheckOnBodyTouchGround();
            }
        }

        protected virtual void Update()
        {
            if (IsCharacterOnScreen)
            {
                if (GameManager.Instance.IsGamePaused())
                {
                    if (!savedRigidData && characterKind != ECharacterKind.Mortadelo)
                    {
                        lastVelocity = rBody.velocity;
                        lastAngularVelocity = rBody.angularVelocity;
                        rBody.bodyType = RigidbodyType2D.Static;
                        rBody.Sleep();

                        savedRigidData = true;
                    }

                    return;
                }

                if (savedRigidData && characterKind != ECharacterKind.Mortadelo)
                {
                    rBody.bodyType = RigidbodyType2D.Dynamic;
                    rBody.velocity = lastVelocity;
                    rBody.angularVelocity = lastAngularVelocity;
                    rBody.WakeUp();
                    savedRigidData = false;
                }

                if (characterKind != ECharacterKind.Dummy && characterKind != ECharacterKind.Njord)
                {
                    if (!dead && characterKind != ECharacterKind.Pepe)
                    {
                        CheckPlayerInNearRange();
                        CheckPlayerInLongRange();
                    }

                    StateMachine.CurrentState?.UpdateState();

                    if (!cachedGroundCheck && CheckGround())
                        OnLandEvent.Invoke();

                    cachedGroundCheck = CheckGround();

                    if (!cachedBodyGroundCheck && CheckOnBodyTouchGround())
                        OnBodyGroundCheckEvent.Invoke();

                    cachedBodyGroundCheck = CheckGround();
                }
            }
        }

        protected virtual void FixedUpdate()
        {
            if (IsCharacterOnScreen)
            {
                if (GameManager.Instance.IsGamePaused() || dead) return;

                if (characterKind != ECharacterKind.Dummy && characterKind != ECharacterKind.Njord)
                    StateMachine.CurrentState.UpdatePhysics();
            }
        }

        
        GUIStyle style = new GUIStyle();
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
                Gizmos.DrawLine(AttacksRange[0].position, AttacksRange[1].position);

                //Long range
                Gizmos.color = Color.cyan;
                Gizmos.DrawLine(AttacksRange[1].position, AttacksRange[2].position);
                
#if UNITY_EDITOR
                
                style.normal.textColor = Color.blue;
                UnityEditor.Handles.color = Color.blue;
                UnityEditor.Handles.Label(AttacksRange[1].position - new Vector3(2, 0), "Attack near range", style);

                style.normal.textColor = Color.cyan;
                UnityEditor.Handles.color = Color.cyan;
                UnityEditor.Handles.Label(AttacksRange[2].position - new Vector3(2, 0), "Attack long range", style);
#endif
            }

            if (controllerKind == EControllerKind.NPC)
            {
                // Interaction gizmo ...
                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(transform.position, interactionRadius);
            }
        }

        public virtual void Flip()
        {
            FacingDirection *= -1;
            transform.Rotate(0.0f, 180.0f, 0.0f);
        }
        
        public virtual void FlipSpriteX(bool flipX)
        {
            renderer.flipX = flipX;
        }

        public void SetVelocity(float velocity)
        {
            if (characterKind != ECharacterKind.Mortadelo)
            {
                velocityStorage.Set(velocity * FacingDirection, rBody.velocity.y);
                rBody.velocity = velocityStorage;
            }
        }

        public void AddForce(Vector2 newForce, bool isImpulse)
        {
            if (characterKind != ECharacterKind.Mortadelo)
            {
                forceStorage.Set(newForce.x * FacingDirection, newForce.y);
                rBody.AddForce(forceStorage, (isImpulse ? ForceMode2D.Impulse : ForceMode2D.Force));
            }
        }

        public void AddTorque(float newTorque, bool isImpulse)
        {
            if(characterKind != ECharacterKind.Mortadelo)
                rBody.AddTorque(newTorque * FacingDirection, (isImpulse ? ForceMode2D.Impulse : ForceMode2D.Force));
        }

        public T GetMovementController<T>() where T : BaseMovementController
        {
            return (T)baseMovementController;
        }

        public Animator GetAnimator()
        {
            return characterAnimator;
        }

        public Transform GetTransfrom()
        {
            return transform;
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

        public bool CheckPlayerInNearRange()
        {
            RaycastHit2D hit =
                Physics2D.Linecast(AttacksRange[0].position, AttacksRange[1].position, ctrlData.whatIsPlayer);
            return hit.collider != null && hit.collider.CompareTag("Player");
        }

        public bool CheckPlayerInLongRange()
        {
            RaycastHit2D hit =
                Physics2D.Linecast(AttacksRange[1].position, AttacksRange[2].position, ctrlData.whatIsPlayer);
            return hit.collider != null && hit.collider.CompareTag("Player");
        }

        protected void MoveOnDamaged(int direction, Vector2 moveOnDamagedForce)
        {
            if (characterKind != ECharacterKind.Mortadelo)
            {
                moveOnDamaged = true;
                moveOnDamagedStartTime = Time.time;
                rBody.AddForce(new Vector2(moveOnDamagedForce.x * direction, moveOnDamagedForce.y));
            }
        }

        protected virtual void OnBecameVisible()
        {
            IsCharacterOnScreen = true;
            GameManager.Instance.EnemiesInScreen
                .Add(this);
        }

        protected virtual void OnBecameInvisible()
        {
            IsCharacterOnScreen = false;
            GameManager.Instance.EnemiesInScreen
                .Remove(this);
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

        public bool CheckOnBodyTouchGround()
        {
            touchDamageBottomLeft.Set(
                touchDamageCheck.position.x - (touchDamageWidth / 2),
                touchDamageCheck.position.y - (touchDamageHeight / 2));

            touchDamageTopRight.Set(
                touchDamageCheck.position.x + (touchDamageWidth / 2),
                touchDamageCheck.position.y + (touchDamageHeight / 2));

            return Physics2D.OverlapArea(
                touchDamageBottomLeft,
                touchDamageTopRight,
                ctrlData.whatIsGround);
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
            StateMachine.ChangeState(null);
        }

        public virtual IEnumerator OnDie()
        {
            yield return 0;
        }

        public void DestroyNow()
        {
            StopAllCoroutines();
            // Dropear items si tenes LUCKY BOYYYYYYY
            DropItems();
            Destroy(gameObject);
        }

        public void DestroyNow(GameObject lapida)
        {
            StopAllCoroutines();
            renderer.enabled = false;
            Instantiate(lapida, transform.position, Quaternion.Euler(0f, 0f, 0f));
            DropItems();
            Destroy(gameObject);
        }


        public virtual void DropItems()
        {
            if (itemDrops != null && itemDrops.Length > 0)
            {
                // TODO: Luck factor !!!
                // MEN TI RA -> UNA MIERDA VA A VER LUCK
                // ALSO..
                int random = Random.Range(0, 100);

                // Dropea algo random
                if (random <= 45)
                {
                    int item = Random.Range(0, itemDrops.Length - 1);
                    Instantiate(itemDrops[item], transform.position, Quaternion.Euler(0f, 0f, 0f));
                }
            }
        }

        public bool CanCharacterInteract()
        {
            return canInteract;
        }

        public void ShowTauntIndicator()
        {
            if (enemyTauntIndicator != null)
            {
                enemyTauntIndicator.gameObject.SetActive(true);
                enemyTauntIndicator.Show();
            }
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
        public ECharacterKind characterKind;
        [SerializeField] private protected float interactionRadius = 4f;
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
        [SerializeField] private protected EnemyTauntIndicator enemyTauntIndicator;

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
            touchDamageCheck,
            playerDetectCenterCheck;

        [SerializeField] private Transform[] AttacksRange;

        [Header("ItemDrops")] [SerializeField] private protected GameObject[] itemDrops;

        [Header("Events Zone")] public UnityEvent OnLandEvent;
        [Header("Events Zone")] public UnityEvent OnBodyGroundCheckEvent;

        #endregion

        #region Variables

        protected bool dead;
        protected BaseMovementController baseMovementController;
        protected Animator characterAnimator;
        protected internal Rigidbody2D rBody;
        protected PlayerController playerController;
        private SpriteRenderer renderer;
        protected string currentState;
        protected bool playerFacingDirection;
        protected internal bool canInteract = true;
        private bool moveOnDamaged;
        private float moveOnDamagedStartTime = float.NegativeInfinity;

        public bool wallDetected { get; private set; }
        public bool groundDetected { get; private set; }
        public bool topDetected { get; private set; }

        private bool deadCoroutineStarted;
        public UnityEvent OnLifeTimeEnded;

        protected bool
            lastGroundDetected,
            lapidaIntance = false,
            firstAttack,
            cachedGroundCheck,
            cachedBodyGroundCheck;

        protected internal bool
            canPlayerInteract = false;

        protected bool
            savedRigidData;

        private Vector2
            movement;

        protected Vector2
            lastVelocity;

        private Vector2
            touchDamageBottomLeft,
            touchDamageTopRight,
            velocityStorage,
            forceStorage;

        protected float
            moveOnAttackStart,
            damagedTimeCD = float.NegativeInfinity,
            lastAngularVelocity,
            lastTouchDamageTime;

        protected bool IsCharacterOnScreen;

        #endregion

        public bool IsDead()
        {
            return dead;
        }

        public void GivePlayerExp()
        {
            GameManager.Instance.AddExperience(ctrlData.experienceReward);
        }
        
        protected void LookAtPlayer()
        {
            float playerPositionX = GameManager.Instance.GetPlayerTransform().position.x;
            if ((transform.position.x < playerPositionX && FacingDirection == -1) ||
                (transform.position.x > playerPositionX && FacingDirection == 1))
                Flip();
        }
    }
}