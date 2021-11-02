using System.Collections;
using System.Collections.Generic;
using Controllers;
using Controllers.Characters.SuperFroggy.States;
using Controllers.StateMachine.States.Data;
using UnityEngine;

public class SuperFroggyController : BaseController
{
    public SuperFroggyIdleState _idleState { get; private set; }
    public SuperFroggyJumpState _jumpState { get; private set; }
    
    [SerializeField] private JumpStateData _jumpStateData;

    protected override void Start()
    {
        _jumpState = new SuperFroggyJumpState(this, StateMachine, "Jumping", _jumpStateData, this);
        _idleState = new SuperFroggyIdleState(this, StateMachine, "Idle", this);
        
        base.Start();
        
        StateMachine.Initialize(_idleState);
    }
}
