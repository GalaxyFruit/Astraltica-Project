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
    private bool isFalling = false;
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

        // Init kamera
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
        if (moveInput != previousMoveInput)
        {
            UpdateAnimation();
            previousMoveInput = moveInput;
        }
    }


    private void HandleSprintInput(bool sprintStatus)
    {
        isSprinting = sprintStatus;
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

        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        float forwardInput = Vector3.Dot(moveInput, forward);
        float rightInput = Vector3.Dot(moveInput, right);

        float direction = Mathf.Atan2(rightInput, forwardInput) * Mathf.Rad2Deg / 90f; 

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
            Debug.Log("HandleAirborneState(); called");
            HandleAirborneState();
        }
        else
        {
            HandleGroundedState();
        }
    }

    private void HandleAirborneState()
    {
        if (!isFalling)
        {
            playerAnimationController.SetFalling();
            isFalling = true;
        }
        currentMovement.y -= gravity * Time.deltaTime;
    }

    private void HandleGroundedState()
    {
        if (isJumping)
        {
            Debug.Log("isJumping set to False!");
            isJumping = false;
        }

        if (isFalling)
        {
            Debug.Log("isFalling is TRUE");
            isFalling = false;
            playerAnimationController.ResetToGrounded();

            HeadBob.Instance.ApplyDip();
        }
    }

}
