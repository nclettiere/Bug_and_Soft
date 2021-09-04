using System.Collections;
using System.Collections.Generic;
using Controllers;
using Controllers.Characters.Mortadelo.States;
using Controllers.StateMachine.States.Data;
using UnityEngine;
using UnityEngine.AI;
using Controllers.Characters.Mortadelo.States;

public class MortadeloController : BaseController
{
    [SerializeField]
    private AttackStateData _attackStateData;
    [SerializeField]
    private DeadStateData _deadStateData;

    private NavMeshAgent _agent;
    
    public Mortadelo_IdleState IdleState { get; private set; }
    public Mortadelo_AttackState AttackState { get; private set; }
    public Mortadelo_DeadState DeadState { get; private set; }
    
    protected override void Start()
    {
        base.Start();
        
        _agent = GetComponent<NavMeshAgent>();
        _agent.updateRotation = false;
        _agent.updateUpAxis = false;

        IdleState = new Mortadelo_IdleState(this, StateMachine, "Idle", this);
        AttackState = new Mortadelo_AttackState(this, StateMachine, "Dash", _attackStateData, this);
        DeadState = new Mortadelo_DeadState(this, StateMachine, "Dead", _deadStateData, this);
            
        StateMachine.Initialize(IdleState);
    }

    public void SetTargetDestination(Vector3 target)
    {
        _agent.SetDestination(target);
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, 1f);
    }
}
