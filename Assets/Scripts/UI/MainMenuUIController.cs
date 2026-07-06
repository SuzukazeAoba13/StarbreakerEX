using System;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUIController : MonoBehaviour
{
    [Header("---- Canvas ----")] 
    [SerializeField] private Canvas _mainMenuCanvas;
    
    [Header("---- Buttons ----")]
    [SerializeField] private Button _startButton;
    [SerializeField] private Button _optionsButton;
    [SerializeField] private Button _quitButton;

    private void OnEnable()
    {
        ButtonPressedBehaviour.ButtonFunctionTable.Add(_startButton.gameObject.name, OnStartButtonClick);
        ButtonPressedBehaviour.ButtonFunctionTable.Add(_optionsButton.gameObject.name, OnOptionsButtonClick);
        ButtonPressedBehaviour.ButtonFunctionTable.Add(_quitButton.gameObject.name, OnQuitButtonClick);
    }

    private void OnDisable()
    {
        ButtonPressedBehaviour.ButtonFunctionTable.Clear();
    }

    private void Start()
    {
        Time.timeScale = 1;
        GameManager.GameState = GameState.Playing;
        UIInput.Instance.SelectUI(_startButton);
    }

    private void OnStartButtonClick()
    {
        _mainMenuCanvas.enabled = false;
        SceneLoader.Instance.LoadGamePlayScene();
    }

    private void OnOptionsButtonClick()
    {
        UIInput.Instance.SelectUI(_optionsButton);
    }
    
    private void OnQuitButtonClick()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); 
#endif
    }
}
