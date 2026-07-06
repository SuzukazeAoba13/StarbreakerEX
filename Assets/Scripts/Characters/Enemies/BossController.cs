using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BossController : EnemyController
{
    [SerializeField] private float _continuousFireDuration = 1.5f;

    [Header("---- Player Detection ----")] 
    [SerializeField] private Transform _playerDetectionTrans;
    [SerializeField] private Vector3 _playerDetectionSize;
    [SerializeField] private LayerMask _playerLayer;

    [Header("---- Beam ----")] 
    [SerializeField] private float _beamCooldownTime = 12f;
    [SerializeField] private AudioData _beamChargingSFX;
    [SerializeField] private AudioData _beamLaunchSFX;
    
    private bool _isBeamReady;
    
    private int _launchBeamID = Animator.StringToHash("launchBeam");
    
    private WaitForSeconds _waitForContinuousFireInterval;
    private WaitForSeconds _waitForFireInterval;
    private WaitForSeconds _waitBeamCooldownTime;
    private List<GameObject> _magazine;
    private AudioData _launchSFX;
    private Animator _animator;
    private Transform _playerTransform;
    
    protected override void Awake()
    {
        base.Awake();
        _waitForContinuousFireInterval = new WaitForSeconds(_minFireInterval);
        _waitForFireInterval = new WaitForSeconds(_maxFireInterval);
        _waitBeamCooldownTime = new WaitForSeconds(_beamCooldownTime);
        _animator =  GetComponent<Animator>();
        _magazine = new List<GameObject>(_projectiles.Length);
        
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    protected override void OnEnable()
    {
        _isBeamReady = false;
        _muzzleVFX.Stop();
        StartCoroutine(nameof(BeamCooldownCoroutine));
        base.OnEnable();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(_playerDetectionTrans.position, _playerDetectionSize);
    }

    private void ActivateBeamWeapon()
    {
        _isBeamReady = false;
        _animator.SetTrigger(_launchBeamID);
        AudioManager.Instance.PlayRandomSFX(_beamChargingSFX);
    }

    private void AnimationEventLaunchBeam()
    {
        AudioManager.Instance.PlayRandomSFX(_beamLaunchSFX);
    }

    private void AnimationEventStopBeam()
    {
        StopCoroutine(nameof(ChasingPlayerCoroutine));
        StartCoroutine(nameof(BeamCooldownCoroutine));
        StartCoroutine(nameof(RandomlyFireCorutine));
    }
    
    private void LoadProjectile()
    {
        _magazine.Clear();
        if (Physics2D.OverlapBox(_playerDetectionTrans.position, _playerDetectionSize, 0f, _playerLayer))
        {
            _magazine.Add(_projectiles[0]);
            _launchSFX = _projectileLanuchSFXs[0];
        }
        else
        {
            if (Random.value < 0.5f)
            {
                _magazine.Add(_projectiles[1]);
                _launchSFX = _projectileLanuchSFXs[1];
            }
            else
            {
                for (int i = 2; i < _projectiles.Length; i++)
                {
                    _magazine.Add(_projectiles[i]);
                }
                _launchSFX = _projectileLanuchSFXs[2];
            }
        }
    }
    
    protected override IEnumerator RandomlyFireCorutine()
    {
        while (isActiveAndEnabled)
        {
            if (GameManager.GameState == GameState.GameOver) yield break;
            
            if (_isBeamReady)
            {
                ActivateBeamWeapon();
                StartCoroutine(nameof(ChasingPlayerCoroutine));
                yield break;
            }
            yield return _waitForFireInterval;
            yield return StartCoroutine(nameof(ContinuousFireCoroutine));
        }
    }

    private IEnumerator ContinuousFireCoroutine()
    {
        LoadProjectile();
        _muzzleVFX.Play();
        
        float continuousFireTimer = 0f;
        while (continuousFireTimer < _continuousFireDuration)
        {
            foreach (GameObject projectile in _magazine)
            {
                PoolManager.Release(projectile, _muzzle.position);
            }

            continuousFireTimer += _minFireInterval;
            AudioManager.Instance.PlayRandomSFX(_launchSFX);
            
            yield return new WaitForSeconds(_minFireInterval);
        }
        
        _muzzleVFX.Stop();
    }

    private IEnumerator BeamCooldownCoroutine()
    {
        yield return _waitBeamCooldownTime;
        _isBeamReady = true;
    }

    private IEnumerator ChasingPlayerCoroutine()
    {
        while (isActiveAndEnabled)
        {
            _targetPosition.x = ViewPort.Instance.MaxX - _paddingX;
            _targetPosition.y = _playerTransform.position.y;
            yield return null;
        }
    }
}
