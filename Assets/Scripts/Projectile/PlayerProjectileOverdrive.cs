using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectileOverdrive : PlayerProjectile
{
    [SerializeField] private ProjectileGuidanceSystem _guidanceSystem;
    
    protected override void OnEnable()
    {
        SetTarget(EnemyManager.Instance.RandomEnemy);
        transform.rotation = Quaternion.identity;
        if (_target == null)
        {
            base.OnEnable();
        }
        else
        {
            StartCoroutine(_guidanceSystem.HomingCoroutine(_target));
        }
    }
}
