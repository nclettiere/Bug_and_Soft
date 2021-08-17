using Controllers;
using System.Collections;
using System.Collections.Generic;
using Controllers.Damage;
using UnityEngine;

namespace Player
{
    public class PlayerCombatController : MonoBehaviour
    {
        internal bool attacking = false,
            awaitingAttack = true,
            respawning = false,
            hasAttackAnimationStarted = false,
            damagedApplied = false;

        private int attackN = 0;

        [SerializeField] private int[] attacksDamage;

        [SerializeField] private Transform[] attacksHitboxPositions;

        [SerializeField] private float[] attacksHitboxRadius;

        [SerializeField] private float generalInputCooldown;

        private Color[] gizmosColors;
        private float lastInputTime = float.NegativeInfinity;

        private Animator pAnimator;

        private PlayerController pCtrl;
        private PlayerLadderClimbingController pLadderCtrl;

        [SerializeField] private Material playerMaterial;

        private PlayerMovementController pMovCtrl;

        [SerializeField] private AudioSource[] swordSwingSFX;

        [SerializeField] private int UnlockedAttacksCount = 2;

        [SerializeField] [Range(0.15f, 1f)] private float WaitForNextAttackMaxTime = 0.75f;

        [SerializeField] private LayerMask whatCanBeDamaged;

        private void Awake()
        {
            pCtrl = GetComponent<PlayerController>();
            pMovCtrl = GetComponent<PlayerMovementController>();
            pLadderCtrl = GetComponent<PlayerLadderClimbingController>();
            pAnimator = GetComponent<Animator>();

            GameManager.Instance.GetPlayerControls().Gameplay.Attack.performed += ctx =>
            {
                if (GameManager.Instance.IsGamePaused() || !GameManager.Instance.GetIsInputEnabled()) return;

                if (!pCtrl.respawning && !pCtrl.isEnrolledInDialogue && !pCtrl.praying && !pCtrl.roll &&
                    !pMovCtrl.IsTouchingLedge)
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

                            // WACHIN Aumentamos el HDR del color del shader para que se vea
                            // mas brillante CACHINN
                            // ARTE     A  R  T  E
                            playerMaterial.SetFloat("EmmisiveIntensity", 0.25f);

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
            if (GameManager.Instance.IsGamePaused() || !GameManager.Instance.GetIsInputEnabled()) return;

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

            // Reseteamos la intensidad de la espada
            playerMaterial.SetFloat("EmmisiveIntensity", 0.04f);

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

                RaycastHit2D[] detectedObjs =
                    Physics2D.CircleCastAll(transform.position, attacksHitboxRadius[attackN - 1],
                        new Vector2(0f, 0f));

                foreach (RaycastHit2D raycast in detectedObjs)
                {
                    // Check si implementa la interface IDamageable
                    BaseController bctrl = raycast.transform.GetComponent<BaseController>();
                    if (bctrl != null && (bctrl is IDamageable))
                    {
                        DamageInfo damageInfo = new DamageInfo(attacksDamage[attackN - 1], transform.position.x, true);
                        damageInfo.MoveOnAttackForce = new Vector2(5f * attackN, 10f);
                        bctrl.Damage(damageInfo);
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