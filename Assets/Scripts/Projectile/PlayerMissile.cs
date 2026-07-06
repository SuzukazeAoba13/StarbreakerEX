using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMissile : PlayerProjectileOverdrive
{
    [SerializeField] private AudioData _targetAcquiredVoice;
    [Header("---- Speed Change ----")]
    [SerializeField] private float _lowSpeed = 8f;
    [SerializeField] private float _highSpeed = 25f;
    [SerializeField] private float _variablesSpeedDelay = 0.5f;

    [Header("---- Explosion ----")] 
    [SerializeField] private GameObject _explosionVFX;
    [SerializeField] private AudioData _explosionSFX;
    [SerializeField] private float _explosionRadius = 3f;
    [SerializeField] private LayerMask _enemyLayerMask;
    [SerializeField] private float _explosionDamage = 100f;
    
    private WaitForSeconds _waitVariablesSpeedDelay;

    protected override void Awake()
    {
        base.Awake();
        _waitVariablesSpeedDelay = new WaitForSeconds(_variablesSpeedDelay);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        StartCoroutine(nameof(VariableSpeedCoroutine));
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
        PoolManager.Release(_explosionVFX,transform.position);
        AudioManager.Instance.PlayRandomSFX(_explosionSFX);
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, _explosionRadius, _enemyLayerMask);
        foreach (Collider2D collder in colliders)
        {
            if (collder.TryGetComponent(out Enemy enemy))
            {
                enemy.TakeDamage(_explosionDamage);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _explosionRadius);
    }

    private IEnumerator VariableSpeedCoroutine()
    {
        _moveSpeed = _lowSpeed;
        yield return _waitVariablesSpeedDelay;
        _moveSpeed = _highSpeed;
        if (_target != null)
        {
            AudioManager.Instance.PlayRandomSFX(_targetAcquiredVoice);
        }
    }
}
