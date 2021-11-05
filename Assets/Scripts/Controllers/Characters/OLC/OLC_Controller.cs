using System;
using System.Collections;
using System.Collections.Generic;
using Controllers;
using Controllers.Characters.OLC.States;
using Controllers.Damage;
using Controllers.Froggy.States.Data;
using Controllers.StateMachine.States;
using Controllers.StateMachine.States.Data;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace Controllers.Froggy
{
    public class OLC_Controller : BaseController
    {
        [Header("OLC Specific Values")] [SerializeField]
        private AttackStateData _attackStateData;

        [SerializeField] private IdleStateData _idleStateData;
        [SerializeField] private OLC_AttackStateData _olcAttackStateData;
        [SerializeField] private GameObject _miniOLC;
        [SerializeField] private PepeController _pepeController;

        private FroggyTongueController instatiatedTongue;

        public OLC_IdleState _idleState { get; private set; }
        public OLC_RenegadeState _renegadeState { get; private set; }
        public OLC_AttackState _attackState { get; private set; }
        public OLC_StunnedState _stunnedState { get; private set; }
        public OLC_SecondPhase _secondPhase { get; private set; }
        public OLC_DeadState _deadState { get; private set; }

        public Transform projectilePos;
        public Transform secondPhasePos;
        public Transform[] arrowsPos;

        [SerializeField] private Sprite visiblePepeQC;
        
        public int currentPhase = 1;
        public Vector3 initialPos;

        public bool OLCisActive;

        protected override void Start()
        {
            if (GameManager.Instance.GetSceneIndex() == 0)
            {
                ctrlData.maxHealth = 100;
            }else if (GameManager.Instance.GetSceneIndex() == 1)
            {
                ctrlData.maxHealth = 150;
            }
            
            base.Start();

            _idleState = new OLC_IdleState(this, StateMachine, "Idle", _idleStateData, this);
            _renegadeState = new OLC_RenegadeState(this, StateMachine, "Renegade", this);
            _attackState = new OLC_AttackState(this, StateMachine, "Attacking", _attackStateData, _olcAttackStateData,
                this);
            _stunnedState = new OLC_StunnedState(this, StateMachine, "Stunned", _idleStateData, this);
            _secondPhase = new OLC_SecondPhase(this, StateMachine, "Jumping", _attackStateData, _olcAttackStateData,
                this);
            _deadState = new OLC_DeadState(this, StateMachine, "DEAD", this);
            StateMachine.Initialize(_idleState);

            initialPos = transform.position;
            GameManager.Instance.OnLevelReset.AddListener(ResetOLC);


            OnLifeTimeEnded.AddListener(() =>
            {
                if (GameManager.Instance.GetSceneIndex() == 0)
                {
                    _pepeController.ShowQuickChat(new Tuple<Sprite, int>(_pepeController.OLCFlyQC[0], 2));
                    _pepeController.ShowQuickChat(new Tuple<Sprite, int>(_pepeController.OLCFlyQC[1], 3));
                }
            });

        }

        public override void Damage(DamageInfo damageInfo)
        {
            if (dead ||
                controllerKind == EControllerKind.NPC ||
                controllerKind == EControllerKind.Neutral)
                return;

            if (Time.time > damagedTimeCD)
            {
                Debug.Log("DamageReceived (" + damageInfo.DamageAmount + ")");
                StopAllCoroutines();


                // Obtenemos la posicion del ataque
                int direction = damageInfo.GetAttackDirection(transform.position.x);

                playerFacingDirection = GameManager.Instance.PlayerController.IsFacingRight();

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
                    GameManager.Instance.PlayerController.RefillHealth();

                    GivePlayerExp();


                    if (GameManager.Instance.GetSceneIndex() == 0)
                    {
                        isInvencible = true;
                        StateMachine.ChangeState(_deadState);
                        //StartCoroutine(LevelWonDelayed());
                        GameManager.Instance.StartCoroutine(GameManager.Instance.ShowTransitionOne());
                    }
                    else if (GameManager.Instance.GetSceneIndex() == 1)
                    {
                        Instantiate(_miniOLC, transform.position, Quaternion.identity);
                        GameManager.Instance.StartCoroutine(GameManager.Instance.ShowTransitionTwo());

                        OnLifeTimeEnded.AddListener(() =>
                        {
                            _pepeController.ShowQuickChat(new Tuple<Sprite, int>(_pepeController.OLCFlyQC[2], 2));
                            _pepeController.ShowQuickChat(new Tuple<Sprite, int>(_pepeController.OLCFlyQC[3], 2));
                            _pepeController.ShowQuickChat(new Tuple<Sprite, int>(_pepeController.OLCFlyQC[4], 3));
                            _pepeController.ShowQuickChat(new Tuple<Sprite, int>(_pepeController.OLCFlyQC[5], 3));
                        });

                        OnLifeTimeEnded.Invoke();
                        Destroy(gameObject);
                    }
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

        //private IEnumerator LevelWonDelayed()
        //{
        //    yield return new WaitForSeconds(10);
        //    yield return 0;
        //}

        private void ResetOLC()
        {
            Debug.Log("RESET OLC");
            StateMachine.ChangeState(_idleState);
            currentHealth = ctrlData.maxHealth;
            transform.position = initialPos;
            GameManager.Instance.HideBossHealth();
            //enabled = false;
            OLCisActive = false;
        }

        protected override void Update()
        {
            if (OLCisActive)
                base.Update();
        }

        protected override void FixedUpdate()
        {
            if (OLCisActive)
                base.FixedUpdate();
        }

        public void StartCombat()
        {
            GameManager.Instance.ShowBossHealth("Odinn's Lost Crow - Fase 1", this);
            OLCisActive = true;
        }
        
        protected override void OnBecameVisible()
        {
            IsCharacterOnScreen = true;
            GameManager.Instance.EnemiesInScreen
                .Add(this);
            
            if (!GameManager.Instance.HasSeenOLC &&
                GameManager.Instance.GetSceneIndex() == 0)
            {
                GameManager.Instance.GetPepeController()
                    .ShowQuickChat(new Tuple<Sprite, int>(visiblePepeQC, 2));
                GameManager.Instance.HasSeenOLC = true;
            }
        }

        protected override void OnBecameInvisible()
        {
            IsCharacterOnScreen = true;
            GameManager.Instance.EnemiesInScreen
                .Remove(this);
        }
    }
}