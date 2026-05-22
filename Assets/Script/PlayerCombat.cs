using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [Header("공격 설정")]
    [SerializeField] private float attackDamage = 20f;
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float attackCooldown = 0.5f;
    [SerializeField] private LayerMask enemyLayer;

    private float lastAttackTime = -999f;
    private PlayerInputReader inputReader;
    private Animator animator;

    private void Awake()
    {
        inputReader = GetComponent<PlayerInputReader>();
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        inputReader.OnAttackRequested += TryAttack;
    }

    private void OnDisable()
    {
        inputReader.OnAttackRequested -= TryAttack;
    }

    private void TryAttack()
    {
        if (Time.time - lastAttackTime < attackCooldown) return;

        lastAttackTime = Time.time;
        PlayAttackAnimation();
        DetectAndDamageEnemy();
    }

    private void PlayAttackAnimation()
    {
        if (animator != null)
            animator.SetTrigger("Attack");
    }

    private void DetectAndDamageEnemy()
    {
        Vector3 origin = transform.position + Vector3.up;

        if (Physics.Raycast(origin, transform.forward, out RaycastHit hit, attackRange, enemyLayer))
        {
            if (hit.collider.GetComponentInParent<IDamageable>() is IDamageable damageable)
            {
                damageable.TakeDamage(attackDamage);
                Debug.Log($"[Combat] {hit.collider.name} {attackDamage} 데미지!");
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position + Vector3.up, transform.forward * attackRange);
    }
}