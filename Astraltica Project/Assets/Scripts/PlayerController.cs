using System;
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
    [Space]
    [SerializeField] private HeadBob _headBob;
    [SerializeField] private StaminaController _staminaController;

    public Transform CameraTransform => cameraTransform;
    public WeaponController weaponController;

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
    private BoostManager boostManager;



    private void Awake()
    {
        playerAnimationController = GetComponent<PlayerAnimationController>();
        characterController = GetComponent<CharacterController>();
        inputManager = GetComponent<PlayerInputManager>();
        boostManager = BoostManager.Instance;

        //#miluju eventy
        //bool isCalled = inputManager.didAwake;
        //Debug.Log("is called?: " + isCalled);
        inputManager.OnMoveInputChanged += HandleMovementInput;
        inputManager.OnLookInputChanged += HandleLookInput;
        inputManager.OnSprintChanged += HandleSprintInput;
        inputManager.OnJumpTriggered += HandleJumpInput;

        inputManager.OnUseWatchTriggered += playerAnimationController.ToggleWatch;

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

    public float GetCurrentSpeed()
    {
        return moveInput.magnitude * walkSpeed * (isSprinting ? sprintMultiplier : 1f);
    }


    private void CalculateMoveInput()
    {
        Vector3 forwardMovement = transform.forward * moveInput.y;
        Vector3 rightMovement = transform.right * moveInput.x;
        Vector3 direction = (forwardMovement + rightMovement).normalized;

        BoostData boostData = boostManager.GetCurrentBoostMultiplier();
        float targetSpeed = walkSpeed * (isSprinting ? sprintMultiplier : 1f) * boostData.speedMultiplier;
        //Debug.Log($"target speed is {targetSpeed} in playercontroller. boost is {boostData.speedMultiplier}");
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

        if (weaponController.HasWeapon)
        {
            weaponController.UpdateWeaponRotation(cameraTransform);
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
        bool newSprintingState = sprintStatus && _staminaController.CurrentStamina > 0;
        //Debug.Log($"current stamina is {_staminaController.CurrentStamina} in playercontroller");

        if (isSprinting != newSprintingState)
        {
            isSprinting = newSprintingState;

            if (isSprinting)
                _staminaController.StartSprint();
            else
                _staminaController.StopSprint();

            UpdateAnimation();
        }

    }
    public void StopSprint()
    {
        if (isSprinting)
        {
            isSprinting = false;
            UpdateAnimation();
        }
    }


    private void UpdateAnimation()
    {
        BoostData boostData = boostManager.GetCurrentBoostMultiplier();
        float targetSpeed = walkSpeed * (isSprinting ? sprintMultiplier : 1f) * boostData.speedMultiplier; // cílová rychlost, pokud běží tak se mění
        float currentSpeed = targetSpeed * moveInput.magnitude; //síla stisku
        
        Vector3 forward = cameraTransform.forward; // směr dopředu z kamery.
        Vector3 right = cameraTransform.right; // směr doprava z kamery.
        forward.y = 0f; // vyrušíme vertikální pozici Y
        right.y = 0f;

        forward.Normalize();
        right.Normalize(); //normalizace chceme použít pouze směr a velikost pohybu

        float forwardInput = Vector3.Dot(moveInput, forward); // Skalární součin určuje, jak moc je pohyb v souladu se směry dopředu a do stran (ChatGPT)
        float rightInput = Vector3.Dot(moveInput, right);

        float direction = Mathf.Atan2(rightInput, forwardInput) * Mathf.Rad2Deg / 90f; // Spočítá směr pohybu hráče v radiánech a převede na stupně (ChatGPT)

        playerAnimationController.UpdateBlendTree(currentSpeed * 0.25f, direction);
    }


    private void HandleJumpInput()
    {
        if (characterController.isGrounded && !isJumping)
        {
            BoostData boostData = boostManager.GetCurrentBoostMultiplier();
            playerAnimationController.TriggerJump();
            currentMovement.y = jumpForce * boostData.jumpMultiplier;
            isJumping = true;
            playerAnimationController.UpdateGroundedState(false);
        }
    }

    private void ApplyGravity()
    {
        if (!characterController.isGrounded)
        {
            //Debug.Log("HandleAirborneState(); called");
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
            //Debug.Log("isJumping set to False!");
            isJumping = false;
        }

        if (isFalling)
        {
            //Debug.Log("isFalling is TRUE");
            isFalling = false;
            playerAnimationController.UpdateGroundedState(true);

            _headBob.ApplyDip();
        }
    }

    private void OnDestroy()
    {
        if (inputManager != null)
        {
            inputManager.OnMoveInputChanged -= HandleMovementInput;
            inputManager.OnLookInputChanged -= HandleLookInput;
            inputManager.OnSprintChanged -= HandleSprintInput;
            inputManager.OnJumpTriggered -= HandleJumpInput;
            inputManager.OnUseWatchTriggered -= playerAnimationController.ToggleWatch;
        }
    }

}
