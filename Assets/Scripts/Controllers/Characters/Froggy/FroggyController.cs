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
        [SerializeField] private AttackStateData _attackStateData;
        [SerializeField] private DeadStateData _deadStateData;

        public GameObject _froggyTongue;

        [Header("Froggy Specific Values")] [SerializeField]
        private IdleStateData _idleStateData;

        [SerializeField] private JumpStateData _jumpStateData;
        [SerializeField] private PrepareAttackStateData _prepareAttackStateData;
        private bool canFroggyDie = false;
        private float jumpCooldownTime;

        private float lastJumpTime = float.NegativeInfinity;

        public Froggy_IdleState _idleState { get; private set; }
        public JumpState _jumpState { get; private set; }
        public Froggy_PrepareAttackState _prepareAttackState { get; private set; }
        public Froggy_AttackState _attackState { get; private set; }
        public Froggy_DeadState _deadState { get; private set; }

        protected override void Start()
        {
            base.Start();

            controllerKind = EControllerKind.Enemy;
            jumpCooldownTime = Random.Range(2.5f, 5f);

            _jumpState = new JumpState(this, StateMachine, "Jumping", _jumpStateData, this);
            _idleState = new Froggy_IdleState(this, StateMachine, "Idle", _idleStateData, this);
            _prepareAttackState =
                new Froggy_PrepareAttackState(this, StateMachine, "PreparingAttack", _prepareAttackStateData, this);
            _attackState = new Froggy_AttackState(this, StateMachine, "Attacking", _attackStateData, this);
            _deadState = new Froggy_DeadState(this, StateMachine, "Dead", _deadStateData, this);

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
            float rotationZ = (FacingDirection == 1 ? 0f : 180f);
            Instantiate(_froggyTongue, transform.position, Quaternion.Inverse(this.transform.rotation))
                .GetComponent<FroggyTongueController>().SetProps(this, _attackState);
        }

        private IEnumerator DeathTiming()
        {
            yield return new WaitForSeconds(2.5f);
            canCharacterDie = true;
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
    }
}