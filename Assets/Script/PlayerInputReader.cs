using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputReader : MonoBehaviour, IPlayerInput
{
    private InputAction moveAction;
    private InputAction lookAction;
    private InputAction jumpAction;
    private InputAction sprintAction;
    private InputAction attackAction; 
    
    

    public Vector2 MoveInput => moveAction != null ? moveAction.ReadValue<Vector2>() : Vector2.zero;
    public Vector2 MouseDelta => lookAction != null ? lookAction.ReadValue<Vector2>() : Vector2.zero;
    public bool IsSprinting => sprintAction != null && sprintAction.IsPressed();
    
    public event Action OnJumpRequested;
    public event Action OnAttackRequested;
    

    private void Awake()
    {
        if (InputSystem.actions != null)
        {
            moveAction = InputSystem.actions.FindAction("Move");
            lookAction = InputSystem.actions.FindAction("Look");
            jumpAction = InputSystem.actions.FindAction("Jump");
            sprintAction = InputSystem.actions.FindAction("Sprint");
            attackAction = InputSystem.actions.FindAction("Attack");
        }
    }

    private void OnEnable()
    {
        moveAction?.Enable();
        lookAction?.Enable();
        jumpAction?.Enable();
        sprintAction?.Enable();
        attackAction?.Enable();
    }

    private void OnDisable()
    {
        moveAction?.Disable();
        lookAction?.Disable();
        jumpAction?.Disable();
        sprintAction?.Disable();
        attackAction?.Disable();
    }

    private void Update()
    {
        if (jumpAction != null && jumpAction.WasPressedThisFrame())
            OnJumpRequested?.Invoke();

        if (attackAction != null && attackAction.WasPressedThisFrame()) // ← 추가
            OnAttackRequested?.Invoke();
    }
}