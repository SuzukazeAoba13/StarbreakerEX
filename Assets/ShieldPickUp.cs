using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldPickUp : LootItem
{
    [SerializeField] private AudioData _fullHealthPickUpSFX;
    [SerializeField] private int _fullHealthScoreBonus = 200;
    [SerializeField] private float _shieldBonus = 20f;
    
    protected override void PickUp()
    {
        if (_player.IsFullHealth)
        {
            _pickUpSFX = _fullHealthPickUpSFX;
            _lootMessage.text = $"SCORE + {_fullHealthScoreBonus}";
            ScoreManager.Instance.AddScore(_fullHealthScoreBonus);
        }
        else
        {
            _pickUpSFX = _defaultPickUpSFX;
            _lootMessage.text = $"SHIELD + {_shieldBonus}";
            _player.RestoreHealth(_shieldBonus);
        }
        base.PickUp();
    }
}
