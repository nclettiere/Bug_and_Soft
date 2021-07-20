using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character
{
    public class CharacterCombatController : MonoBehaviour
    {
        [SerializeField]
        private bool combatEnabled;
        [SerializeField]
        private float inputTimer, attack1Radius, attack1Damage;
        [SerializeField]
        private Transform attack1HitboxPos;
        [SerializeField]
        private int attacksUnlocked = 2;
        [SerializeField]
        [Range(0.15f, 1f)]
        private float WaitTimeForAttack = 0.75f;
        [SerializeField]
        private LayerMask whatIsEnemy;
        [SerializeField]
        private AudioSource[] swordSwingSFX;

        private Animator anim;

        private bool combatInputTriggered, attacking, awaitingAttack;

        private int attackN = 1;

        private float lastInputTime = Mathf.NegativeInfinity;

        protected CharacterMovement characterMovement;
        protected CharacterControllerBase charController;

        void Awake()
        {
            characterMovement = GetComponent<CharacterMovement>();
            charController = GetComponent<CharacterControllerBase>();

            anim = GetComponent<Animator>();
            anim.SetBool("CanAttack", combatEnabled);

            GameManager.Instance.playerControls.Gameplay.Attack.performed += ctx =>
            {
                if (combatEnabled && !charController.praying && !charController.roll)
                {
                    combatInputTriggered = true;
                    lastInputTime = Time.time;
                }
            };
        }

        private void Update()
        {
            CheckForAttacks();
            CheckHitboxAttack();
        }

        private void CheckForAttacks()
        {

            if (combatInputTriggered)
            {
                // Attack 1
                if (!attacking)
                {
                    StopAllCoroutines(); // Stops reset

                    combatInputTriggered = false;
                    attacking = true;
                    attackN++;
                    awaitingAttack = false;
                    anim.SetBool("AwaitingAttack", false);
                    anim.SetInteger("AttackN", attackN);
                    anim.SetBool("Attacking", attacking);

                    // Tell character movement to add a
                    // force when attacking !!!
                    characterMovement.Attacking(attackN);
                    AttackSwingSFX();
                }
            }

            if (Time.time >= lastInputTime + inputTimer)
            {
                // Awaiting new attacks
                combatInputTriggered = false;
            }
        }

        private void CheckHitboxAttack()
        {
            Collider2D[] detectedObjs = Physics2D.OverlapCircleAll(attack1HitboxPos.position, attack1Radius, whatIsEnemy);

            foreach (Collider2D collider in detectedObjs)
            {
                collider.transform.parent.SendMessage("Damage", attack1Damage);
                // Instantiate hit particle
            }
        }

        private void FinishAttack()
        {
            if (attackN + 1 > attacksUnlocked)
            {
                ResetAttackNow();
                StopAllCoroutines();
                return;
            }

            attacking = false;
            awaitingAttack = true;
            anim.SetBool("Attacking", attacking);
            anim.SetBool("AwaitingAttack", awaitingAttack);

            StartCoroutine(ResetAttack(attackN));
        }

        /// <summary>
        /// Resetea el ataque del jugador si no hace un combo completo !!!
        /// </summary>
        private IEnumerator ResetAttack(int currentAttackN)
        {
            /// Se le da un delay al jugador para que responda
            yield return new WaitForSeconds(WaitTimeForAttack);

            // Si ha transcurrido el tiempo y no se ha accionado ningun attack,
            // se resetea el ataque
            if(currentAttackN == attackN)
                ResetAttackNow();
        }

        private void ResetAttackNow()
        {

            attacking = false;
            attackN = 0;
            awaitingAttack = true;
            anim.SetBool("AwaitingAttack", false);
            anim.SetBool("Attacking", false);
            anim.SetInteger("AttackN", attackN);

        }

        /// <summary>
        /// Accionado cuando la animacion de la espada hace un 'swing'
        /// </summary>
        public void AttackSwingSFX()
        {
            if ((attackN - 1) >= 0 && (attackN - 1) <= swordSwingSFX.Length)
                swordSwingSFX[attackN - 1].Play();
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(attack1HitboxPos.position, attack1Radius);
        }
    }
}