using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CharacterMotor3D : MonoBehaviour, IMovementState
{
    [Header("References")]
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private GroundChecker groundChecker;

    [Header("Move Settings")]
    [SerializeField] private float walkSpeed = 2f;
    [SerializeField] private float runSpeed = 5f;
    [SerializeField] private float rotationSpeed = 12f;

    [Header("Jump Settings")]
    [SerializeField] private float jumpForce = 6f;
    [SerializeField] private float gravityMultiplier = 2f;

    private IPlayerInput input;
    private Rigidbody rb;
    
    private Vector3 moveDirection;
    private bool jumpRequested;

    // IMovementState 구현
    public float CurrentMoveSpeed01 { get; private set; }
    public float VerticalVelocity => rb.linearVelocity.y;
    public bool IsGrounded => groundChecker != null && groundChecker.IsGrounded;
    public event Action OnJumpStarted;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        rb.useGravity = true;

        input = GetComponent<IPlayerInput>();
    }

    private void OnEnable()
    {
        if (input != null)
            input.OnJumpRequested += HandleJumpRequest;
    }

    private void OnDisable()
    {
        if (input != null)
            input.OnJumpRequested -= HandleJumpRequest;
    }

    private void HandleJumpRequest()
    {
        if (IsGrounded)
        {
            jumpRequested = true;
        }
    }

    private void Update()
    {
        if (input == null) return;

        CalculateMoveDirection(input.MoveInput);
        UpdateBodyRotation(input.MoveInput);
    }

    private void FixedUpdate()
    {
        MoveWithRigidbody();
        JumpWithRigidbody();
        ApplyExtraGravity();
    }

    private void CalculateMoveDirection(Vector2 moveInput)
    {
        float inputMagnitude = moveInput.magnitude;

        if (inputMagnitude < 0.01f)
        {
            moveDirection = Vector3.zero;
            CurrentMoveSpeed01 = 0f;
            return;
        }

        Transform basisTransform = cameraTransform != null ? cameraTransform : transform;
        Vector3 forward = Vector3.ProjectOnPlane(basisTransform.forward, Vector3.up).normalized;
        Vector3 right = Vector3.ProjectOnPlane(basisTransform.right, Vector3.up).normalized;

        if (forward.sqrMagnitude < 0.01f) forward = transform.forward;
        if (right.sqrMagnitude < 0.01f) right = transform.right;

        Vector3 direction = forward * moveInput.y + right * moveInput.x;
        direction.y = 0f;

        moveDirection = direction.sqrMagnitude > 0.01f ? direction.normalized : Vector3.zero;
        float rawSpeed = input.IsSprinting ? 1f : 0.5f;
        CurrentMoveSpeed01 = Mathf.Lerp(CurrentMoveSpeed01, rawSpeed * inputMagnitude, Time.deltaTime * 10f);
    }

    private void UpdateBodyRotation(Vector2 moveInput)
    {
        if (moveDirection.sqrMagnitude < 0.01f) return;

        Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private void MoveWithRigidbody()
    {
        float targetSpeed = (input != null && input.IsSprinting) ? runSpeed : walkSpeed;
        Vector3 horizontalVelocity = moveDirection * targetSpeed;
        
        rb.linearVelocity = new Vector3(horizontalVelocity.x, rb.linearVelocity.y, horizontalVelocity.z);
    }

    private void JumpWithRigidbody()
    {
        if (!jumpRequested) return;
        jumpRequested = false;

        Vector3 velocity = rb.linearVelocity;
        velocity.y = 0f; // 점프 전 Y축 속도를 초기화
        rb.linearVelocity = velocity;

        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        OnJumpStarted?.Invoke();
    }

    private void ApplyExtraGravity()
    {
        if (IsGrounded) return;
        if (rb.linearVelocity.y > 0f) return; // ← 상승 중엔 스킵
        rb.AddForce(Physics.gravity * (gravityMultiplier - 1f), ForceMode.Acceleration);
    }
}