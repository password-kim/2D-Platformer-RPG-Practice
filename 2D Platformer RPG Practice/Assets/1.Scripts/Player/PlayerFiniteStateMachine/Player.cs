using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region State Variables
    public PlayerStateMachine StateMahine { get; private set; }

    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerJumpState JumpState { get; private set; }
    public PlayerInAirState InAirState { get; private set; }
    public PlayerLandState LandState { get; private set; }
    public PlayerWallSlideState WallSlideState { get; private set; }
    public PlayerWallGrabState WallGrabState { get; private set; }
    public PlayerWallClimbState WallClimbState { get; private set; }

    [SerializeField]
    private PlayerData _playerData;
    #endregion

    #region Components
    public Animator Anim { get; private set; }
    public PlayerInputHandler InputHandler { get; private set; }
    public Rigidbody2D RB { get; private set; }

    #endregion

    #region Check Transforms
    [SerializeField]
    private Transform _groundCheck;

    [SerializeField]
    private Transform _wallCheck;

    #endregion

    #region Other Variables
    public Vector2 CurrentVelocity { get; private set; }
    public int FacingDirection { get; private set; }

    private Vector2 _workspace;

    #endregion

    #region Unity Callback Functions
    private void Awake()
    {
        StateMahine = new PlayerStateMachine();

        IdleState = new PlayerIdleState(this, StateMahine, _playerData, "idle");
        MoveState = new PlayerMoveState(this, StateMahine, _playerData, "move");
        JumpState = new PlayerJumpState(this, StateMahine, _playerData, "inAir");
        InAirState = new PlayerInAirState(this, StateMahine, _playerData, "inAir");
        LandState = new PlayerLandState(this, StateMahine, _playerData, "land");
        WallSlideState = new PlayerWallSlideState(this, StateMahine, _playerData, "wallSlide");
        WallGrabState = new PlayerWallGrabState(this, StateMahine, _playerData, "wallGrab");
        WallClimbState = new PlayerWallClimbState(this, StateMahine, _playerData, "wallClimb");

        Anim = GetComponent<Animator>();
        InputHandler = GetComponent<PlayerInputHandler>();
        RB = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        StateMahine.Initialize(IdleState);

        FacingDirection = 1;
    }

    private void Update()
    {
        CurrentVelocity = RB.velocity;
        StateMahine.CurrentState.LogicUpdate();
    }

    private void FixedUpdate()
    {
        StateMahine.CurrentState.PhysicsUpdate();
    }

    #endregion

    #region Set Functions

    public void SetVelocityX(float velocity)
    {
        _workspace.Set(velocity, CurrentVelocity.y);
        RB.velocity = _workspace;
        CurrentVelocity = _workspace;
    }

    public void SetVelocityY(float velocity)
    {
        _workspace.Set(CurrentVelocity.x, velocity);
        RB.velocity = _workspace;
        CurrentVelocity = _workspace;
    }

    #endregion

    #region Check Functions

    public bool CheckIfGrounded()
    {
        return Physics2D.OverlapCircle(_groundCheck.position, _playerData.GroundCheckRadius, _playerData.WhatIsGround);
    }

    public bool CheckIfTouchingWall()
    {
        return Physics2D.Raycast(_wallCheck.position, Vector2.right * FacingDirection, _playerData.WallCheckDistance, _playerData.WhatIsGround);
    }

    public void CheckIfshouldFlip(int xInput)
    {
        if(xInput != 0 && xInput != FacingDirection)
        {
            Flip();
        }
    }

    #endregion

    #region Other Functions

    private void AnimationTrigger() => StateMahine.CurrentState.AnimationTrigger();

    private void AnimationFinishTrigger() => StateMahine.CurrentState.AnimationFinishTrigger();

    private void Flip()
    {
        FacingDirection *= -1;
        transform.Rotate(0.0f, 180.0f, 0.0f);
    }
    #endregion
}
