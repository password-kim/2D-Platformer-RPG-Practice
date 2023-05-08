using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCrouchMoveState : PlayerGroundedState
{
    public PlayerCrouchMoveState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.SetColliderHeight(playerData.CrouchColliderHeight);
    }

    public override void Exit()
    {
        base.Exit();
        player.SetColliderHeight(playerData.StandColliderHeight);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (false == isExitingState)
        {
            player.SetVelocityX(playerData.CrouchMovementVelocity * player.FacingDirection);
            player.CheckIfshouldFlip(xInput);

            if(xInput == 0)
            {
                stateMachine.ChangeState(player.CrouchIdleState);
            }
            else if(yInput != -1 && false == isTouchingCeiling)
            {
                stateMachine.ChangeState(player.IdleState);
            }
        }

    }
}
