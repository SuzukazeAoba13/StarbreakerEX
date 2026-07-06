using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectileAiming : Projectile
{
    private void Awake()
    {
        SetTarget(GameObject.FindGameObjectWithTag("Player"));
    }

    protected override void OnEnable()
    {
        StartCoroutine(nameof(MoveDirectionCoroutine));
        base.OnEnable();
    }
    
    private IEnumerator MoveDirectionCoroutine()
    {
        yield return null;
        if (_target.activeSelf)
        {
            _moveDirection = (_target.transform.position - transform.position).normalized;
        }
    }
}
