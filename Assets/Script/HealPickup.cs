using UnityEngine;

// 플레이어가 닿으면 HP를 회복하고 사라지는 오브젝트
public class HealPickup : MonoBehaviour
{
    [SerializeField] private float healAmount = 30f;  // 회복량

    // Trigger에 뭔가 들어왔을 때 호출됨
    private void OnTriggerEnter(Collider other)
    {
        // 들어온 오브젝트에서 PlayerHealth를 찾음
        if (!other.TryGetComponent<PlayerHealth>(out var playerHealth)) return;

        playerHealth.Heal(healAmount);
        Destroy(gameObject);  // 아이템 제거
    }
}