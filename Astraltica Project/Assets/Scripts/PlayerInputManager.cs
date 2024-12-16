using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class PlayerInputManager : MonoBehaviour
{
    [Header("Input Action Asset")]
    [SerializeField] private InputActionAsset playerControls;

    [Header("Action Map Name References")]
    [SerializeField] private string actionMapName = "Player";

    [Header("Action Name References")]
    [SerializeField] private string[] actionNames = { "Move", "Look", "Jump", "Sprint", "UseWatch", "Inventory", "Hotbar" };

    private Dictionary<string, InputAction> actions = new();
    private Dictionary<string, System.Action<InputAction.CallbackContext>> actionCallbacks = new();

    private Dictionary<string, System.Action<InputAction.CallbackContext>> canceledActionCallbacks = new();

    public event System.Action<int> OnHotbarSlotSelected;

    public Vector2 MoveInput { get; private set; }
    public Vector2 LookInput { get; private set; }
    public bool JumpTriggered { get; private set; }
    public bool IsSprinting { get; private set; }

    public event System.Action<Vector2> OnMoveInputChanged;
    public event System.Action<Vector2> OnLookInputChanged;
    public event System.Action<bool> OnSprintChanged;
    public event System.Action OnJumpTriggered;
    public event System.Action OnUseWatchTriggered;
    public event System.Action OnInventoryChanged;

    private void Awake()
    {
        InitializeActions();
        RegisterCallbacks();
        RegisterCanceledCallbacks();
        RegisterInputActions();
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

    private void InitializeActions()
    {
        foreach (var actionName in actionNames)
        {
            var action = playerControls.FindActionMap(actionMapName).FindAction(actionName);
            if (action != null)
            {
                actions[actionName] = action;
            }
        }
    }

    private void RegisterCallbacks()
    {
        actionCallbacks["Move"] = context =>
        {
            MoveInput = context.ReadValue<Vector2>();
            OnMoveInputChanged?.Invoke(MoveInput);
        };

        actionCallbacks["Look"] = context =>
        {
            LookInput = context.ReadValue<Vector2>();
            OnLookInputChanged?.Invoke(LookInput);
        };

        actionCallbacks["Sprint"] = context =>
        {
            IsSprinting = context.ReadValue<float>() > 0;
            OnSprintChanged?.Invoke(IsSprinting);
        };

        actionCallbacks["Jump"] = context =>
        {
            JumpTriggered = true;
            OnJumpTriggered?.Invoke();
        };

        actionCallbacks["UseWatch"] = context => OnUseWatchTriggered?.Invoke();

        actionCallbacks["Inventory"] = context => OnInventoryChanged?.Invoke();
    }

    private void RegisterCanceledCallbacks()
    {
        canceledActionCallbacks["Move"] = context =>
        {
            MoveInput = Vector2.zero;
            OnMoveInputChanged?.Invoke(MoveInput);
        };

        canceledActionCallbacks["Look"] = context =>
        {
            LookInput = Vector2.zero;
            OnLookInputChanged?.Invoke(LookInput);
        };

        canceledActionCallbacks["Sprint"] = context =>
        {
            IsSprinting = false;
            OnSprintChanged?.Invoke(IsSprinting);
        };

        actionCallbacks["Hotbar"] = context =>
        {
            string controlName = context.control.name;

            if (int.TryParse(controlName, out int slotNumber))
            {
                Debug.Log($"Hotbar Slot Selected: {slotNumber}");
                OnHotbarSlotSelected?.Invoke(slotNumber);
            }
        };
    }


    private void RegisterInputActions()
    {
        foreach (var pair in actions)
        {
            if (actionCallbacks.ContainsKey(pair.Key))
            {
                pair.Value.performed += actionCallbacks[pair.Key];
            }

            if (canceledActionCallbacks.ContainsKey(pair.Key))
            {
                pair.Value.canceled += canceledActionCallbacks[pair.Key];
            }
        }
    }


    public void DisableInputs()
    {
        foreach (var pair in actions)
        {
            if (pair.Key == "Inventory") continue;

            pair.Value.Disable();
        }
    }

    public void EnableInputs()
    {
        foreach (var action in actions.Values)
        {
            action.Enable();
        }
    }

}
