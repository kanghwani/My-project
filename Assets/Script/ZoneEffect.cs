using UnityEngine;

// 구역 안에 있는 동안 지속적으로 데미지 또는 회복을 주는 컴포넌트
public class ZoneEffect : MonoBehaviour
{
    // 효과 타입 선택
    public enum EffectType { Damage, Heal }

    [Header("구역 설정")]
    [SerializeField] private EffectType effectType = EffectType.Damage;
    [SerializeField] private float effectAmount = 10f;   // 초당 효과량
    [SerializeField] private float tickInterval = 1f;    // 몇 초마다 적용할지

    private PlayerHealth playerInZone;  // 현재 구역 안의 플레이어
    private float tickTimer;            // 다음 틱까지 남은 시간

    private void OnTriggerEnter(Collider other)
    {
        // 플레이어가 들어왔을 때만 처리
        if (other.TryGetComponent<PlayerHealth>(out var playerHealth))
        {
            playerInZone = playerHealth;
            tickTimer = 0f; // 들어오자마자 즉시 1회 적용
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // 플레이어가 나갔을 때 초기화
        if (other.TryGetComponent<PlayerHealth>(out _))
            playerInZone = null;
    }

    private void Update()
    {
        if (playerInZone == null) return;

        // 타이머 감소
        tickTimer -= Time.deltaTime;

        if (tickTimer > 0f) return;

        // 틱 실행
        tickTimer = tickInterval;

        if (effectType == EffectType.Damage)
            playerInZone.TakeDamage(effectAmount);
        else
            playerInZone.Heal(effectAmount);
    }
}