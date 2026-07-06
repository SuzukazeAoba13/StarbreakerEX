using System;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    [SerializeField] private Pool[] _enemyPools;
    [SerializeField] private Pool[] _playerProjectilePools;
    [SerializeField] private Pool[] _enemyProjectilePools;
    [SerializeField] private Pool[] _vfxPools;
    [SerializeField] private Pool[] _lootItemPools;
    private static Dictionary<GameObject, Pool> _dictionary;
    
    private void Awake()
    {
        _dictionary = new Dictionary<GameObject, Pool>();
        Initialize(_enemyPools);
        Initialize(_playerProjectilePools);
        Initialize(_enemyProjectilePools);
        Initialize(_vfxPools);
        Initialize(_lootItemPools);
    }

    #if UNITY_EDITOR
    private void OnDestroy()
    {
        CheckPoolSize(_enemyPools);
        CheckPoolSize(_playerProjectilePools);
        CheckPoolSize(_enemyProjectilePools);
        CheckPoolSize(_vfxPools);
        CheckPoolSize(_lootItemPools);
    }
    #endif

    private void CheckPoolSize(Pool[] pools)
    {
        foreach (Pool pool in pools)
        {
            if (pool.RuntimeSize > pool.Size)
            {
                Debug.LogWarning($"对象池{pool.Prefab.name}的实际运行尺寸为:{pool.RuntimeSize} 大于原定尺寸:{pool.Size}");
            }
        }
    }
    
    private void Initialize(Pool[]  pools)
    {
        foreach (Pool pool in pools)
        {
#if UNITY_EDITOR
            if (_dictionary.ContainsKey(pool.Prefab))
            {
                Debug.LogError($"对象池中出现了多个相同的预制体:{pool.Prefab.name}");
                continue;
            }  
#endif
            _dictionary.Add(pool.Prefab, pool);
            Transform poolParent = new GameObject($"Pool{pool.Prefab.name}").transform;
            poolParent.parent = transform;
            pool.Initialize(poolParent);
        }
    }

    public static GameObject Release(GameObject prefab)
    {
#if UNITY_EDITOR
        if (!_dictionary.ContainsKey(prefab))
        {
            Debug.LogError($"对象池管理器找不到该预制体:{prefab.name}");
            return null;
        }
#endif
        return _dictionary[prefab].PreparedObject();
    }
    
    public static GameObject Release(GameObject prefab,Vector3 position)
    {
#if UNITY_EDITOR
        if (!_dictionary.ContainsKey(prefab))
        {
            Debug.LogError($"对象池管理器找不到该预制体:{prefab.name}");
            return null;
        }
#endif
        return _dictionary[prefab].PreparedObject(position);
    }
    
    public static GameObject Release(GameObject prefab,Vector3 position, Quaternion rotation)
    {
#if UNITY_EDITOR
        if (!_dictionary.ContainsKey(prefab))
        {
            Debug.LogError($"对象池管理器找不到该预制体:{prefab.name}");
            return null;
        }
#endif
        return _dictionary[prefab].PreparedObject(position,rotation);
    }
    
    public static GameObject Release(GameObject prefab, Vector3 position, Quaternion rotation,Vector3 localScale)
    {
#if UNITY_EDITOR
        if (!_dictionary.ContainsKey(prefab))
        {
            Debug.LogError($"对象池管理器找不到该预制体:{prefab.name}");
            return null;
        }
#endif
        return _dictionary[prefab].PreparedObject(position, rotation, localScale);
    }
}
