using Controllers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class PlayerCombatController : MonoBehaviour
    {
        [SerializeField]
        private float generalInputCooldown;
        [SerializeField]
        [Range(0.15f, 1f)]
        private float WaitForNextAttackMaxTime = 0.75f;
        [SerializeField]
        private int UnlockedAttacksCount = 2;
        [SerializeField]
        private Transform[] attacksHitboxPositions;
        [SerializeField]
        private float[] attacksHitboxRadius;
        [SerializeField]
        private float[] attacksDamage;
        [SerializeField]
        private LayerMask whatCanBeDamaged;
        [SerializeField]
        private AudioSource[] swordSwingSFX;

        protected PlayerController pCtrl;
        protected PlayerMovementController pMovCtrl;
        protected PlayerLadderClimbingController pLadderCtrl;

        internal bool attacking = false, awaitingAttack = true, respawning = false, hasAttackAnimationStarted = false, damagedApplied = false;
        private int attackN = 0;
        private float lastInputTime = float.NegativeInfinity;

        private Color[] gizmosColors;

        private Animator pAnimator;

        private void Awake()
        {
            pCtrl = GetComponent<PlayerController>();
            pMovCtrl = GetComponent<PlayerMovementController>();
            pLadderCtrl = GetComponent<PlayerLadderClimbingController>();
            pAnimator = GetComponent<Animator>();

            GameManager.Instance.playerControls.Gameplay.Attack.performed += ctx =>
            {
                if (!pCtrl.respawning && !pCtrl.praying && !pCtrl.roll && !pMovCtrl.IsTouchingLedge)
                {
                    if (pLadderCtrl != null && !pLadderCtrl.isClimbing)
                    {
                        if (awaitingAttack && Time.time >= lastInputTime)
                        {
                            StopAllCoroutines();

                            attacking = true;
                            attackN++;
                            awaitingAttack = false;

                            damagedApplied = false;

                            pAnimator.SetBool("AwaitingAttack", false);
                            pAnimator.SetBool("Attacking", true);
                            pAnimator.SetInteger("AttackN", attackN);

                            pMovCtrl.AttackMovement(attackN * 2);

                            lastInputTime = Time.time + generalInputCooldown;
                        }
                    }
                }
            };
        }

        private void Update()
        {
            CheckAttackHitBox();
        }

        /// <summary>
        /// Accionado cuando la animacion del ataque termina
        /// Se pasa a esperar al siguiente ataque en el combo loco (awaitingAttack)
        /// </summary>
        public void AnimAttackEndEvt()
        {
            attacking = false;
            awaitingAttack = true;
            hasAttackAnimationStarted = false;
            pAnimator.SetBool("Attacking", false);
            pAnimator.SetBool("AwaitingAttack", awaitingAttack);
            pAnimator.SetBool("HasAttackAnimationStarted", false);
            pMovCtrl.EnableFlip();
            if (attackN + 1 > UnlockedAttacksCount)
            {
                ResetAttackNow();
                return;
            }
            StartCoroutine(ResetAttack());
        }

        /// <summary>
        /// Accionado cuando la animacion de la espada hace un 'swing'
        /// </summary>
        public void AnimAttackSwingEvt()
        {
            if ((attackN - 1) >= 0 && (attackN - 1) <= swordSwingSFX.Length)
                swordSwingSFX[attackN - 1].Play();

            damagedApplied = false;
        }


        public void AnimHasAttackAnimationStartedEvt()
        {
            hasAttackAnimationStarted = true;
            pAnimator.SetBool("HasAttackAnimationStarted", hasAttackAnimationStarted);
        }

        /// <summary>
        /// Resetea el ataque del jugador si no hace un combo completo !!!
        /// </summary>
        private IEnumerator ResetAttack()
        {
            /// Se le da un delay al jugador para que responda
            yield return new WaitForSeconds(WaitForNextAttackMaxTime);
            // Si ha transcurrido el tiempo y no se ha accionado ningun attack,
            // se resetea el ataque
            ResetAttackNow();
        }

        internal void ResetAttackNow()
        {
            attacking = false;
            awaitingAttack = true;
            attackN = 0;
            hasAttackAnimationStarted = false;
            damagedApplied = false;
            pMovCtrl.EnableFlip();
            pAnimator.SetBool("AwaitingAttack", true);
            pAnimator.SetBool("HasAttackAnimationStarted", false);
            pAnimator.SetBool("Attacking", false);
            pAnimator.SetInteger("AttackN", attackN);

        }

        private void CheckAttackHitBox()
        {
            if (!damagedApplied)
            {
                if (attackN <= 0)
                    return;

                Collider2D[] detectedObjs = Physics2D.OverlapCircleAll(attacksHitboxPositions[attackN - 1].position, attacksHitboxRadius[attackN - 1], whatCanBeDamaged);

                foreach (Collider2D collider in detectedObjs)
                {
                    // Check si implementa la interface IDamageable
                    BaseController bctrl = collider.transform.GetComponent<BaseController>();
                    if (bctrl != null && (bctrl is IDamageable))
                    {
                        bctrl.Damage(attacksDamage[attackN - 1], attackN);
                    }
                }
                damagedApplied = true;
            }
        }

        private void OnDrawGizmos()
        {
            if (attacksHitboxPositions.Length == attacksHitboxRadius.Length)
            {
                for (int i = 0; i < attacksHitboxPositions.Length; i++)
                {
                    Gizmos.color = Color.red;
                    Gizmos.DrawWireSphere(
                        attacksHitboxPositions[i].position,
                        attacksHitboxRadius[i]);
                }
            }
        }
    }
}