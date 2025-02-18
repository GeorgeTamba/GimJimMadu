using UnityEngine;
using Cinemachine;

public class PlayerScript : MonoBehaviour
{
    public CharacterController controller;
    public Transform playerCamera;

    public float walkSpeed = 4f;
    public float sprintSpeed = 12f;
    public float crouchSpeed = 2f;

    public float mouseSensitivity = 250f;
    public float crouchHeight = 0.5f;
    public float normalHeight = 2f;
    public float jumpHeight = 1.5f;
    public float gravity = -9.81f;

    private float currentStamina;
    private float xRotation = 0f;
    private float currentSpeed;
    private Vector3 velocity;
    private bool isGrounded;

    public CinemachineVirtualCamera virtualCamera;
    public Transform followTarget;
    public Vector3 cameraOffset;

    void Start()
    {
        Debug.Log("test github");
        Cursor.lockState = CursorLockMode.Locked;
        currentSpeed = walkSpeed;

        // Set follow and offset for Cinemachine camera
        if (virtualCamera != null && followTarget != null)
        {
            virtualCamera.Follow = followTarget;
            virtualCamera.transform.localPosition = cameraOffset;
        }
    }

    void Update()
    {
        HandleMovement();
        //HandleMouseLook();
    }

    void HandleMovement()
    {
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 move = transform.right * moveX + transform.forward * moveZ;

        // Sprint
        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed = sprintSpeed;
        }
        //// Crouch
        //else if (Input.GetKey(KeyCode.LeftControl))
        //{
        //    currentSpeed = crouchSpeed;
        //    controller.height = crouchHeight;
        //    controller.center = new Vector3(0f, crouchHeight / 2, 0f); // Adjust center for crouch
        //}
        //else
        //{
        //    currentSpeed = walkSpeed;
        //    controller.height = normalHeight;
        //    controller.center = new Vector3(0f, normalHeight / 2, 0f); // Reset center for standing
        //}

        controller.Move(move * currentSpeed * Time.deltaTime);

        // Jump
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    //void HandleMouseLook()
    //{
    //    float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
    //    float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

    //    xRotation -= mouseY;
    //    xRotation = Mathf.Clamp(xRotation, -90f, 90f);

    //    playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    //    transform.Rotate(Vector3.up * mouseX);
    //}
}