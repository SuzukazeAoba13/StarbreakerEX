using System;
using UnityEngine;
using UnityEngine.UI;

public class WaveUI : MonoBehaviour
{
    private Text _waveText;

    private void Awake()
    {
        GetComponent<Canvas>().worldCamera = Camera.main;
        _waveText = GetComponentInChildren<Text>();
    }

    private void OnEnable()
    {
        _waveText.text = $"- WAVE {EnemyManager.Instance.WaveNumber} -";
    }
}
