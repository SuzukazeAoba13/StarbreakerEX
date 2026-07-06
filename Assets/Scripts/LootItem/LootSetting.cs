using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LootSetting
{
    public GameObject Prefab;
    [Range(0f,100f)] public float DropPercentage;

    public void Spawn(Vector3 position)
    {
        if (Random.Range(0, 100f) <= DropPercentage)
        {
            PoolManager.Release(Prefab, position);
        }
    }
}
