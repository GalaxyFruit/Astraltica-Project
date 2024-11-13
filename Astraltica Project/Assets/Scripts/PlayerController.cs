using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Speed")]
    [SerializeField] private float walkSpeed = 4.0f;
    [SerializeField] private float sprintMultiplier = 1.5f;
    [SerializeField] private float jumpForce = 5.0f;
    [SerializeField] private float gravity = 9.81f;

    [Header("Mouse Look Settings")]
    [SerializeField] private float mouseSensitivity = 2.0f;
    [SerializeField] private float verticalLookLimit = 80f;

    private CharacterController characterController;
    private Vector3 currentMovement = Vector3.zero;
    private bool isJumping = false;
    private PlayerInputManager inputManager;
    private PlayerAnimationController playerAnimationController;

    private Transform cameraTransform;
    private float currentXRotation = 0f;
    private Vector2 moveInput;
    private bool isSprinting;
    private Vector2 previousMoveInput;
    private bool previousIsSprinting;



    private void Awake()
    {
        playerAnimationController = GetComponent<PlayerAnimationController>();
        characterController = GetComponent<CharacterController>();
        inputManager = PlayerInputManager.Instance;

        //#miluju eventy
        inputManager.OnMoveInputChanged += HandleMovementInput;
        inputManager.OnLookInputChanged += HandleLookInput;
        inputManager.OnSprintChanged += HandleSprintInput;
        inputManager.OnJumpTriggered += HandleJumpInput;

        // Init uwu kamera
        if (Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
        }
    }

    private void FixedUpdate()
    {
        ApplyGravity();

        CalculateMoveInput();

        if (currentMovement != Vector3.zero)
        {
            characterController.Move(currentMovement * Time.deltaTime);
        }
    }

    private void CalculateMoveInput()
    {
        Vector3 forwardMovement = transform.forward * moveInput.y;
        Vector3 rightMovement = transform.right * moveInput.x;
        Vector3 direction = (forwardMovement + rightMovement).normalized;

        float targetSpeed = walkSpeed * (isSprinting ? sprintMultiplier : 1f);
        currentMovement.x = direction.x * targetSpeed;
        currentMovement.z = direction.z * targetSpeed;
    }

    private void HandleLookInput(Vector2 lookInput)
    {
        transform.Rotate(Vector3.up * lookInput.x * mouseSensitivity);

        currentXRotation -= lookInput.y * mouseSensitivity;
        currentXRotation = Mathf.Clamp(currentXRotation, -verticalLookLimit, verticalLookLimit);

        if (cameraTransform != null)
        {
            cameraTransform.localRotation = Quaternion.Euler(currentXRotation, 0f, 0f);
        }
    }


    private void HandleMovementInput(Vector2 input)
    {
        moveInput = input;

        // Trigger animation update only if movement input has changed
        if (moveInput != previousMoveInput)
        {
            UpdateAnimation();
            previousMoveInput = moveInput;
        }
    }

    private void HandleSprintInput(bool sprintStatus)
    {
        isSprinting = sprintStatus;

        // Trigger animation update only if sprint state has changed
        if (isSprinting != previousIsSprinting)
        {
            UpdateAnimation();
            previousIsSprinting = isSprinting;
        }
    }

    private void UpdateAnimation()
    {
        float targetSpeed = walkSpeed * (isSprinting ? sprintMultiplier : 1f);
        float currentSpeed = targetSpeed * moveInput.magnitude;

        float angle = Vector3.SignedAngle(Vector3.forward, new Vector3(moveInput.x, 0, moveInput.y), Vector3.up);
        float direction = Mathf.Clamp(angle / 90f, -1f, 1f);

        playerAnimationController.UpdateBlendTree(currentSpeed * 0.25f, direction);
    }




    private void HandleJumpInput()
    {
        if (characterController.isGrounded && !isJumping)
        {
            playerAnimationController.TriggerJump();
            currentMovement.y = jumpForce;
            isJumping = true;
        }
    }

    private void ApplyGravity()
    {
        if (!characterController.isGrounded)
        {
            currentMovement.y -= gravity * Time.deltaTime;
        }
        else if (isJumping)
        {
            isJumping = false;
        }
    }
}
