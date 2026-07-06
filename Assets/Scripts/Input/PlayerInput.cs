using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "Player Input")]
public class PlayerInput : ScriptableObject,InputActions.IGameplayActions,InputActions.IPauseMenuActions,InputActions.IGameOverScreenActions
{
    public event UnityAction<Vector2> onMove = delegate { };
    public event UnityAction onStopMove = delegate { };
    public event UnityAction onFire = delegate { };
    public event UnityAction onStopFire = delegate { };
    public event UnityAction onDodge = delegate { };
    public event UnityAction onOverdrive = delegate { };
    public event UnityAction onPause = delegate { };
    public event UnityAction onUnPause = delegate { };
    public event UnityAction onLaunchMissile = delegate { };
    public event UnityAction onConfirmGameOver = delegate { };
    
    private InputActions _inputActions;

    private void OnEnable()
    {
        _inputActions = new InputActions();
        _inputActions.Gameplay.SetCallbacks(this);
        _inputActions.PauseMenu.SetCallbacks(this);
        _inputActions.GameOverScreen.SetCallbacks(this);
    }

    private void OnDisable()
    {
        DisableAllInput();
    }

    private void SwitchActionMap(InputActionMap actionMap,bool isUIInput)
    {
        _inputActions.Disable();
        actionMap.Enable();
        if (isUIInput)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
    
    public void EnableGameplayInput() => SwitchActionMap(_inputActions.Gameplay, false);
    
    public void EnablePauseMenuInput() => SwitchActionMap(_inputActions.PauseMenu, true);

    public void EnableGameOverScreenInput() => SwitchActionMap(_inputActions.GameOverScreen, false);
    
    public void DisableAllInput() => _inputActions.Disable();

    public void SwitchToDynamicUpdateMode() => InputSystem.settings.updateMode = InputSettings.UpdateMode.ProcessEventsInDynamicUpdate;
    
    public void SwitchToFixedUpdateMode() => InputSystem.settings.updateMode = InputSettings.UpdateMode.ProcessEventsInFixedUpdate;
    
    public void OnMove(InputAction.CallbackContext context)
    {
        // if (context.phase == InputActionPhase.Performed)
        // {
        //     onMove.Invoke(context.ReadValue<Vector2>());
        // }
        //
        // if (context.phase == InputActionPhase.Canceled)
        // {
        //     onStopMove.Invoke();
        // }
        if (context.performed)
        {
            onMove.Invoke(context.ReadValue<Vector2>());
        }

        if (context.canceled)
        {
            onStopMove.Invoke();
        }
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        // if (context.phase == InputActionPhase.Performed)
        // {
        //     onFire.Invoke();
        // }
        //
        // if (context.phase == InputActionPhase.Canceled)
        // {
        //     onStopFire.Invoke();
        // }
        if (context.performed)
        {
            onFire.Invoke();
        }

        if (context.canceled)
        {
            onStopFire.Invoke();
        }
    }

    public void OnDodge(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            onDodge.Invoke();
        }
    }

    public void OnOverdrive(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            onOverdrive.Invoke();
        }
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            onPause.Invoke();
        }
    }

    public void OnUnPause(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            onUnPause.Invoke();
        }
    }

    public void OnLaunchMissile(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            onLaunchMissile.Invoke();
        }
    }

    public void OnConfirmGameOver(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            onConfirmGameOver.Invoke();
        }
    }
}
