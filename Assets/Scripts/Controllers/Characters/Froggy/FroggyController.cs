using System.Collections;
using System.Collections.Generic;
using Controllers;
using Controllers.StateMachine.States;
using Controllers.StateMachine.States.Data;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Controllers.Froggy
{
    public class FroggyController : BaseController
    {
        public GameObject _froggyTongue;

        [Header("Froggy Specific Values")] [SerializeField]
        private AttackStateData _attackStateData;

        [SerializeField] private DeadStateData _deadStateData;
        [SerializeField] private IdleStateData _idleStateData;
        [SerializeField] private JumpStateData _jumpStateData;
        [SerializeField] private PrepareAttackStateData _prepareAttackStateData;
        [SerializeField] private DamageStateData _damageStateData;
        private bool canFroggyDie = false;
        private FroggyTongueController instatiatedTongue;

        public Froggy_IdleState _idleState { get; private set; }
        public JumpState _jumpState { get; private set; }
        public Froggy_PrepareAttackState _prepareAttackState { get; private set; }
        public Froggy_AttackState _attackState { get; private set; }
        public Froggy_DeadState _deadState { get; private set; }
        public Froggy_DamageState _damageState { get; private set; }
        public Froggy_NearAttackState _nearAttackState { get; private set; }

        protected override void Start()
        {
            base.Start();

            controllerKind = EControllerKind.Enemy;

            _jumpState = new JumpState(this, StateMachine, "Jumping", _jumpStateData, this);
            _idleState = new Froggy_IdleState(this, StateMachine, "Idle", _idleStateData, this);
            _prepareAttackState =
                new Froggy_PrepareAttackState(this, StateMachine, "PreparingAttack", _prepareAttackStateData, this);
            _attackState = new Froggy_AttackState(this, StateMachine, "Attacking", _attackStateData, this);
            _deadState = new Froggy_DeadState(this, StateMachine, "Dead", _deadStateData, this);
            _damageState = new Froggy_DamageState(this, StateMachine, "Dead", _damageStateData, this);
            _nearAttackState = new Froggy_NearAttackState(this, StateMachine, "Idle", _jumpStateData, this);
            
            StateMachine.Initialize(_idleState);

            InvokeRepeating("MoveCejas", 0f, 7f);
        }

        public void Anim_OnAttackingAnimStarted()
        {
            _attackState.attackStarted = true;
        }

        /// <summary>
        /// Ataque especial de Froggy => Lengua suculenta
        /// </summary>
        public void SpecialAttack()
        {
            instatiatedTongue = Instantiate(_froggyTongue, transform.position, Quaternion.Inverse(transform.rotation))
                .GetComponent<FroggyTongueController>();
            instatiatedTongue.SetProps(this, _attackState);
        }

        /// <summary>
        ///     Metodo para empezar la animacion de mover cejas del PJ Njord
        /// </summary>
        private void MoveCejas()
        {
            GetAnimator().SetBool("EyebrowsMovement", true);
        }

        /// <summary>
        ///     Metodo para terminar la animacion de mover cejas del PJ Njord
        /// </summary>
        /// <remarks>
        ///     Es llamado al final de la animacion cejas.
        /// </remarks>
        private void AnimCejasEnded()
        {
            GetAnimator().SetBool("EyebrowsMovement", false);
        }

        public override void Die()
        {
            base.Die();
            StateMachine.ChangeState(_deadState);
        }

        public override void Damage(float amount, int attackN)
        {
            if (dead ||
                controllerKind == EControllerKind.NPC ||
                controllerKind == EControllerKind.Neutral)
                return;

            if (Time.time > damagedTimeCD && currentState != ECharacterState.Dead)
            {
                if (instatiatedTongue != null)
                    instatiatedTongue.Cancel();
                StateMachine.ChangeState(_damageState);

                StopAllCoroutines();

                playerFacingDirection = playerController.IsFacingRight();

                Instantiate(hitParticles, transform.position, Quaternion.Euler(0.0f, 0.0f, Random.Range(0.0f, 360f)));
                if (firstAttack)
                    hitAttackSFX1.Play();
                else
                    hitAttackSFX2.Play();
                firstAttack = !firstAttack;

                if (!isInvencible)
                    currentHealth -= (int) amount;

                if (canBeMovedOnAttack && currentHealth >= 0.0f)
                {
                    MoveOnAttack(attackN);
                }

                StartCoroutine(DamageEffect());

                if (currentHealth <= 0f)
                    Die();
                else
                    StateMachine.ChangeState(_damageState);

                damagedTimeCD = Time.time + 0.45f;
            }
        }
    }
}