using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverScreen : MonoBehaviour
{
    [SerializeField] private PlayerInput _input;
    [SerializeField] private Canvas _hudCanvas;
    [SerializeField] private AudioData _confirmGameOverSound;
    private int _exitStateID = Animator.StringToHash("GameOverScreenExit");
    
    private Canvas _canvas;
    private Animator _animator;

    private void Awake()
    {
        _canvas = GetComponent<Canvas>();
        _animator = GetComponent<Animator>();
        
        _canvas.enabled = false;
        _animator.enabled = false;
    }

    private void OnEnable()
    {
        GameManager.onGameOver += OnGameOver;
        _input.onConfirmGameOver += OnConfirmGameOver;
    }

    private void OnDisable()
    {
        GameManager.onGameOver -= OnGameOver;
        _input.onConfirmGameOver -= OnConfirmGameOver;
    }

    private void OnGameOver()
    {
        _hudCanvas.enabled = false;
        _canvas.enabled = true;
        _animator.enabled = true;
        _input.DisableAllInput();
    }

    private void OnConfirmGameOver()
    {
        AudioManager.Instance.PlaySFX(_confirmGameOverSound);
        _input.DisableAllInput();
        _animator.Play(_exitStateID);
        SceneLoader.Instance.LoadScoringScene();
    }
    
    private void EnableGameOverScreenInput()
    {
        _input.EnableGameOverScreenInput();
    }
}
