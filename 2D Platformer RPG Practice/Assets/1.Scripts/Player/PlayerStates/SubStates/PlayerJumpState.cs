using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerAbilityState
{
    private int _amountOfJumpsLeft;

    public PlayerJumpState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
        _amountOfJumpsLeft = playerData.AmountOfJumps;
    }

    public override void Enter()
    {
        base.Enter();
        player.SetVelocityY(playerData.JumpVelocity);
        isAbilityDone = true;
        --_amountOfJumpsLeft;
        player.InAirState.SetIsJumping();
    }

    public bool CanJump()
    {
        if(_amountOfJumpsLeft > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void ResetAmountOfJumpsLeft() => _amountOfJumpsLeft = playerData.AmountOfJumps;

    public void DecreaseAmountOfJumpsLeft() => --_amountOfJumpsLeft;
}
