using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 targetOffset = new Vector3(0f, 1.5f, 0f);
    [SerializeField] private float distance = 6f;
    [SerializeField] private float mouseSensitivity = 3f;
    [SerializeField] private float minPitch = -20f;
    [SerializeField] private float maxPitch = 60f;

    [Header("Colisión")]
    [SerializeField] private LayerMask obstacleLayers = ~0;
    [SerializeField] private float collisionRadius = 0.3f;
    [SerializeField] private float minDistance = 0.5f;

    private float yaw;
    private float pitch = 20f;

    private void Start()
    {
        if (target == null) { enabled = false; return; }

        yaw = transform.eulerAngles.y;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void LateUpdate()
    {
        yaw += Input.GetAxis("Mouse X") * mouseSensitivity;
        pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);
        Vector3 pivot = target.position + targetOffset;
        Vector3 direction = rotation * Vector3.back;
        float finalDistance = distance;

        if (Physics.SphereCast(pivot, collisionRadius, direction, out RaycastHit hit,
            distance, obstacleLayers, QueryTriggerInteraction.Ignore))
        {
            finalDistance = Mathf.Max(hit.distance, minDistance);
        }

        transform.position = pivot + direction * finalDistance;
        transform.rotation = rotation;
    }

    private void OnDisable()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}