using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private float maxHealth = 100f;

    private float currentHealth;
    private Renderer enemyRenderer;  // 색상 변화용

    private void Start()
    {
        currentHealth = maxHealth;
        // 자신 또는 자식에서 Renderer 찾기
        enemyRenderer = GetComponentInChildren<Renderer>();
    }

    public void TakeDamage(float amount)
    {
        currentHealth = Mathf.Max(0f, currentHealth - amount);
        Debug.Log($"[Enemy] HP: {currentHealth}/{maxHealth}");

        FlashDamageColor();

        if (currentHealth <= 0f)
            Die();
    }

    // 피격 시 빨간색으로 잠깐 바뀜
    private void FlashDamageColor()
    {
        if (enemyRenderer == null) return;
        // 코루틴으로 0.2초 후 원래 색으로 복구
        StopAllCoroutines();
        StartCoroutine(DamageFlash());
    }

    private System.Collections.IEnumerator DamageFlash()
    {
        enemyRenderer.material.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        enemyRenderer.material.color = Color.white;
    }

    private void Die()
    {
        
        Debug.Log($"[Enemy] {gameObject.name} 처치!");
        Destroy(gameObject);
    }
}