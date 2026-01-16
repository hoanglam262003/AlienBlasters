using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    private const string PLAYER_PREFS_BINDINGS = "InputBindings";
    public static GameInput Instance { get; private set; }

    public event EventHandler OnBindingRebind;

    public enum Binding
    {
        Move_Left,
        Move_Right,
        Jump,
    }

    private PlayerInputActions playerInputActions;
    private InputActionRebindingExtensions.RebindingOperation currentRebind;

    private void Awake()
    {
        Instance = this;
        playerInputActions = new PlayerInputActions();

        if (PlayerPrefs.HasKey(PLAYER_PREFS_BINDINGS))
        {
            playerInputActions.LoadBindingOverridesFromJson(
                PlayerPrefs.GetString(PLAYER_PREFS_BINDINGS));
        }

        playerInputActions.Player.Enable();
    }

    private void OnDestroy()
    {
        playerInputActions.Player.Disable();
        playerInputActions.Dispose();
    }

    // ================= MOVEMENT =================
    public float GetMoveHorizontal()
    {
        return playerInputActions.Player.Move.ReadValue<float>();
    }

    public bool IsJumpPressed()
    {
        return playerInputActions.Player.Jump.triggered;
    }

    // ================= REBIND =================
    public string GetBindingText(Binding binding)
    {
        switch (binding)
        {
            default:
            case Binding.Move_Left:
                return playerInputActions.Player.Move.bindings[1].ToDisplayString();
            case Binding.Move_Right:
                return playerInputActions.Player.Move.bindings[2].ToDisplayString();
            case Binding.Jump:
                return playerInputActions.Player.Jump.bindings[0].ToDisplayString();
        }
    }

    public void RebindBinding(Binding binding, Action onSuccess)
    {
        CancelRebindIfRunning();
        playerInputActions.Player.Disable();

        InputAction action;
        int bindingIndex;

        switch (binding)
        {
            default:
            case Binding.Move_Left:
                action = playerInputActions.Player.Move;
                bindingIndex = 1;
                break;
            case Binding.Move_Right:
                action = playerInputActions.Player.Move;
                bindingIndex = 2;
                break;
            case Binding.Jump:
                action = playerInputActions.Player.Jump;
                bindingIndex = 0;
                break;
        }

        currentRebind = action.PerformInteractiveRebinding(bindingIndex)
            .WithControlsExcluding("Mouse")
            .OnComplete(operation =>
            {
                operation.Dispose();
                currentRebind = null;

                playerInputActions.Player.Enable();

                PlayerPrefs.SetString(
                    PLAYER_PREFS_BINDINGS,
                    playerInputActions.SaveBindingOverridesAsJson());
                PlayerPrefs.Save();

                onSuccess?.Invoke();
                OnBindingRebind?.Invoke(this, EventArgs.Empty);
            });

        currentRebind.Start();
    }

    public void CancelRebindIfRunning()
    {
        if (currentRebind != null)
        {
            currentRebind.Cancel();
            currentRebind.Dispose();
            currentRebind = null;
        }
    }
}
