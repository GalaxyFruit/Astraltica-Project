using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Speeds")]
    [SerializeField] private float walkSpeed = 4.0f;
    [SerializeField] private float sprintMultiplier = 1.5f;

    [Header("Jump Parameters")]
    [SerializeField] private float jumpForce = 5.0f;
    [SerializeField] private float gravity = 9.81f;

    [Header("Look Sensitivity")]
    [SerializeField] private float mouseSensitivity = 2.0f;
    [SerializeField] private float upDownRange = 80.0f;

    [SerializeField] private StaminaController staminaController;

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
        playerAnimationController.UpdateGroundedState(characterController.isGrounded);
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        if (inputManager.MoveInput != Vector2.zero) // Pokud je vstup aktivní
        {
            Vector3 movementInput = new Vector3(inputManager.MoveInput.x, 0f, inputManager.MoveInput.y).normalized;

            float targetSpeed = walkSpeed * (inputManager.IsSprinting ? sprintMultiplier : 1f);
            float currentSpeed = targetSpeed * movementInput.magnitude;

            currentMovement.x = (transform.right * movementInput.x).x * currentSpeed;
            currentMovement.z = (transform.forward * movementInput.z).z * currentSpeed;

            // Výpočet směru jen pro animace
            float direction = 0f;
            if (movementInput.z < 0) direction = -1f; // dozadu
            else if (movementInput.x != 0) direction = Mathf.Sign(movementInput.x);

            playerAnimationController.UpdateBlendTree(currentSpeed * 0.25f, direction);
            Debug.Log("speed: " + currentSpeed / 4 + " direction: " + direction);
        }
        else // Pokud není žádný vstup, zastav pohyb
        {
            currentMovement.x = 0f;
            currentMovement.z = 0f;
            playerAnimationController.UpdateBlendTree(0f, 0f); // Reset animací na idle
        }

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
                playerAnimationController.ResetToGrounded();
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
