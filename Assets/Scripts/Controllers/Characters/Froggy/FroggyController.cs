using System.Collections;
using System.Collections.Generic;
using Controllers;
using Controllers.StateMachine.States;
using Controllers.StateMachine.States.Data;
using UnityEngine;

namespace Controllers.Froggy
{
    public class FroggyController : BaseController
    {
        [Header("Froggy Specific Values")] 
        
        [SerializeField] private IdleStateData _idleStateData;
        [SerializeField] private JumpStateData _jumpStateData;
        [SerializeField] private PrepareAttackStateData _prepareAttackStateData;
        [SerializeField] private AttackStateData _attackStateData;

        public Froggy_IdleState _idleState { get; private set; }
        public JumpState _jumpState { get; private set; }
        public Froggy_PrepareAttackState _prepareAttackState { get; private set; }
        public Froggy_AttackState _attackState { get; private set; }
        
        private float lastJumpTime = float.NegativeInfinity;
        private float jumpCooldownTime;
        private bool canFroggyDie = false;

        protected override void Start()
        {
            base.Start();
            
            controllerKind = EControllerKind.Enemy;
            jumpCooldownTime = Random.Range(2.5f, 5f);
            
            _jumpState = new JumpState(this, StateMachine, "Jumping", _jumpStateData, this);
            _idleState = new Froggy_IdleState(this, StateMachine, "Idle", _idleStateData, this);
            _prepareAttackState = new Froggy_PrepareAttackState(this, StateMachine, "PreparingAttack", _prepareAttackStateData, this);
            _attackState = new Froggy_AttackState(this, StateMachine, "Attacking", _attackStateData, this);
            
            
            StateMachine.Initialize(_idleState);
            
            InvokeRepeating("MoveCejas", 0f, 7f);
        }

        protected override void OnEnterDeadStateStart()
        {
            rigidbody2D.constraints = RigidbodyConstraints2D.None;
            rigidbody2D.AddForce(new Vector2(3f * -FacingDirection, 3f), ForceMode2D.Impulse);
            rigidbody2D.AddTorque(10f * -FacingDirection, ForceMode2D.Impulse);

            StartCoroutine(DeathTiming());
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
    }
}
