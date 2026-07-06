using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private GameObject _hitVFX;
    [SerializeField] AudioData[] _hitSFXs;
    [SerializeField] private float _damage;
    [SerializeField] protected float _moveSpeed = 10f;
    [SerializeField] protected Vector2 _moveDirection;
    protected GameObject _target;
    
    protected virtual void OnEnable()
    {
        StartCoroutine(nameof(MoveDirectlyCoroutine));
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<Character>(out Character character))
        {
            character.TakeDamage(_damage);
            // ContactPoint2D contactPoint = collision.GetContact(0);
            // PoolManager.Release(_hitVFX,contactPoint.point,Quaternion.LookRotation(contactPoint.normal),Vector3.one);
            PoolManager.Release(_hitVFX,collision.GetContact(0).point,Quaternion.LookRotation(collision.GetContact(0).normal));
            AudioManager.Instance.PlayRandomSFX(_hitSFXs);
            gameObject.SetActive(false);
        }
    }
    
    private IEnumerator MoveDirectlyCoroutine()
    {
        while (gameObject.activeSelf)
        {
            Move();
            yield return null;
        }
    }

    protected void SetTarget(GameObject target) => _target = target;
    
    public void Move() => transform.Translate(_moveDirection * _moveSpeed * Time.deltaTime);
}
