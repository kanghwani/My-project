using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform cameraPivot;
    [SerializeField] private Transform playerBody;

    [Header("Camera Look")]
    [SerializeField] private float mouseSensitivity = 0.12f;
    [SerializeField] private float minPitch = -30f;
    [SerializeField] private float maxPitch = 65f;

    private IPlayerInput input;
    private float cameraPitch;

    private void Awake()
    {
        
        input = GetComponent<IPlayerInput>(); 
    }

    private void LateUpdate()
    {
        if (input == null || cameraPivot == null || playerBody == null) return;

        Vector2 mouseDelta = input.MouseDelta;
        if (mouseDelta.sqrMagnitude < 0.01f) return;

        // 몸통 좌우 
        playerBody.Rotate(Vector3.up * mouseDelta.x * mouseSensitivity);

        // 카메라 상하 
        cameraPitch -= mouseDelta.y * mouseSensitivity;
        cameraPitch = Mathf.Clamp(cameraPitch, minPitch, maxPitch);
        cameraPivot.localRotation = Quaternion.Euler(cameraPitch, 0f, 0f);
    }
}