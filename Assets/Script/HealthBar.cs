using UnityEngine;
using UnityEngine.UI;

// 플레이어 머리 위에 뜨는 HP바
public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private Transform followTarget;  // 따라다닐 오브젝트 (unitychan)
    [SerializeField] private Vector3 offset = new Vector3(0, 2.5f, 0);  // 머리 위 위치

    private void OnEnable()
    {
        if (playerHealth != null)
            playerHealth.OnHealthChanged += UpdateBar;
    }

    private void OnDisable()
    {
        if (playerHealth != null)
            playerHealth.OnHealthChanged -= UpdateBar;
    }

    // HP 변경 시 슬라이더 갱신
    private void UpdateBar(float current, float max)
    {
        slider.value = current / max;  // 0~1 사이 값
    }

    // 항상 플레이어 위에 위치, 카메라 방향으로 회전
    private void LateUpdate()
    {
        if (followTarget == null) return;

        transform.position = followTarget.position + offset;
        transform.forward = Camera.main.transform.forward;  // 카메라 향해 회전
    }
}