using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInAirState : PlayerState
{
    // Inputs
    private int _xInput;
    private bool _jumpInput;
    private bool _jumpInputStop;
    private bool _grabInput;
    private bool _dashInput;

    // Checks
    private bool _isGrounded;
    private bool _isTouchingWall;
    private bool _isTouchingWallBack;
    private bool _oldIsTouchingWall;
    private bool _oldIsTouchingWallBack;
    private bool _isTouchingLedge;
    private bool _isJumping;
    
    // Times
    private bool _coyoteTime;
    private bool _wallJumpCoyoteTIme;
    private float _startWallJumpCoyoteTime;

    public PlayerInAirState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();

        _oldIsTouchingWall = _isTouchingWall;
        _oldIsTouchingWallBack = _isTouchingWallBack;

        _isGrounded = player.CheckIfGrounded();
        _isTouchingWall = player.CheckIfTouchingWall();
        _isTouchingWallBack = player.CheckIfTouchingWallBack();
        _isTouchingLedge = player.CheckIfTouchingLedge();

        if(_isTouchingWall && false == _isTouchingLedge)
        {
            player.LedgeClimbState.SetDetectedPosition(player.transform.position);
        }

        if(false == _wallJumpCoyoteTIme && false == _isTouchingWall && false == _isTouchingWallBack && (_oldIsTouchingWall || _oldIsTouchingWallBack))
        {
            StartWallJumpCoyoteTime();
        }
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();

        _oldIsTouchingWall = false;
        _oldIsTouchingWallBack = false;
        _isTouchingWall = false;
        _isTouchingWallBack = false;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        CheckCoyoteTime();
        CheckWallJumpCoyoteTime();

        _xInput = player.InputHandler.NormInputX;
        _jumpInput = player.InputHandler.JumpInput;
        _jumpInputStop = player.InputHandler.JumpInputStop;
        _grabInput = player.InputHandler.GrabInput;
        _dashInput = player.InputHandler.DashInput;

        CheckJumpMultiplier();

        if (_isGrounded && player.CurrentVelocity.y < 0.01f)
        {
            stateMachine.ChangeState(player.LandState);
        }
        else if(_isTouchingWall && false == _isTouchingLedge && false == _isGrounded)
        {
            stateMachine.ChangeState(player.LedgeClimbState);
        }
        else if(_jumpInput && (_isTouchingWall || _isTouchingWallBack || _wallJumpCoyoteTIme))
        {
            StopWallJumpCoyoteTime();
            _isTouchingWall = player.CheckIfTouchingWall();
            player.WallJumpState.DetermineWallJumpDirection(_isTouchingWall);
            stateMachine.ChangeState(player.WallJumpState);
        }
        else if (_jumpInput && player.JumpState.CanJump())
        {
            stateMachine.ChangeState(player.JumpState);
        }
        else if(_isTouchingWall && _grabInput && _isTouchingLedge)
        {
            stateMachine.ChangeState(player.WallGrabState);
        }
        else if (_isTouchingWall && _xInput == player.FacingDirection && player.CurrentVelocity.y <= 0)
        {
            stateMachine.ChangeState(player.WallSlideState); 
        }
        else if(_dashInput && player.DashState.CheckIfCanDash())
        {
            stateMachine.ChangeState(player.DashState);
        }
        else
        {
            player.CheckIfshouldFlip(_xInput);
            player.SetVelocityX(playerData.MovementVelocity * _xInput);

            player.Anim.SetFloat("yVelocity", player.CurrentVelocity.y);
            player.Anim.SetFloat("xVelocity", Mathf.Abs(player.CurrentVelocity.x));
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    private void CheckJumpMultiplier()
    {
        if (_isJumping)
        {
            if (_jumpInputStop)
            {
                player.SetVelocityY(player.CurrentVelocity.y * playerData.VariableJumpHeightMultiplier);
                _isJumping = false;
            }
            else if (player.CurrentVelocity.y <= 0f)
            {
                _isJumping = false;
            }
        }
    }

    private void CheckCoyoteTime()
    {
        if(_coyoteTime && Time.time > startTime + playerData.CoyoteTime)
        {
            _coyoteTime = false;
            player.JumpState.DecreaseAmountOfJumpsLeft();
        }
    }

    private void CheckWallJumpCoyoteTime()
    {
        if(_wallJumpCoyoteTIme && Time.time > _startWallJumpCoyoteTime + playerData.CoyoteTime)
        {
            _wallJumpCoyoteTIme = false;
        }
    }

    public void StartCoyoteTime() => _coyoteTime = true;

    public void StartWallJumpCoyoteTime()
    {
        _wallJumpCoyoteTIme = true;
        _startWallJumpCoyoteTime = Time.time;
    }

    public void StopWallJumpCoyoteTime() => _wallJumpCoyoteTIme = false;

    public void SetIsJumping() => _isJumping = true;
}
