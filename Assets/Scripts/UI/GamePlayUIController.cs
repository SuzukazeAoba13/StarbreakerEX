using UnityEngine.UI;
using UnityEngine;

public class GamePlayUIController : MonoBehaviour
{
    [Header("---- Player Input ----")]
    [SerializeField] private PlayerInput _playerInput;

    [Header("---- Audio Data ----")] 
    [SerializeField] private AudioData _pauseSFX;
    [SerializeField] private AudioData _unPauseSFX;
    
    [Header("---- Canvas ----")]
    [SerializeField] private Canvas _hudCanvas;
    [SerializeField] private Canvas _menuCanvas;
    
    [Header("---- Buttons ----")]
    [SerializeField] private Button _resumeButton;
    [SerializeField] private Button _optionsButton;
    [SerializeField] private Button _mainMenuButton;

    private int _buttonPressedParameterID = Animator.StringToHash("Pressed");
    
    private void OnEnable()
    {
        _playerInput.onPause += Pause;
        _playerInput.onUnPause += UnPause;
        
        ButtonPressedBehaviour.ButtonFunctionTable.Add(_resumeButton.name, OnResumeButtonClick);
        ButtonPressedBehaviour.ButtonFunctionTable.Add(_optionsButton.name, OnOptionsButtonClick);
        ButtonPressedBehaviour.ButtonFunctionTable.Add(_mainMenuButton.name, OnMainMenuButtonClick);
    }

    private void OnDisable()
    {
        _playerInput.onPause -= Pause;
        _playerInput.onUnPause -= UnPause;
        
        ButtonPressedBehaviour.ButtonFunctionTable.Clear();
    }

    private void Pause()
    {
        _hudCanvas.enabled = false;
        _menuCanvas.enabled = true;
        TimeController.Instance.Pause();
        GameManager.GameState = GameState.Paused;
        _playerInput.EnablePauseMenuInput();
        _playerInput.SwitchToDynamicUpdateMode();
        UIInput.Instance.SelectUI(_resumeButton);
        AudioManager.Instance.PlaySFX(_pauseSFX);
    }

    private void UnPause()
    {
        _resumeButton.Select();
        _resumeButton.animator.SetTrigger(_buttonPressedParameterID);
        AudioManager.Instance.PlaySFX(_unPauseSFX);
    }

    private void OnResumeButtonClick()
    {
        _menuCanvas.enabled = false;
        _hudCanvas.enabled = true;
        GameManager.GameState = GameState.Playing;
        TimeController.Instance.UnPause();
        _playerInput.EnableGameplayInput();
        _playerInput.SwitchToFixedUpdateMode();
    }

    private void OnOptionsButtonClick()
    {
        UIInput.Instance.SelectUI(_optionsButton);
        _playerInput.EnablePauseMenuInput();
    }

    private void OnMainMenuButtonClick()
    {
        _menuCanvas.enabled = false;
        SceneLoader.Instance.LoadMainMenuScene();
    }
}
