using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveForce = 30f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float extraGravity = 15f;

    [Header("References")]
    [SerializeField] private Transform cameraTransform;

    [Header("Items")]
    [SerializeField] private ItemCounterUI itemCounter;

    private Rigidbody rigidbodyComponent;
    private Vector3 moveDirection;
    private bool isGrounded;
    private bool jumpRequested;
    private int groundContacts;

    private void Awake()
    {
        rigidbodyComponent = GetComponent<Rigidbody>();

        if (cameraTransform == null && Camera.main != null)
            cameraTransform = Camera.main.transform;
    }

    private void Update()
    {
        float horizontal = 0f;
        float vertical = 0f;

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            horizontal -= 1f;
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            horizontal += 1f;
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            vertical -= 1f;
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            vertical += 1f;

        // Movimiento relativo a la cámara
        Vector3 forward = cameraTransform.forward;
        forward.y = 0f;
        forward.Normalize();

        Vector3 right = cameraTransform.right;
        right.y = 0f;
        right.Normalize();

        moveDirection = (right * horizontal + forward * vertical).normalized;

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            jumpRequested = true;
        }
    }

    private void FixedUpdate()
    {
        rigidbodyComponent.AddForce(moveDirection * moveForce);

        // Gravedad extra para que la caída sea más rápida
        rigidbodyComponent.AddForce(Vector3.down * extraGravity, ForceMode.Acceleration);

        if (jumpRequested)
        {
            rigidbodyComponent.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            jumpRequested = false;
            isGrounded = false;
            groundContacts = 0;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            groundContacts++;
            isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            groundContacts = Mathf.Max(0, groundContacts - 1);
            isGrounded = groundContacts > 0;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Collectible"))
        {
            if (itemCounter != null)
                itemCounter.AddItem();

            Destroy(other.gameObject);
        }
    }
}