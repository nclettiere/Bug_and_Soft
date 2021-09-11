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
        //[SerializeField] private AttackStateData _attackStateData;
        //[SerializeField] private DeadStateData _deadStateData;

        //private NavMeshAgent _agent;

        public Zanopiano_WalkState WalkState { get; private set; }
        public Zanopiano_AttackState AttackState { get; private set; }

        protected override void Start()
        {
            base.Start();

            //_agent = GetComponent<NavMeshAgent>();
            //_agent.updateRotation = false;
            //_agent.updateUpAxis = false;

            WalkState = new Zanopiano_WalkState(this, StateMachine, "Walk", this);
            AttackState = new Zanopiano_AttackState(this, StateMachine, "Attacking", this);

            StateMachine.Initialize(WalkState);
        }

        public void SetTargetDestination(Vector3 target)
        {
            //_agent.SetDestination(target);
        }

        protected override void OnDrawGizmos()
        {
            base.OnDrawGizmos();

            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(transform.position, 1f);
        }

        public void Anim_ZanopianoAlertingCompleted()
        {
            GetAnimator().SetBool("Alerting", false);
            GetAnimator().SetBool("Alerting_Completed", true);
        }
        
        public void Anim_ZanopianoAttackingCompleted()
        {
            GetAnimator().SetBool("Attacking", false);
            StateMachine.ChangeState(WalkState);
        }
    }
}