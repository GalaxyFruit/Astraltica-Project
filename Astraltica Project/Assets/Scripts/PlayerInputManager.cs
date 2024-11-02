using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputManager : MonoBehaviour
{
    [Header("Input Action Asset")]
    [SerializeField] private InputActionAsset playerControls;

    [Header("Action Map Name References")]
    [SerializeField] private string actionMapName = "Player";

    [Header("Action Name References")]
    [SerializeField] private string move = "Move";
    [SerializeField] private string look = "Look";
    [SerializeField] private string jump = "Jump";
    [SerializeField] private string sprint = "Sprint";

    private InputAction moveAction;
    private InputAction lookAction;
    private InputAction jumpAction;
    private InputAction sprintAction;

    private HeadBob headBob;

    public Vector2 MoveInput { get; private set; }
    public Vector2 LookInput { get; private set; }
    public bool JumpTriggered { get; private set; }
    public bool IsSprinting { get; private set; }

    public static PlayerInputManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        moveAction = playerControls.FindActionMap(actionMapName).FindAction(move);
        lookAction = playerControls.FindActionMap(actionMapName).FindAction(look);
        jumpAction = playerControls.FindActionMap(actionMapName).FindAction(jump);
        sprintAction = playerControls.FindActionMap(actionMapName).FindAction(sprint);

        RegisterInputAction();
    }

    private void Start()
    {
        LockMouseCursor();
        headBob = HeadBob.Instance;
    }

    private void LockMouseCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    /// <summary>
    /// Registering input actions as event handlers, only triggers when the input action is performed.
    /// </summary>
    private void RegisterInputAction()
    {
        moveAction.performed += context =>
        {
            MoveInput = context.ReadValue<Vector2>();
            headBob.StartHeadBob();
        };

        moveAction.canceled += context =>
        {
            MoveInput = Vector2.zero;
            headBob.StopHeadBob();
        };

        lookAction.performed += context => LookInput = context.ReadValue<Vector2>();
        lookAction.canceled += context => LookInput = Vector2.zero;

        jumpAction.performed += context =>
        {
            JumpTriggered = true;
        };
        jumpAction.canceled += context => JumpTriggered = false;
        sprintAction.performed += context => IsSprinting = context.ReadValue<float>() > 0;
        sprintAction.canceled += context =>
        {
            IsSprinting = false;
        };
    }

    private void OnEnable()
    {
        moveAction.Enable();
        lookAction.Enable();
        jumpAction.Enable();
        sprintAction.Enable();
    }

    private void OnDisable()
    {
        moveAction.Disable();
        lookAction.Disable();
        jumpAction.Disable();
        sprintAction.Disable();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
