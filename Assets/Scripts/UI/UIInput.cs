using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem.UI;

public class UIInput : Singleton<UIInput>
{
    [SerializeField] private PlayerInput _playerInput;
    private InputSystemUIInputModule _uiInputModule;

    protected override void Awake()
    {
        base.Awake();
        _uiInputModule = GetComponent<InputSystemUIInputModule>();
        _uiInputModule.enabled = false;
    }

    public void SelectUI(Selectable uiObject)
    {
        uiObject.Select();
        uiObject.OnSelect(null);
        _uiInputModule.enabled = true;
    }

    public void DisableAllUIInputs()
    {
        _playerInput.DisableAllInput();
        _uiInputModule.enabled = false;
    }
}
