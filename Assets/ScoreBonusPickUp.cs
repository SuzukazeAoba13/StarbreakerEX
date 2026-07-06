using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreBonusPickUp : LootItem
{
    [SerializeField] private int _scoreBonus;
    
    protected override void PickUp()
    {
        ScoreManager.Instance.AddScore(_scoreBonus);
        base.PickUp();
    }
}
