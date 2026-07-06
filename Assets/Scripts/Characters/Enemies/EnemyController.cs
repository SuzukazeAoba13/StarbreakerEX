using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyController : MonoBehaviour
{
    [Header("---- Move ----")]
    [SerializeField] private float _moveSpeed = 2f;
    [SerializeField] private float _moveRotationAngle = 15f;
    protected float _paddingX;
    protected float _paddingY;
    protected Vector3 _targetPosition;
    
    [Header("---- Fire ----")]
    [SerializeField] protected GameObject[] _projectiles;
    [SerializeField] protected AudioData[] _projectileLanuchSFXs;
    [SerializeField] protected Transform _muzzle;
    [SerializeField] protected ParticleSystem _muzzleVFX;
    [SerializeField] protected float _minFireInterval = 1.5f;
    [SerializeField] protected float _maxFireInterval = 3f;
    private WaitForFixedUpdate _waitForFixedUpdate;

    protected virtual void Awake()
    {
        Vector3 size = transform.GetChild(0).GetComponent<Renderer>().bounds.size;
        _paddingX = size.x * 0.5f;
        _paddingY = size.y * 0.5f;
        
        _waitForFixedUpdate = new WaitForFixedUpdate();
    }

    protected virtual void OnEnable()
    {
        StartCoroutine(nameof(RandomlyMovingCorutine));
        StartCoroutine(nameof(RandomlyFireCorutine));
    }

    private void OnDisable()
    {
        StopCoroutine(nameof(RandomlyMovingCorutine));
        StopCoroutine(nameof(RandomlyFireCorutine));
    }

    private IEnumerator RandomlyMovingCorutine()
    {
        transform.position = ViewPort.Instance.RandomEnemySpawnPosition(_paddingX, _paddingY);
        _targetPosition = ViewPort.Instance.RandomRightHalfPosition(_paddingX, _paddingY);
        while (gameObject.activeSelf)
        {
            //if (Vector3.Distance(transform.position, targetPosition) > Mathf.Epsilon * Mathf.Epsilon)
            if ((transform.position - _targetPosition).sqrMagnitude >= (_moveSpeed * Time.fixedDeltaTime) * (_moveSpeed * Time.fixedDeltaTime))
            {
                transform.position = Vector3.MoveTowards(transform.position, _targetPosition, _moveSpeed * Time.fixedDeltaTime);
                transform.rotation = Quaternion.AngleAxis((_targetPosition-transform.position).normalized.y * _moveRotationAngle,Vector3.right);
            }
            else
            {
                _targetPosition = ViewPort.Instance.RandomRightHalfPosition(_paddingX, _paddingY);
            }
            yield return _waitForFixedUpdate;
        }
    }

    protected virtual IEnumerator RandomlyFireCorutine()
    {
        while (gameObject.activeSelf)
        {
            yield return new WaitForSeconds(Random.Range(_minFireInterval, _maxFireInterval));
            if (GameManager.GameState == GameState.GameOver)
                yield break;
            foreach (GameObject projectile in _projectiles)
            {
                PoolManager.Release(projectile,_muzzle.position);
            }
            AudioManager.Instance.PlayRandomSFX(_projectileLanuchSFXs);
            _muzzleVFX.Play();
        }
    }
}
