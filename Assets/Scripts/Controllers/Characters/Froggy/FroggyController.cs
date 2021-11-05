using System;
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
        [SerializeField] private GameObject _farty;
        [SerializeField] private FroggyTongueController tongueControllerongue;
        [SerializeField] private Sprite visiblePepeQC;
        public GameObject _superFroggyNuke;
        private bool canFroggyDie = false;

        public Froggy_IdleState _idleState { get; private set; }
        public JumpState _jumpState { get; private set; }
        public Froggy_PrepareAttackState _prepareAttackState { get; private set; }
        public Froggy_AttackState _attackState { get; private set; }
        public Froggy_DeadState _deadState { get; private set; }
        public Froggy_DamageState _damageState { get; private set; }
        public Froggy_NearAttackState _nearAttackState { get; private set; }

        // Super Froggy
        public bool transforming;
        public int currentPhase = 1;
        public bool transformed;
        [SerializeField] private SuperFroggy_SecondPhaseStateData _superFroggySecondPhaseStateData;
        
        public SuperFroggy_SecondPhase _superFroggySecondPhase { get; private set; }
        public int hallPosition = 0;

        protected override void Start()
        {
            base.Start();

            _jumpState = new JumpState(this, StateMachine, "Jumping", _jumpStateData, this);
            _idleState = new Froggy_IdleState(this, StateMachine, "Idle", this);
            _prepareAttackState =
                new Froggy_PrepareAttackState(this, StateMachine, "PreparingAttack", _prepareAttackStateData, this);
            _attackState = new Froggy_AttackState(this, StateMachine, "Attacking", _attackStateData, this);
            _deadState = new Froggy_DeadState(this, StateMachine, "Dead", _deadStateData, this);
            _damageState = new Froggy_DamageState(this, StateMachine, "Dead", _damageStateData, this);
            _nearAttackState = new Froggy_NearAttackState(this, StateMachine, "Idle", _jumpStateData, this);
            //_superFroggySecondPhase = new SuperFroggy_SecondPhase(this, StateMachine, "Idle", _superFroggySecondPhaseStateData, this);
            
            StateMachine.Initialize(_idleState);

            InvokeRepeating("MoveCejas", 0f, 7f);
        }

        protected override void Update()
        {
            base.Update();
            currentState = StateMachine.CurrentState.ToString();
        }

        public void Anim_OnAttackingAnimStarted()
        {
            _attackState.attackStarted = true;
        }

        public void SpecialAttack()
        {
            tongueControllerongue.gameObject.SetActive(true);
            tongueControllerongue.SetProps(this);
            tongueControllerongue.Activate();
        }

        public void OnTongueFinish()
        {
            _attackState.OnTongeFinished();
        }

        private void MoveCejas()
        {
            GetAnimator().SetBool("EyebrowsMovement", true);
        }
        
        private void AnimCejasEnded()
        {
            GetAnimator().SetBool("EyebrowsMovement", false);
        }

        public override void Die()
        {
            dead = true;
            GetAnimator().SetBool("Idle", false);
            GetAnimator().SetBool("PreparingAttack", false);
            GetAnimator().SetBool("Attacking", false);
            GetAnimator().SetBool("Dead", true);
            
            StateMachine.ChangeState(_deadState);
        }

        public override IEnumerator OnDie()
        {
            GameManager.Instance.AddPlayerKrowns(KrownReward);
            
            AddForce(new Vector2(20f * -FacingDirection, 10f), true);
            //AddTorque(10000f * -FacingDirection, false);
            
            yield return new WaitForSeconds(1);
            
            Destroy(gameObject);
        }

        protected override void OnBecameVisible()
        {
            base.OnBecameVisible();

            if (!GameManager.Instance.HasSeenFroggy)
            {
                GameManager.Instance.GetPepeController()
                    .ShowQuickChat(new Tuple<Sprite, int>(visiblePepeQC, 1));
                GameManager.Instance.HasSeenFroggy = true;
            }
        }
    }
}