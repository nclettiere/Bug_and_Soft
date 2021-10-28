using System;
using System.Collections;
using Controllers.Characters.Vergen.States;
using Controllers.Damage;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Controllers.Characters.Vergen
{
    public class VergenController : BaseController
    {
        private bool _CombatEnabled;
        
        [SerializeField] private GameObject _SpearObject;
        [SerializeField] private Transform _SpearPosition;
        [SerializeField] private LayerMask _WhatIsSpikes;
        
        [SerializeField] private GameObject _vergenGhost;
        [SerializeField] private VergenGhostTrigger[] _vergenGhostTriggers;

        public VergenIdleState vergenIdleState { get; private set; }
        public VergenPhaseOneState vergenPhaseOneState { get; private set; }
        public VergenNormalAttackState vergenNormalAttackState { get; private set; }
        public VergenLongRangeAttack vergenLongAttackState { get; private set; }
        
        public VergenTorbeshinoState vergenTorbeshinoState { get; private set; }
        
        public bool HasSpearAttackFinished { get; private set; }

        private bool spearShot;
        
        protected override void Start()
        {
            base.Start();
            
            vergenIdleState = new VergenIdleState(this, StateMachine, "Idle", this);
            vergenPhaseOneState = new VergenPhaseOneState(this, StateMachine, "Run", this);
            vergenNormalAttackState = new VergenNormalAttackState(this, StateMachine, "NormalAttack", this);
            vergenLongAttackState = new VergenLongRangeAttack(this, StateMachine, "LongRangeAttack", this);
            vergenTorbeshinoState = new VergenTorbeshinoState(this, StateMachine, "Torbeshino", this);
            
            vergenLongAttackState.OnVergenShootingSpears.AddListener(ShootSpears);
            
            StateMachine.Initialize(vergenIdleState);
        }

        protected override void Update()
        {
            if (_CombatEnabled)
            {
                base.Update();
            }
        }

        protected override void FixedUpdate()
        {
            if (_CombatEnabled)
            {
                base.FixedUpdate();
            }
        }

        public void StartCombat()
        {
            _CombatEnabled = true;
            StateMachine.ChangeState(vergenPhaseOneState);
            
            GameManager.Instance.ShowBossHealth("VERGEN - FASE 1", this);
        }
        
        public void OnAttackFinish()
        {
            vergenNormalAttackState.OnAttackFinish();
        }

        public void PhaseOneAttackWait()
        {
            StartCoroutine(AttackWaitForExit());
        }
        
        private IEnumerator AttackWaitForExit()
        {
            yield return new WaitForSeconds(Random.Range(0f, 1f));
            StateMachine.ChangeState(vergenPhaseOneState);
        }
        
        public void SpawnSpear()
        {
            Quaternion rot;

            if (FacingDirection == 1)
                rot = Quaternion.Euler(0, 0, 0);
            else
                rot = Quaternion.Euler(0, 180, 0);
            
            GameObject proj = Instantiate(_SpearObject, _SpearPosition.position, rot);
            VergenSpear vSpear = proj.GetComponent<VergenSpear>();
            
            vSpear.FireProjectile(30f, 30f, false);

            vergenLongAttackState.OnSpearShot();
        }

        public void ShootSpears()
        {
            Debug.Log("Shooting "+ vergenLongAttackState.NumberOfSpears +" spears!");
            //StartCoroutine(StartShooting());
            HasSpearAttackFinished = false;
            GetAnimator().SetBool("LongRangeAttack", true);
        }

        private IEnumerator StartShooting()
        {
            for (int i = 0; i < vergenLongAttackState.NumberOfSpears; i++)
            {
                spearShot = false;
                Debug.Log($"Number of spears is {vergenLongAttackState.NumberOfSpears}");
                GetAnimator().SetBool("LongRangeAttack", true);
                yield return new WaitUntil(() => spearShot);
                GetAnimator().SetBool("LongRangeAttack", false);
            }
            
            Debug.Log("Shooting finished!");

            vergenLongAttackState.OnAttackFinish();
            
            yield return null;
        }

        private void Anim_OnSpearShotFinished()
        {
            HasSpearAttackFinished = true;
            GetAnimator().SetBool("LongRangeAttack", false);
            spearShot = true;
        }

        public bool CheckForSpikes()
        {
            var capsule = GetComponent<CapsuleCollider2D>();
            RaycastHit2D hit = Physics2D.CircleCast(transform.position, 2, Vector2.zero, 0, _WhatIsSpikes);
            return hit.collider != null && hit.collider.CompareTag("Spikes");
        }

        public override void Damage(DamageInfo damageInfo)
        {
            
            if (Time.time > damagedTimeCD)
            {
                _vergenGhostTriggers[Random.Range(0, 2)].Run();
                
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
                    GivePlayerExp();
                    OnLifeTimeEnded.Invoke();
                    Die();
                }

                damagedTimeCD = Time.time + .15f;
            }
        }
    }
}