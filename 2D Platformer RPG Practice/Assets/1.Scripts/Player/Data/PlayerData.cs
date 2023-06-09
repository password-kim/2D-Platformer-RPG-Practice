using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Data/Player Data/Base Data")]
public class PlayerData : ScriptableObject
{
    [Header("Move State")]
    public float MovementVelocity = 10f;

    [Header("Jump State")]
    public float JumpVelocity = 15f;
    public int AmountOfJumps = 1;

    [Header("Wall Jump State")]
    public float WallJumpVelocity = 20f;
    public float WallJumpTime = 0.4f;
    public Vector2 WallJumpAngle = new Vector2(1, 2);

    [Header("In Air State")]
    public float CoyoteTime = 0.2f;
    public float VariableJumpHeightMultiplier = 0.5f;

    [Header("Wall Slide State")]
    public float WallSlideVelocity = 3f;

    [Header("Wall Climb State")]
    public float WallClimbVelocity = 3f;

    [Header("Ledge Climb State")]
    public Vector2 StartOffset;
    public Vector2 StopOffset;

    [Header("Dash State")]
    public float DashCoolDown = 0.5f;
    public float MaxHoldTime = 1f;
    public float HoldTimeScale = 0.25f;
    public float DashTime = 0.2f;
    public float DashVelocity = 30f;
    public float Drag = 10f;
    public float DashEndYMultiplier = 0.2f;
    public float DistBetweenAfterImages = 0.5f;

    [Header("Crouch State")]
    public float CrouchMovementVelocity = 5f;
    public float StandColliderHeight = 1.6f;
    public float CrouchColliderHeight = 0.8f;

    [Header("Check Variables")]
    public float GroundCheckRadius = 0.3f;
    public float WallCheckDistance = 0.5f;
    public LayerMask WhatIsGround;
}
