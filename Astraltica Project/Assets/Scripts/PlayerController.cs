using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Speeds")]
    [SerializeField] private float walkSpeed = 3.0f;
    [SerializeField] private float sprintMultiplier = 2.0f;

    [Header("Jump Parameters")]
    [SerializeField] private float jumpForce = 5.0f;
    [SerializeField] private float gravity = 9.81f;

    [Header("Look Sensitivity")]
    [SerializeField] private float mouseSensitivity = 2.0f;
    [SerializeField] private float upDownRange = 80.0f;

    private CharacterController characterController;
    private Camera mainCamera;
    private PlayerInputManager inputManager;
    private Vector3 currentMovement = Vector3.zero;
    private float verticalRotation;
    private bool isJumping = false;

    private PlayerAnimationController playerAnimationController;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        mainCamera = Camera.main;
        inputManager = PlayerInputManager.Instance;
        playerAnimationController = GetComponent<PlayerAnimationController>();
    }

    private void Update()
    {
        HandleRotation();
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        Vector3 movementInput = new Vector3(inputManager.MoveInput.x, 0f, inputManager.MoveInput.y).normalized;
        Vector3 horizontalMovement = transform.forward * movementInput.z + transform.right * movementInput.x;

        float speed = walkSpeed * (inputManager.Sprinting ? sprintMultiplier : 1f);
        currentMovement.x = horizontalMovement.x * speed;
        currentMovement.z = horizontalMovement.z * speed;

        HandleJumping();
        characterController.Move(currentMovement * Time.deltaTime);
    }

    private void HandleJumping()
    {
        if (characterController.isGrounded)
        {
            if (isJumping)
            {
                isJumping = false;
            }

            currentMovement.y = -2f;

            if (inputManager.JumpTriggered && !isJumping)
            {
                playerAnimationController.TriggerJump();
                currentMovement.y = jumpForce;
                isJumping = true;
            }
        }
        else
        {
            currentMovement.y -= gravity * Time.deltaTime;
        }
    }

    private void HandleRotation()
    {
        float mouseXRotation = inputManager.LookInput.x * mouseSensitivity;
        transform.Rotate(0, mouseXRotation, 0);

        verticalRotation -= inputManager.LookInput.y * mouseSensitivity;
        verticalRotation = Mathf.Clamp(verticalRotation, -upDownRange, upDownRange);
        mainCamera.transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
    }
}
