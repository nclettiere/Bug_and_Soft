using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Controllers.Movement;
using Player;

namespace Controllers
{
    public class BaseController : MonoBehaviour, IDamageable
    {
        #region UserVariables
        [SerializeField]
        protected EControllerKind controllerKind = EControllerKind.NPC;
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
        #endregion

        #region Variables
        protected BaseMovementController baseMovementController;
        protected Animator characterAnimator;
        protected Rigidbody2D RigidBdy;
        private float currentHealth, moveOnAttackStart;
        protected ECharacterKind  characterKind;
        protected PlayerController playerController;
        protected bool playerFacingDirection;
        protected bool playerOnLeft;
        private bool firstAttack2;
        private float damagedTimeCD = float.NegativeInfinity;
        #endregion

        protected virtual void OnStart()
        {
            // Debe ser llenado en una clase heredada
            // Ej: DummyController.cs
        }

        protected virtual void OnUpdate()
        {
            // Debe ser llenado en una clase heredada
            // Ej: DummyController.cs
        }

        protected virtual void OnFixedUpdate()
        {
            // Debe ser llenado en una clase heredada
            // Ej: DummyController.cs
        }

        /// <summary>
        /// Obtiene el controller de moviemento del personaje especificando que tipo de moviemnto usa en T
        /// </summary>
        /// <typeparam name="T">Clase derivada de BaseMovementController</typeparam>
        /// <returns>Un objeto de clase T que hereda de BaseMovementController almacenado en el BaseController.</returns>
        public T GetMovementController<T>() where T : BaseMovementController
        {
            return (T) baseMovementController;
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

        private void Update() {
            CheckMoveOnAttack();
            OnUpdate();
        }

        private void FixedUpdate() {
            OnFixedUpdate();
        }

        private void MoveOnAttack(int attackN)
        {
            movedOnAttack = true;
            moveOnAttackStart = Time.time;

            if (playerFacingDirection)
                RigidBdy.velocity = new Vector2((moveOnAttackSpeed.x * attackN), moveOnAttackSpeed.y);
            else
                RigidBdy.velocity = new Vector2((moveOnAttackSpeed.x * attackN) * -1, moveOnAttackSpeed.y);
        }

        private void CheckMoveOnAttack()
        {
            if(Time.time >= moveOnAttackStart + moveOnAttackDuration && movedOnAttack)
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
