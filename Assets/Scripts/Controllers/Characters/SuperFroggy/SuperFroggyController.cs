using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Controllers.Characters.SuperFroggy.States;
using Controllers;
using Controllers.Characters.SuperFroggy.States;
using Controllers.Froggy;
using Controllers.StateMachine.States.Data;
using UnityEngine;

public class SuperFroggyController : BaseController
{
    public SuperFroggyIdleState _idleState { get; private set; }
    public SuperFroggyJumpState _jumpState { get; private set; }
    public SuperFroggyNearAttackState _nearAttackState { get; private set; }
    
    [SerializeField] private JumpStateData _jumpStateData;

    // Super Froggy
    public bool transforming;
    public int currentPhase = 1;
    public bool transformed;
    [SerializeField] private SuperFroggy_SecondPhaseStateData _superFroggySecondPhaseStateData;

    public SuperFroggy_SecondPhase _superFroggySecondPhase { get; private set; }
    public int hallPosition = 0;

    [SerializeField] private GameObject _farty;
    [SerializeField] private FroggyTongueController tongueControllerongue;
    public GameObject _superFroggyNuke;

    protected override void Start()
    {
        _jumpState = new SuperFroggyJumpState(this, StateMachine, "Jumping", _jumpStateData, this);
        _idleState = new SuperFroggyIdleState(this, StateMachine, "Idle", this);
        _nearAttackState = new SuperFroggyNearAttackState(this, StateMachine, "Idle", _jumpStateData, this);
        
        base.Start();
        
        StateMachine.Initialize(_idleState);
    }

    // SuperFroggy : MiniBoss => SecondPhase
    public void EnterPhaseTwo()
    {
        if (controllerKind == EControllerKind.Boss && currentPhase == 1)
        {
            transforming = true;
            currentPhase++;
            StateMachine.ChangeState(_superFroggySecondPhase);
        }
    }

    public void Explode()
    {
        Instantiate(_farty, transform.position, Quaternion.Euler(0f, 0f, 0f));
        Destroy(gameObject);
    }

    public void PlaceBomb()
    {
        Instantiate(_superFroggySecondPhaseStateData.bombs, transform.position + new Vector3(0f, 2f), Quaternion.Euler(0f, 0f, 0f));
    }
}
