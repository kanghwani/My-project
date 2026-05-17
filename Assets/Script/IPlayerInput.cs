using System;
using UnityEngine;

// 입력을 제공
public interface IPlayerInput
{
    Vector2 MoveInput { get; }
    Vector2 MouseDelta { get; }
    bool IsSprinting { get; }
    event Action OnJumpRequested; 
}

// 캐릭터의 현재 상태를 외부에 제공
public interface IMovementState
{
    float CurrentMoveSpeed01 { get; }
    float VerticalVelocity { get; }
    bool IsGrounded { get; }
    event Action OnJumpStarted;
}