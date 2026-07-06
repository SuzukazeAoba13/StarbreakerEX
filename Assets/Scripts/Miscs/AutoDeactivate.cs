using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDeactivate : MonoBehaviour
{
    [SerializeField] private bool _destroyGameObject;
    [SerializeField] private float _lifeTime = 3f;
    private WaitForSeconds _waitLifeTime;

    private void Awake()
    {
        _waitLifeTime = new WaitForSeconds(_lifeTime);
    }

    private void OnEnable()
    {
        StartCoroutine(nameof(DeactivateCoroutine));
    }

    IEnumerator DeactivateCoroutine()
    {
        yield return _waitLifeTime;
        if (_destroyGameObject)
        {
            Destroy(gameObject);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
