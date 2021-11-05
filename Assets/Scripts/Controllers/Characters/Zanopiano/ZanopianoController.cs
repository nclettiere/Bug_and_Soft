using System.Collections;
using System.Collections.Generic;
using Controllers;
using Controllers.Characters.Mortadelo.States;
using Controllers.StateMachine.States.Data;
using UnityEngine;
using UnityEngine.AI;
using Controllers.Characters.Mortadelo.States;
using Controllers.Characters.Zanopiano.States;

namespace Controllers.Characters.Zanopiano
{
    public class ZanopianoController : BaseController
    {
        private static readonly int AlertingCompleted = Animator.StringToHash("Alerting_Completed");
        private static readonly int Alerting = Animator.StringToHash("Alerting");
        
        public Transform attackPosition;
        public float attackRadius = 1f;
        
        public Sprite PepeQC;
        
        //[SerializeField] private DeadStateData _deadStateData;

        //private NavMeshAgent _agent;

        public Zanopiano_WalkState WalkState { get; private set; }
        public Zanopiano_AttackState AttackState { get; private set; }
        public Zanopiano_DeadState DeadState { get; private set; }

        protected override void Start()
        {
            base.Start();

            WalkState = new Zanopiano_WalkState(this, StateMachine, "Walk", this);
            AttackState = new Zanopiano_AttackState(this, StateMachine, "Attacking", this);
            DeadState = new Zanopiano_DeadState(this, StateMachine, "Dead", this);

            StateMachine.Initialize(WalkState);
        }

        protected override void Update()
        {
            if (currentHealth <= 0)
            {
                StateMachine.ChangeState(DeadState);
            }
            
            base.Update();
        }

        public void SetTargetDestination(Vector3 target)
        {
            //_agent.SetDestination(target);
        }

        protected override void OnDrawGizmos()
        {
            base.OnDrawGizmos();

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPosition.position, attackRadius);
        }

        public void Anim_ZanopianoAlertingCompleted()
        {
            WalkState.AlertingFinished();
        }
        
        public void Anim_ZanopianoAttackingCompleted()
        {
            AttackState.AttackedFinished();
        }
        
        public void Anim_ZanopianoAttackPunalada()
        {
            AttackState.AttackNow();
        }
    }
}