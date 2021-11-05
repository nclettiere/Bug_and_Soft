using System;
using System.Collections;
using System.Collections.Generic;
using Controllers;
using Controllers.Characters.Mortadelo.States;
using Controllers.StateMachine.States.Data;
using UnityEngine;
using UnityEngine.AI;
using Controllers.Characters.Mortadelo.States;
using Pathfinding;

public class MortadeloController : BaseController
{
    [SerializeField]
    private AttackStateData _attackStateData;
    [SerializeField]
    private DeadStateData _deadStateData;
    [SerializeField]
    private GameObject _explosion;

    private AIDestinationSetter _destination;
    
    public Mortadelo_IdleState IdleState { get; private set; }
    public Mortadelo_AttackState AttackState { get; private set; }
    public Mortadelo_DeadState DeadState { get; private set; }
    
    [SerializeField] private Sprite deathQC;
    
    protected override void Start()
    {
        base.Start();
        
        _destination = GetComponent<AIDestinationSetter>();

        IdleState = new Mortadelo_IdleState(this, StateMachine, "Idle", this);
        AttackState = new Mortadelo_AttackState(this, StateMachine, "Dash", _attackStateData, this);
        DeadState = new Mortadelo_DeadState(this, StateMachine, "Dead", _deadStateData, this);
            
        StateMachine.Initialize(IdleState);
    }

    private void Update()
    {
        if (currentHealth <= 0)
        {
            StateMachine.ChangeState(DeadState);

            if (!GameManager.Instance.HasKilledMortadelo)
            {
                GameManager.Instance.GetPepeController()
                    .ShowQuickChat(new Tuple<Sprite, int>(deathQC, 1));
                GameManager.Instance.HasKilledMortadelo = true;
            }
        }
        base.Update();
    }

    public void SetTargetDestination()
    {
        _destination.target = GameManager.Instance.PlayerController.transform;
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, 1f);
    }

    public void Suicidar()
    {
        Instantiate(_explosion, transform.position, Quaternion.identity);
    }
}
