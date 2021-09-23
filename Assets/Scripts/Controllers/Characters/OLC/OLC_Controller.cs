using System.Collections;
using System.Collections.Generic;
using Controllers;
using Controllers.Damage;
using Controllers.Froggy.States.Data;
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

        [SerializeField] private IdleStateData _idleStateData;
        [SerializeField] private OLC_AttackStateData _olcAttackStateData;
        
        private FroggyTongueController instatiatedTongue;

        public OLC_IdleState _idleState { get; private set; }
        public OLC_RenegadeState _renegadeState { get; private set; }
        public OLC_AttackState _attackState { get; private set; }
        public OLC_StunnedState _stunnedState { get; private set; }
        public OLC_SecondPhase _secondPhase { get; private set; }
        
        public Transform projectilePos;
        public Transform secondPhasePos;
        public Transform[] arrowsPos;

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
            _attackState = new OLC_AttackState(this, StateMachine, "Attacking", _attackStateData, _olcAttackStateData, this);
            _stunnedState = new OLC_StunnedState(this, StateMachine, "Stunned", _idleStateData, this);
            _secondPhase = new OLC_SecondPhase(this, StateMachine, "Jumping", _attackStateData, _olcAttackStateData, this);
            StateMachine.Initialize(_idleState);
        }

        public override void Damage(DamageInfo damageInfo)
        {
            if (dead ||
                controllerKind == EControllerKind.NPC ||
                controllerKind == EControllerKind.Neutral)
                return;

            if (Time.time > damagedTimeCD && currentState != ECharacterState.Dead)
            {
                Debug.Log("DamageReceived (" + damageInfo.DamageAmount + ")");
                StopAllCoroutines();
                
                
                // Obtenemos la posicion del ataque
                int direction = damageInfo.GetAttackDirection(transform.position.x);

                playerFacingDirection = playerController.IsFacingRight();

                Instantiate(hitParticles, transform.position, Quaternion.Euler(0.0f, 0.0f, Random.Range(0.0f, 360f)));
                
                if (firstAttack)
                    hitAttackSFX1.Play();
                else
                    hitAttackSFX2.Play();
                
                firstAttack = !firstAttack;

                if (!isInvencible)
                    currentHealth -= damageInfo.DamageAmount;

                if (damageInfo.MoveOnAttack)
                    MoveOnDamaged(direction, damageInfo.MoveOnAttackForce);

                StartCoroutine(DamageEffect());

                if (currentHealth <= 0f)
                {
                    GameManager.Instance.GetUIManager()
                        .ShowLvlWonPanel();
                    GameManager.PlayerController.RefillHealth();

                    GivePlayerExp();

                    Destroy(gameObject);
                }
                else if (currentHealth <= ctrlData.maxHealth / 4 && currentPhase != 2)
                {
                    StateMachine.ChangeState(_secondPhase);
                    currentPhase++;
                }
                else
                {
                    int randomStun = Random.Range(0, 10);
                    if (randomStun >= 7)
                    {
                        StateMachine.ChangeState(_stunnedState);
                    }
                }

                damagedTimeCD = Time.time + .15f;
            }
        }

        protected override void Update()
        {
            base.Update();
        }
    }
}