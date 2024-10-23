using System;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerInputManager : MonoBehaviour
{
    [Header("Input Action Asset")]
    [SerializeField] private InputActionAsset playerControls;

    [Header("Action Map Name References")]
    [SerializeField] private string actionMapName = "Player";

    // vytváření typu a názvu akce
    [Header("Action Name References")]
    [SerializeField] private string move = "Move";
    [SerializeField] private string look = "Look";
    [SerializeField] private string jump = "Jump";
    [SerializeField] private string sprint = "Sprint";

    //input akce
    private InputAction moveAction;
    private InputAction lookAction;
    private InputAction jumpAction;
    private InputAction sprintAction;

    //vytváření vlastnosti ke čtení
    public Vector2 MoveInput {  get; private set; }
    public Vector2 LookInput {  get; private set; }
    public bool JumpTriggered {  get; private set; }
    public float SprintValue {  get; private set; }

    public static PlayerInputManager Instance { get; private set; }

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject.transform.parent);
        } else
        {
            Destroy(gameObject.transform.parent);
        }

        //hledání akce, kterou jsme vytvořili 
        moveAction = playerControls.FindActionMap(actionMapName).FindAction(move);
        lookAction = playerControls.FindActionMap(actionMapName).FindAction(look);
        jumpAction = playerControls.FindActionMap(actionMapName).FindAction(jump);
        sprintAction = playerControls.FindActionMap(actionMapName).FindAction(sprint);

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


    /// <summary>
    /// Registrování vstupů k akcím, které jsme vytvořili
    /// Jednoduše řečeno dáváme hodnotu když Unity zaznamená danou akcí a při zrušení hodnotu dáme zpátky na null
    /// </summary>
    private void RegisterInputAction()
    {
        RegisterAction(moveAction, context => MoveInput = context.ReadValue<Vector2>(), () => MoveInput = Vector2.zero);
        RegisterAction(lookAction, context => LookInput = context.ReadValue<Vector2>(), () => LookInput = Vector2.zero);
        RegisterAction(jumpAction, context => JumpTriggered = true, () => JumpTriggered = false);
        RegisterAction(sprintAction, context => SprintValue = context.ReadValue<float>(), () => SprintValue = 0f);
    }

    private void RegisterAction(InputAction action, Action<InputAction.CallbackContext> onPerformed, Action onCanceled)
    {
        action.performed += onPerformed;
        action.canceled += context => onCanceled();
    }



    /// <summary>
    /// Zapnutí trackování Inputu
    /// </summary>
    private void OnEnable()
    {
        moveAction.Enable();
        lookAction.Enable();
        jumpAction.Enable();
        sprintAction.Enable();
    }

    /// <summary>
    /// Vypnutí trackování Inputu
    /// </summary>
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
