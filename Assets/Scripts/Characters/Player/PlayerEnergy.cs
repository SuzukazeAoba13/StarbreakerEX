using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEnergy : Singleton<PlayerEnergy>
{
    [SerializeField] private EnergyBar _energyBar;
    [SerializeField] private float _overdriveInterval = 0.1f;
    private bool _available = true;
    public const int MAX = 100;
    public const int PERCENT = 1;
    private int _energy;

    private WaitForSeconds _waitForOverdriveInterval;

    protected override void Awake()
    {
        base.Awake();
        _waitForOverdriveInterval = new WaitForSeconds(_overdriveInterval);
    }

    private void OnEnable()
    {
        PlayerOverdrive.on += PlayerOverdriveOn;
        PlayerOverdrive.off += PlayerOverdriveOff;
    }

    private void OnDisable()
    {
        PlayerOverdrive.on -= PlayerOverdriveOn;
        PlayerOverdrive.off -= PlayerOverdriveOff;
    }

    private void Start()
    {
        _energyBar.Initialize(_energy,MAX);
        Obtain(MAX);
    }

    public void Obtain(int value)
    {
        if (_energy == MAX || !_available || !gameObject.activeSelf)
            return;
        _energy = Mathf.Clamp(_energy+ value, 0, MAX);
        _energyBar.UpdateStats(_energy,MAX);
    }

    public void Use(int value)
    {
        _energy -= value;
        _energyBar.UpdateStats(_energy,MAX);
        if (_energy == 0 && !_available)
        {
            PlayerOverdrive.off.Invoke();
        }
    }
    
    public bool IsEnough(int value) => _energy >= value;

    private void PlayerOverdriveOn()
    {
        _available = false;
        StartCoroutine(nameof(KeepUsingCoroutine));   
    }

    private void PlayerOverdriveOff()
    {
        _available = true;
        StopCoroutine(nameof(KeepUsingCoroutine));   
    }
    
    private IEnumerator KeepUsingCoroutine()
    {
        while (gameObject.activeSelf && _energy > 0)
        {
            yield return _waitForOverdriveInterval;
            Use(PERCENT);
        }
    }
}
