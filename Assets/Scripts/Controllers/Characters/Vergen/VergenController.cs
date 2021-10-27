using System.Collections;
using Controllers.Characters.Vergen.States;
using UnityEngine;

namespace Controllers.Characters.Vergen
{
    public class VergenController : BaseController
    {
        private bool _CombatEnabled;
        
        [SerializeField] private GameObject _SpearObject;
        [SerializeField] private Transform _SpearPosition;

        public VergenIdleState vergenIdleState { get; private set; }
        public VergenPhaseOneState vergenPhaseOneState { get; private set; }
        public VergenNormalAttackState vergenNormalAttackState { get; private set; }
        public VergenLongRangeAttack vergenLongAttackState { get; private set; }
        
        public 
        
        protected override void Start()
        {
            base.Start();
            
            vergenIdleState = new VergenIdleState(this, StateMachine, "Idle", this);
            vergenPhaseOneState = new VergenPhaseOneState(this, StateMachine, "Run", this);
            vergenNormalAttackState = new VergenNormalAttackState(this, StateMachine, "NormalAttack", this);
            vergenLongAttackState = new VergenLongRangeAttack(this, StateMachine, "LongRangeAttack", this);
            
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
            
            GameObject proj = Instantiate(_SpearObject, _SpearPosition.position, rot, transform);
            VergenSpear vSpear = proj.GetComponent<VergenSpear>();
            
            vSpear.FireProjectile(30f, 30f, false);
        }
    }
}