using UnityEngine;

public class PlayerLedgeClimbState : PlayerState
{
    private Vector2 _detectedPos;
    private Vector2 _cornerPos;

    private Vector2 _startPos;
    private Vector2 _stopPos;

    private bool _isHanging;
    private bool _isClimbing;
    private bool _isTouchingCeiling;

    private bool _jumpInput;

    private int _xInput;
    private int _yInput;

    public PlayerLedgeClimbState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();

        player.Anim.SetBool("ledgeClimb", false);
    }

    public override void AnimationTrigger()
    {
        base.AnimationTrigger();

        _isHanging = true;
    }

    public override void Enter()
    {
        base.Enter();

        player.SetVelocityZero();
        player.transform.position = _detectedPos;
        _cornerPos = player.DetermineCornerPosition();

        _startPos.Set(_cornerPos.x - (player.FacingDirection * playerData.StartOffset.x), _cornerPos.y - playerData.StartOffset.y);
        _stopPos.Set(_cornerPos.x + (player.FacingDirection * playerData.StopOffset.x), _cornerPos.y + playerData.StopOffset.y);

        player.transform.position = _startPos;
    }

    public override void Exit()
    {
        base.Exit();

        _isHanging = false;

        if(_isClimbing)
        {
            player.transform.position = _stopPos;
            _isClimbing = false;
        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(isAnimationFinished)
        {
            if(_isTouchingCeiling)
            {
                stateMachine.ChangeState(player.CrouchIdleState);
            }
            else
            {
                stateMachine.ChangeState(player.IdleState);
            }
        }
        else
        {
            _xInput = player.InputHandler.NormInputX;
            _yInput = player.InputHandler.NormInputY;
            _jumpInput = player.InputHandler.JumpInput;

            player.SetVelocityZero();
            player.transform.position = _startPos;

            if (_xInput == player.FacingDirection && _isHanging && false == _isClimbing)
            {
                CheckForSpace();
                _isClimbing = true;
                player.Anim.SetBool("ledgeClimb", true);
            }
            else if (_yInput == -1 && _isHanging && false == _isClimbing)
            {
                stateMachine.ChangeState(player.InAirState);
            }
            else if(_jumpInput && false == _isClimbing)
            {
                player.WallJumpState.DetermineWallJumpDirection(true);
                stateMachine.ChangeState(player.WallJumpState);
            }
        }
    }

    public void SetDetectedPosition(Vector2 pos) => _detectedPos = pos;

    private void CheckForSpace()
    {
        _isTouchingCeiling = Physics2D.Raycast(
            _cornerPos + (Vector2.up * 0.015f) + (Vector2.right * player.FacingDirection * 0.015f),
            Vector2.up, 
            playerData.StandColliderHeight, 
            playerData.WhatIsGround);
        player.Anim.SetBool("isTouchingCeiling", _isTouchingCeiling);
    }
}
