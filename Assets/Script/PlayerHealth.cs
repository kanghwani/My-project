using UnityEngine;
using UnityEngine.SceneManagement;  // 씬 재시작용

public class PlayerHealth : MonoBehaviour, IDamageable
{
    [Header("HP 설정")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float gameOverDelay = 2f;  // 사망 후 재시작까지 대기
    [SerializeField] private UnityEngine.UI.Image damageOverlay; // 빨간 전체화면 Image

    public float CurrentHealth { get; private set; }
    public float MaxHealth => maxHealth;

    // HP 변경 시 HealthBar에게 알려주는 이벤트
    // float: 현재HP, float: 최대HP
    public event System.Action<float, float> OnHealthChanged;

    private bool isDead = false;

    private void Start()
    {
        CurrentHealth = maxHealth;
        OnHealthChanged?.Invoke(CurrentHealth, maxHealth);  // 시작 시 UI 초기화
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return;
        CurrentHealth = Mathf.Max(0f, CurrentHealth - amount);
        OnHealthChanged?.Invoke(CurrentHealth, maxHealth);
    
        StopAllCoroutines();
        StartCoroutine(DamageFlash()); // ← 추가
    
        if (CurrentHealth <= 0f) Die();
    }
    
    private System.Collections.IEnumerator DamageFlash()
    {
        if (damageOverlay == null) yield break;
        damageOverlay.color = new Color(1, 0, 0, 0.4f); // 반투명 빨강
        yield return new WaitForSeconds(0.3f);
        damageOverlay.color = new Color(1, 0, 0, 0f);   // 투명
    }

    public void Heal(float amount)
    {
        if (isDead) return;

        CurrentHealth = Mathf.Min(maxHealth, CurrentHealth + amount);
        Debug.Log($"[Player] 회복 → HP: {CurrentHealth}/{maxHealth}");

        OnHealthChanged?.Invoke(CurrentHealth, maxHealth);  // UI 업데이트
    }

    private void Die()
    {
        isDead = true;
        Debug.Log("[Player] 사망! 게임오버");
        StartCoroutine(GameOverSequence());
    }

    private System.Collections.IEnumerator GameOverSequence()
    {
        // gameOverDelay초 후 현재 씬 재시작
        yield return new WaitForSeconds(gameOverDelay);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}