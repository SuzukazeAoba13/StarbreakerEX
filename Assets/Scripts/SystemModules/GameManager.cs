using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : PersistentSingleton<GameManager>
{
    public static UnityAction onGameOver;
    
    public static GameState GameState
    {
        get => Instance._gameState;
        set => Instance._gameState = value;
    }
    [SerializeField] private GameState _gameState = GameState.Playing;
}

public enum GameState
{
    Playing,
    Paused,
    GameOver,
    Scoring
}
