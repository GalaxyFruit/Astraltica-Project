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
    [SerializeField] private string interact = "UseWatch";

    private InputAction moveAction;
    private InputAction lookAction;
    private InputAction jumpAction;
    private InputAction sprintAction;
    private InputAction useWatchAction;

    public Vector2 MoveInput { get; private set; }
    public Vector2 LookInput { get; private set; }
    public bool JumpTriggered { get; private set; }
    public bool IsSprinting { get; private set; }

    public delegate void MovementEvent(Vector2 input);
    public event MovementEvent OnMoveInputChanged;

    public delegate void LookEvent(Vector2 lookInput);
    public event LookEvent OnLookInputChanged;

    public delegate void SprintEvent(bool isSprinting);
    public event SprintEvent OnSprintChanged;

    public delegate void JumpEvent();
    public event JumpEvent OnJumpTriggered;

    public delegate void UseWatchEvent();
    public event UseWatchEvent OnUseWatchTriggered; // Event for "UseWatch"

    private void Awake()
    {
        moveAction = playerControls.FindActionMap(actionMapName).FindAction(move);
        lookAction = playerControls.FindActionMap(actionMapName).FindAction(look);
        jumpAction = playerControls.FindActionMap(actionMapName).FindAction(jump);
        sprintAction = playerControls.FindActionMap(actionMapName).FindAction(sprint);
        useWatchAction = playerControls.FindActionMap(actionMapName).FindAction(interact);

        RegisterInputAction();
    }

    private void Start()
    {
        LockMouseCursor();
    }

    private void LockMouseCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void RegisterInputAction()
    {
        moveAction.performed += context =>
        {
            MoveInput = context.ReadValue<Vector2>();
            OnMoveInputChanged?.Invoke(MoveInput);
        };

        lookAction.performed += context =>
        {
            LookInput = context.ReadValue<Vector2>();
            OnLookInputChanged?.Invoke(LookInput);
        };
        lookAction.canceled += context => LookInput = Vector2.zero;

        moveAction.canceled += context =>
        {
            MoveInput = Vector2.zero;
            OnMoveInputChanged?.Invoke(MoveInput);
        };

        sprintAction.performed += context =>
        {
            IsSprinting = context.ReadValue<float>() > 0;
            OnSprintChanged?.Invoke(IsSprinting);
        };
        sprintAction.canceled += context =>
        {
            IsSprinting = false;
            OnSprintChanged?.Invoke(IsSprinting);
        };

        jumpAction.performed += context =>
        {
            JumpTriggered = true;
            OnJumpTriggered?.Invoke();
        };

        useWatchAction.performed += context =>
        {
            OnUseWatchTriggered?.Invoke();
            //Debug.Log("Clicked F");
        };
    }
}
