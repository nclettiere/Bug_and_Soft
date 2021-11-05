using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Controllers.Characters.SuperFroggy.States;
using Controllers;
using Controllers.Characters.SuperFroggy.States;
using Controllers.Froggy;
using Controllers.StateMachine.States.Data;
using Items;
using UnityEngine;

public class SuperFroggyController : BaseController
{
    public SuperFroggyIdleState _idleState { get; private set; }
    public SuperFroggyJumpState _jumpState { get; private set; }
    public SuperFroggyNearAttackState _nearAttackState { get; private set; }
    public SuperFroggyLongAttackState _longAttackState { get; private set; }
    public SuperFroggyDeadState _deadState { get; private set; }

    [SerializeField] private JumpStateData _jumpStateData;

    private float bombPlacementTime = float.NegativeInfinity;
    public bool transforming;
    public int currentPhase = 1;
    public bool transformed;
    [SerializeField] private SuperFroggy_SecondPhaseStateData _superFroggySecondPhaseStateData;

    public SuperFroggy_SecondPhase _superFroggySecondPhase { get; private set; }
    public int hallPosition = 0;

    [SerializeField] private GameObject _farty;
    [SerializeField] private GameObject _bombs;
    [SerializeField] private FroggyTongueController tongueControllerongue;
    public GameObject _superFroggyNuke;

    public Sprite PepeQC;
    
    private bool shownPepeQC;

    protected override void Start()
    {
        _jumpState = new SuperFroggyJumpState(this, StateMachine, "Jumping", _jumpStateData, this);
        _idleState = new SuperFroggyIdleState(this, StateMachine, "Idle", this);
        _nearAttackState = new SuperFroggyNearAttackState(this, StateMachine, "Jumping", _jumpStateData, this);
        _longAttackState = new SuperFroggyLongAttackState(this, StateMachine, "Idle", this);
        _deadState = new SuperFroggyDeadState(this, StateMachine, "Idle", this);
        IsCharacterOnScreen = true;
        
        base.Start();
        
        StateMachine.Initialize(_idleState);
    }

    //protected override void Update()
    //{
    //    if (GameManager.Instance.IsGamePaused())
    //    {
    //        if (!savedRigidData)
    //        {
    //            lastVelocity = rBody.velocity;
    //            lastAngularVelocity = rBody.angularVelocity;
    //            rBody.bodyType = RigidbodyType2D.Static;
    //            rBody.Sleep();
//
    //            savedRigidData = true;
    //        }
//
    //        return;
    //    }
//
    //    if (savedRigidData)
    //    {
    //        rBody.bodyType = RigidbodyType2D.Dynamic;
    //        rBody.velocity = lastVelocity;
    //        rBody.angularVelocity = lastAngularVelocity;
    //        rBody.WakeUp();
    //        savedRigidData = false;
    //    }
//
    //    if (!dead)
    //    {
    //        CheckPlayerInNearRange();
    //        CheckPlayerInLongRange();
    //    }
//
    //    StateMachine.CurrentState.UpdateState();
//
    //    if (!cachedGroundCheck && CheckGround())
    //        OnLandEvent.Invoke();
//
    //    cachedGroundCheck = CheckGround();
//
    //    if (!cachedBodyGroundCheck && CheckOnBodyTouchGround())
    //        OnBodyGroundCheckEvent.Invoke();
//
    //    cachedBodyGroundCheck = CheckGround();
    //}
//
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (IsCharacterOnScreen)
        {
            if (GameManager.Instance.IsGamePaused() || dead) return;

            StateMachine.CurrentState.UpdatePhysics();

            if (currentHealth <= ctrlData.maxHealth / 2.5f)
            {
                if (Time.time >= bombPlacementTime)
                {
                    PlaceBomb();
                    bombPlacementTime = Time.time + 1.5f;
                }
            }
        }
    }
    
    public void SpecialAttack()
    {
        tongueControllerongue.gameObject.SetActive(true);
        tongueControllerongue.SetProps(this);
        tongueControllerongue.Activate();
    }
    
    public void OnTongueFinish()
    {
        _longAttackState.OnTongeFinished();
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
        Instantiate(_bombs, transform.position + new Vector3(0f, 2f),
            Quaternion.Euler(0f, 0f, 0f));
    }
    
    public override void Die()
    {
        dead = true;
        StateMachine.ChangeState(_deadState);
        GameManager.Instance.HideBossHealth();
        
        //_froggyController.DropItems();

        if (GetComponent<ItemGiver>() != null)
        {
            GetComponent<ItemGiver>().Run();
        }
    }
    
    public override IEnumerator OnDie()
    {
        Debug.Log("OnDie called");
        GameManager.Instance.AddPlayerKrowns(KrownReward);
            
        AddForce(new Vector2(20f * -FacingDirection, 10f), true);
        yield return new WaitForSeconds(1);
            
        Destroy(gameObject);
    }
    
    public void Anim_OnAttackingAnimStarted()
    {
    }

    protected override void OnBecameVisible()
    {
        base.OnBecameVisible();

        if (!shownPepeQC)
        {
            GameManager.Instance.GetPepeController()
                .ShowQuickChat(new Tuple<Sprite, int>(PepeQC, 1));
            shownPepeQC = true;
        }
    }
}