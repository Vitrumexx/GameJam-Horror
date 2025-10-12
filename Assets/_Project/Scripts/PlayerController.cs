using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed;
    public float runSpeed;
    public float gravity = -9.81f;

    [Header("Mouse Settings")]
    public float mouseSensitivity = 100f;
    public Transform playerCamera;
    private float xRotation = 0f;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        MouseLook();
        Movement();
    }

    void MouseLook()
    {
        float mouseX = 10 * Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = 10 * Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -70f, 70f);

        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    void Movement()
    {
        isGrounded = controller.isGrounded;

        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        float speed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;

        controller.Move(move * speed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
