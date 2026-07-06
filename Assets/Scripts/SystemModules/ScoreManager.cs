using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScoreManager : PersistentSingleton<ScoreManager>
{
    #region Score Display
    public int Score => _score;
    private int _score;
    private int _currentScore;
    private Vector3 _scoreTextScale = new Vector3(1.2f, 1.2f, 1f);

    public void ResetScore()
    {
        _score = 0;
        _currentScore = 0;
        ScoreDisplay.UpdateText(_score);
    }

    public void AddScore(int scorePoint)
    {
        _currentScore += scorePoint;
        StartCoroutine(nameof(AddScoreCoroutine)); 
    }
    
    private IEnumerator AddScoreCoroutine()
    {
        ScoreDisplay.ScaleText(_scoreTextScale);
        while (_score < _currentScore)
        {
            _score += 1;
            ScoreDisplay.UpdateText(_score);
            yield return null;
        }
        ScoreDisplay.ScaleText(Vector3.one);
    }
    #endregion

    #region High Score System
    [System.Serializable]
    public class PlayerScore
    {
        public int Score;
        public string PlayerName;
        public PlayerScore(int score, string playerName)
        {
            Score = score;
            PlayerName = playerName;
        }
    }

    [System.Serializable]
    public class PlayerScoreData
    {
        public List<PlayerScore> PlayerScores = new List<PlayerScore>();
    }
    
    private readonly string SaveFileName = "player_score.json";
    private string _playerName = "No Name";
    public bool HasNewHighScore => _score > LoadPlayerScoreData().PlayerScores[9].Score;

    public void SetPlayerName(string newrName)
    {
        _playerName = newrName;
    }
    
    public void SavePlayerScoreData()
    {
        PlayerScoreData playerScoreData = LoadPlayerScoreData();
        playerScoreData.PlayerScores.Add(new PlayerScore(_score, _playerName));
        playerScoreData.PlayerScores.Sort((x, y) => y.Score.CompareTo(x.Score));
        SaveSystem.Save(SaveFileName, playerScoreData);
    }
    
    public PlayerScoreData LoadPlayerScoreData()
    {
        PlayerScoreData playerScoreData = new PlayerScoreData();
        
        if (SaveSystem.SaveFileExists(SaveFileName))
        {
            playerScoreData = SaveSystem.Load<PlayerScoreData>(SaveFileName);
        }
        else
        {
            while (playerScoreData.PlayerScores.Count < 10)
            {
                playerScoreData.PlayerScores.Add(new PlayerScore(0,_playerName));
            }
            SaveSystem.Save(SaveFileName, playerScoreData);
        }
        return playerScoreData;
    }
    #endregion
}
