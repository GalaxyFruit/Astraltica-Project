using UnityEngine;

/// <summary>
/// Class for player movement and interaction with animation controller
/// </summary>
public class PlayerController : MonoBehaviour
{
    public float speed = 5f;         
    public float sprintSpeed = 10f;  
    public float jumpForce = 5f;    
    public float mouseSensitivity = 100f; 
    public Transform cameraTransform;

    public StaminaController staminaController;

    private CharacterController characterController;
    private Vector3 velocity;
    private bool isGrounded;
    private float xRotation = 0f;

    private PlayerAnimationController animationController;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        animationController = GetComponent<PlayerAnimationController>();  

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // Check if player is on ground
        isGrounded = characterController.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // Camera rotation by mouse cursor
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); 

        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);

        // Movement control
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 move = transform.right * moveX + transform.forward * moveZ;

        //bool isWalking = move.magnitude > 0; 

        //animationController.SetWalking(isWalking);

        // Handle sprinting logic
        if (Input.GetKey(KeyCode.LeftShift) && staminaController.CanSprint())
        {
            characterController.Move(move * sprintSpeed * Time.deltaTime);
            //animationController.SetRunning(true);
            staminaController.StartSprinting();
        }
        else
        {
            characterController.Move(move * speed * Time.deltaTime);
            //animationController.SetRunning(false);
            staminaController.StopSprinting();
        }

        // Jumping logic
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpForce * -2f * Physics.gravity.y);
            //animationController.TriggerJump();
        }

        velocity.y += Physics.gravity.y * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }
}
