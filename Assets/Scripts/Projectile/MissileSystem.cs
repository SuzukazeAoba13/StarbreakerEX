using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileSystem : MonoBehaviour
{
    [SerializeField] private int _defaultAmount = 5;
    [SerializeField] private float _cooldownTime = 2f;
    [SerializeField] private GameObject _missilePrefab;
    [SerializeField] private AudioData _launchSFX;
    
    private bool _isReady = true;
    private int _amount;

    private void Awake()
    {
        _amount = _defaultAmount;
    }

    private void Start()
    {
        MissileDisplay.UpdateAmountText(_amount);
    }

    public void PickUp()
    {
        _amount++;
        MissileDisplay.UpdateAmountText(_amount);

        if (_amount == 1)
        {
            MissileDisplay.UpdateCooldownImage(0f);
            _isReady = true;
        }
    }
    
    public void Launch(Transform muzzleTransform)
    {
        if (_amount == 0 || !_isReady) return;
        
        _isReady = false;
        PoolManager.Release(_missilePrefab,muzzleTransform.position);
        AudioManager.Instance.PlayRandomSFX(_launchSFX);
        _amount--;
        MissileDisplay.UpdateAmountText(_amount);
        if (_amount == 0)
        {
            MissileDisplay.UpdateCooldownImage(1);
        }
        else
        {
            StartCoroutine(nameof(CoolDownCoroutine));
        }
    }
    
    private IEnumerator CoolDownCoroutine()
    {
        float cooldownValue = _cooldownTime;
        while (cooldownValue > 0)
        {
            MissileDisplay.UpdateCooldownImage(cooldownValue / _cooldownTime);
            cooldownValue = Mathf.Max(cooldownValue-Time.unscaledDeltaTime,0f);
            yield return null;
        }
        _isReady = true;
    }
}
