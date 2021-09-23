using System.Collections;
using Controllers;
using Controllers.Damage;
using UnityEngine;

namespace Player
{
    public class PlayerCombatController : MonoBehaviour
    {
        [SerializeField] private int[] attacksDamage;

        [SerializeField] private Transform[] attacksHitboxPositions;

        [SerializeField] private float[] attacksHitboxRadius;

        [SerializeField] private float generalInputCooldown;

        [SerializeField] private Material playerMaterial;

        [SerializeField] private AudioSource[] swordSwingSFX;

        [SerializeField] private int UnlockedAttacksCount = 2;

        [SerializeField] [Range(0.15f, 1f)] private float WaitForNextAttackMaxTime = 0.75f;

        [SerializeField] private LayerMask whatCanBeDamaged;

        internal bool attacking,
            awaitingAttack = true,
            respawning = false,
            hasAttackAnimationStarted,
            damagedApplied;

        private int attackN;

        private Color[] gizmosColors;
        private float lastInputTime = float.NegativeInfinity;

        private Animator pAnimator;

        private PlayerController pCtrl;
        private PlayerLadderClimbingController pLadderCtrl;

        private PlayerMovementController pMovCtrl;

        private void Start()
        {
            pCtrl = GetComponent<PlayerController>();
            pMovCtrl = GetComponent<PlayerMovementController>();
            pLadderCtrl = GetComponent<PlayerLadderClimbingController>();
            pAnimator = GetComponent<Animator>();
        }

        private void Update()
        {
            if (GameManager.Instance.IsGamePaused() || !GameManager.Instance.GetIsInputEnabled()) return;

            CheckAttackHitBox();
        }

        private void OnDrawGizmos()
        {
            if (attacksHitboxPositions.Length == attacksHitboxRadius.Length)
                for (var i = 0; i < attacksHitboxPositions.Length; i++)
                {
                    Gizmos.color = Color.red;
                    Gizmos.DrawWireSphere(
                        attacksHitboxPositions[i].position,
                        attacksHitboxRadius[i]);
                }
        }

        /// <summary>
        ///     Accionado cuando la animacion del ataque termina
        ///     Se pasa a esperar al siguiente ataque en el combo loco (awaitingAttack)
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
        ///     Accionado cuando la animacion de la espada hace un 'swing'
        /// </summary>
        public void AnimAttackSwingEvt()
        {
            if (attackN - 1 >= 0 && attackN - 1 <= swordSwingSFX.Length)
                swordSwingSFX[attackN - 1].Play();

            damagedApplied = false;
        }


        public void AnimHasAttackAnimationStartedEvt()
        {
            hasAttackAnimationStarted = true;
            pAnimator.SetBool("HasAttackAnimationStarted", hasAttackAnimationStarted);
        }

        /// <summary>
        ///     Resetea el ataque del jugador si no hace un combo completo !!!
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

                var detectedObjs =
                    Physics2D.CircleCastAll(transform.position, attacksHitboxRadius[attackN - 1],
                        new Vector2(0f, 0f));

                foreach (var raycast in detectedObjs)
                {
                    // Check si implementa la interface IDamageable
                    var bctrl = raycast.transform.GetComponent<BaseController>();
                    if (bctrl != null && bctrl is IDamageable)
                    {
                        var damageInfo = new DamageInfo(attacksDamage[attackN - 1], transform.position.x, true);
                        damageInfo.MoveOnAttackForce = new Vector2(5f * attackN, 10f);
                        bctrl.Damage(damageInfo);
                    }
                }

                damagedApplied = true;
            }
        }

        public void IncreseDamage()
        {
            attacksDamage[0] = attacksDamage[0] + (int)(0.10f * attacksDamage[0]);
            attacksDamage[1] = attacksDamage[1] + (int)(0.10f * attacksDamage[1]);
        }
        
        public int GetDamagePromedio()
        {
            return ((attacksDamage[0] + attacksDamage[0]) / 2);
        }

        public void AttackPerformed()
        {
            if (GameManager.Instance.IsGamePaused() || !GameManager.Instance.GetIsInputEnabled()) return;

            if (!pCtrl.respawning && !pCtrl.isEnrolledInDialogue && !pCtrl.praying && !pCtrl.roll &&
                !pMovCtrl.IsTouchingLedge)
                if (pLadderCtrl != null && !pLadderCtrl.isClimbing)
                    if (awaitingAttack && Time.time >= lastInputTime)
                    {
                        StopAllCoroutines();

                        attacking = true;
                        attackN++;
                        awaitingAttack = false;

                        damagedApplied = false;

                        playerMaterial.SetFloat("EmmisiveIntensity", 0.25f);

                        pAnimator.SetBool("AwaitingAttack", false);
                        pAnimator.SetBool("Attacking", true);
                        pAnimator.SetInteger("AttackN", attackN);

                        pMovCtrl.AttackMovement(attackN * 2);

                        lastInputTime = Time.time + generalInputCooldown;
                    }
        }
    }
}