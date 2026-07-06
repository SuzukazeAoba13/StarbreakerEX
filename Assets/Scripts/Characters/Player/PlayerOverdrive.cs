using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerOverdrive : MonoBehaviour
{
    public static UnityAction on = delegate { };
    public static UnityAction off = delegate { };

    [SerializeField] private GameObject _triggerVFX;
    [SerializeField] private GameObject _engineVFXNormal;
    [SerializeField] private GameObject _engineVFXOverdrive;
    [SerializeField] private AudioData _onSFX;
    [SerializeField] private AudioData _offSFX;
    
    private void Awake()
    {
        on += On;
        off += Off;
    }

    private void OnDestroy()
    {
        on -= On;
        off -= Off;
    }

    private void On()
    {
        _triggerVFX.SetActive(true);
        _engineVFXNormal.SetActive(false);
        _engineVFXOverdrive.SetActive(true);
        AudioManager.Instance.PlayRandomSFX(_onSFX);
    }

    private void Off()
    {
        _engineVFXOverdrive.SetActive(false);
        _engineVFXNormal.SetActive(true);
        AudioManager.Instance.PlayRandomSFX(_offSFX);
    }
}
