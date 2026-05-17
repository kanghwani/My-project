using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    [SerializeField] private float groundCheckRadius = 0.25f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform checkPoint;

    public bool IsGrounded { get; private set; }

    private void Update()
    {
        Vector3 position = checkPoint != null ? checkPoint.position : transform.position + Vector3.down * 0.8f;
        
        IsGrounded = Physics.CheckSphere(
            position,
            groundCheckRadius,
            groundLayer,
            QueryTriggerInteraction.Ignore
        );
    }

    private void OnDrawGizmosSelected()
    {
        Vector3 position = checkPoint != null ? checkPoint.position : transform.position + Vector3.down * 0.8f;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(position, groundCheckRadius);
    }
}