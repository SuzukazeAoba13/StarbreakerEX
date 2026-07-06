using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class ScoringUIController : MonoBehaviour
{
    [Header("---- Background ----")]
    [SerializeField] private Image _background;
    [SerializeField] private Sprite[] _backgroundImages;

    [Header("---- Scoring Screen ----")] 
    [SerializeField] private Canvas _scoringScreenCanvas;
    [SerializeField] private Text _playerScoreText;
    [SerializeField] private Button _mainMenuButton;
    [SerializeField] private Transform _highScoreLeaderboardContainer;
    
    [Header("---- High Score Screen----")]
    [SerializeField] private Canvas _newHighScoreScreenCanvas;
    [SerializeField] private Button _cancelButton;
    [SerializeField] private Button _submitButton;
    [SerializeField] private InputField _playerNameInpputField;
    
    private void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        ShowRandomBackground();
        if (ScoreManager.Instance.HasNewHighScore)
        {
            ShowNewHighScoreScreen();
        }
        else
        {
            ShowScoringScreen();
        }
        ButtonPressedBehaviour.ButtonFunctionTable.Add(_mainMenuButton.gameObject.name,OnMainMenuButtonClicked);
        ButtonPressedBehaviour.ButtonFunctionTable.Add(_submitButton.gameObject.name,OnSubmitButtonClicked);
        ButtonPressedBehaviour.ButtonFunctionTable.Add(_cancelButton.gameObject.name,HideNewHighScoreScreen);

        GameManager.GameState = GameState.Scoring;
    }

    private void OnDisable()
    {
        ButtonPressedBehaviour.ButtonFunctionTable.Clear();
    }

    private void ShowRandomBackground() => _background.sprite = _backgroundImages[Random.Range(0, _backgroundImages.Length)];

    private void ShowNewHighScoreScreen()
    {
        _newHighScoreScreenCanvas.enabled = true;
        UIInput.Instance.SelectUI(_cancelButton);
    }

    private void HideNewHighScoreScreen()
    {
        _newHighScoreScreenCanvas.enabled = false;
        ScoreManager.Instance.SavePlayerScoreData();
        ShowRandomBackground();
        ShowScoringScreen();
    }
    
    private void ShowScoringScreen()
    {
        _scoringScreenCanvas.enabled = true;
        _playerScoreText.text = ScoreManager.Instance.Score.ToString();
        UIInput.Instance.SelectUI(_mainMenuButton);
        UpdateHighScoreLeaderboard();
    }

    private void UpdateHighScoreLeaderboard()
    {
       var playerScoreList = ScoreManager.Instance.LoadPlayerScoreData().PlayerScores;
       for (int i = 0; i < _highScoreLeaderboardContainer.childCount; i++)
       {
           Transform child = _highScoreLeaderboardContainer.GetChild(i);
           child.Find("Rank").GetComponent<Text>().text = (i+1).ToString();
           child.Find("Score").GetComponent<Text>().text = playerScoreList[i].Score.ToString();
           child.Find("Name").GetComponent<Text>().text = playerScoreList[i].PlayerName;
       }
    }
    
    private void OnMainMenuButtonClicked()
    {
        _scoringScreenCanvas.enabled = false;
        SceneLoader.Instance.LoadMainMenuScene();
    }

    private void OnSubmitButtonClicked()
    {
        if (!string.IsNullOrEmpty(_playerNameInpputField.text))
        {
            ScoreManager.Instance.SetPlayerName(_playerNameInpputField.text);
        }

        HideNewHighScoreScreen();
    }
}
