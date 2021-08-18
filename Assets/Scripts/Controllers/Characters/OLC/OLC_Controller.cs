using System.Collections;
using System.Collections.Generic;
using Controllers;
using Controllers.Damage;
using Controllers.StateMachine.States;
using Controllers.StateMachine.States.Data;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Controllers.Froggy
{
    public class OLC_Controller : BaseController
    {
        public GameObject _plumasAttack;

        [Header("OLC Specific Values")] [SerializeField]
        private AttackStateData _attackStateData;

        [SerializeField] private DeadStateData _deadStateData;
        [SerializeField] private IdleStateData _idleStateData;
        [SerializeField] private JumpStateData _jumpStateData;
        [SerializeField] private PrepareAttackStateData _prepareAttackStateData;
        [SerializeField] private DamageStateData _damageStateData;
        
        private FroggyTongueController instatiatedTongue;

        public OLC_IdleState _idleState { get; private set; }
        public OLC_RenegadeState _renegadeState { get; private set; }
        public OLC_AttackState _attackState { get; private set; }

        // Super Froggy
        public bool transforming;
        public int currentPhase = 1;
        public bool transformed;
        
        public int hallPosition = 0;

        protected override void Start()
        {
            base.Start();

            _idleState = new OLC_IdleState(this, StateMachine, "Idle", _idleStateData, this);
            _renegadeState = new OLC_RenegadeState(this, StateMachine, "Renegade", this);
            _attackState = new OLC_AttackState(this, StateMachine, "Attacking", _attackStateData, this);

            StateMachine.Initialize(_idleState);

            InvokeRepeating("MoveCejas", 0f, 7f);
        }

        protected override void Update()
        {
            base.Update();
        }

        public void ShootPlumas()
        {
            Instantiate(_plumasAttack, transform.position, Quaternion.Euler(0, 0, 0));
        }
    }
}