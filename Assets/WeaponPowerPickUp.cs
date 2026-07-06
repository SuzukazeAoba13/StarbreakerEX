using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPowerPickUp : LootItem
{
    [SerializeField] private AudioData _fullPowerPickUpSFX;
    [SerializeField] private int _fullPowerScoreBonus = 200;
    
    protected override void PickUp()
    {
        if (_player.IsFullPower)
        {
            _pickUpSFX = _fullPowerPickUpSFX;
            _lootMessage.text = $"SCORE + {_fullPowerScoreBonus}";
            ScoreManager.Instance.AddScore(_fullPowerScoreBonus);
        }
        else
        {
            _pickUpSFX = _defaultPickUpSFX;
            _player.PowerUp();
            _lootMessage.text = "POWER UP!";
        }
        base.PickUp();
    }
}
