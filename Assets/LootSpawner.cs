using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootSpawner : MonoBehaviour
{
    [SerializeField] private LootSetting[] _lootSettings;

    public void Spawan(Vector2 position)
    {
        foreach (LootSetting lootSetting in _lootSettings)
        {
            lootSetting.Spawn(position + Random.insideUnitCircle);
        }
    }
}
