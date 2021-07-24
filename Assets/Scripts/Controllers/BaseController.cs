using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Controllers.Movement;
using Player;

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
        IDamageable
    {
        #region UserVariables
        [SerializeField]
        protected EControllerKind controllerKind = EControllerKind.NPC;
        [SerializeField]
        protected float interactionRadius = 4f;
        [SerializeField]
        protected float maxHealth, moveOnAttackDuration, generalSpeed = 40f;
        [SerializeField]
        protected bool isInvencible, canBeStunned, canBeMovedOnAttack, movedOnAttack;
        [SerializeField]
        protected Vector2 moveOnAttackSpeed;
        [SerializeField]
        protected GameObject hitParticles;
        [SerializeField]
        protected AudioSource hitAttackSFX1;
        [SerializeField]
        protected AudioSource hitAttackSFX2;
        [SerializeField]
        protected GameObject dialogueBubble;
        #endregion

        #region Variables
        protected BaseMovementController baseMovementController;
        protected Animator characterAnimator;
        protected Rigidbody2D RigidBdy;
        private float currentHealth, moveOnAttackStart;
        protected ECharacterKind characterKind;
        protected PlayerController playerController;
        protected bool playerFacingDirection;
        protected bool playerOnLeft;
        private bool firstAttack2;
        private float damagedTimeCD = float.NegativeInfinity;
        private bool canPlayerInteract = false;
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


        private void Start()
        {
            baseMovementController = GetComponent<BaseMovementController>();
            characterAnimator = GetComponent<Animator>();
            RigidBdy = GetComponent<Rigidbody2D>();

            playerController = GameObject.Find("Player").GetComponent<PlayerController>();

            currentHealth = maxHealth;

            OnStart();
        }

        private void Update()
        {
            CheckInteractions();
            CheckMoveOnAttack();
            OnUpdate();
        }

        private void FixedUpdate()
        {
            OnFixedUpdate();
        }

        private void OnDrawGizmos()
        {
            // Interaction gizmo
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, interactionRadius);
            // other ...
            DrawGizmos();
        }

        /// <summary>
        ///     Obtiene el controller de moviemento del personaje especificando que tipo de moviemnto usa en T
        /// </summary>
        /// <typeparam name="T">Clase derivada de BaseMovementController</typeparam>
        /// <returns>Un objeto de clase T que hereda de BaseMovementController almacenado en el BaseController.</returns>
        public T GetMovementController<T>() where T : BaseMovementController
        {
            return (T)baseMovementController;
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
                RigidBdy.velocity = new Vector2((moveOnAttackSpeed.x * multiplier), moveOnAttackSpeed.y);
            else
                RigidBdy.velocity = new Vector2((moveOnAttackSpeed.x * multiplier) * -1, moveOnAttackSpeed.y);
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
                RigidBdy.velocity = new Vector2(0.0f, RigidBdy.velocity.y);
            }
        }

        public virtual void Damage(float amount, int attackN)
        {
            if (Time.time > damagedTimeCD)
            {
                playerFacingDirection = playerController.IsFacingRight();

                Instantiate(hitParticles, transform.position, Quaternion.Euler(0.0f, 0.0f, Random.Range(0.0f, 360f)));

                if (firstAttack2)
                    hitAttackSFX1.Play();
                else
                    hitAttackSFX2.Play();

                firstAttack2 = !firstAttack2;


                if (!isInvencible)
                {
                    currentHealth -= amount;
                }

                if (canBeMovedOnAttack && currentHealth >= 0.0f)
                {
                    MoveOnAttack(attackN);
                }

                damagedTimeCD = Time.time + 0.15f;
            }
        }
    }
}
