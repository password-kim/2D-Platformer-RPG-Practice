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

    [Header("In Air State")]
    public float CoyoteTime = 0.2f;
    public float VariableJumpHeightMultiplier = 0.5f;

    [Header("Wall Slide State")]
    public float WallSlideVelocity = 3f;

    [Header("Wall Climb State")]
    public float WallClimbVelocity = 3f;

    [Header("Check Variables")]
    public float GroundCheckRadius = 0.3f;
    public float WallCheckDistance = 0.5f;
    public LayerMask WhatIsGround;
}
