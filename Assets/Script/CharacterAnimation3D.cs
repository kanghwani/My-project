using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CharacterAnimation3D : MonoBehaviour
{
    [Header("Animation Settings")]
    [SerializeField] private float moveBlendDampTime = 0.1f;

    private Animator animator;
    private IMovementState movementState; // 

    private static readonly int MoveSpeedHash = Animator.StringToHash("MoveSpeed");
    private static readonly int IsGroundedHash = Animator.StringToHash("IsGrounded");
    private static readonly int VerticalVelocityHash = Animator.StringToHash("VerticalVelocity");
    private static readonly int JumpHash = Animator.StringToHash("Jump");

    private void Awake()
    {
        animator = GetComponent<Animator>();
        movementState = GetComponent<IMovementState>();
    }

    private void OnEnable()
    {
        if (movementState != null)
            movementState.OnJumpStarted += TriggerJumpAnimation;
    }

    private void OnDisable()
    {
        if (movementState != null)
            movementState.OnJumpStarted -= TriggerJumpAnimation;
    }

    private void Update()
    {
        if (animator == null || movementState == null) return;

        animator.SetFloat(MoveSpeedHash, movementState.CurrentMoveSpeed01, moveBlendDampTime, Time.deltaTime);
        animator.SetBool(IsGroundedHash, movementState.IsGrounded);
        animator.SetFloat(VerticalVelocityHash, movementState.VerticalVelocity);
    }

    private void TriggerJumpAnimation()
    {
        if (animator != null)
        {
            animator.SetTrigger(JumpHash);
        }
    }
}