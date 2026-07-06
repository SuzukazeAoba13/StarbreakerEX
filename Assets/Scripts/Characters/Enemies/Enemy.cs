using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    [SerializeField] private int _scorePoint = 100;
    [SerializeField] private int _deathEnergyBouns = 3;
    [SerializeField] protected int _healthFactor;

    private LootSpawner _lootSpawner;

    protected virtual void Awake()
    {
        _lootSpawner = GetComponent<LootSpawner>();
    }

    protected override void OnEnable()
    {
        SetHealth();
        base.OnEnable();
    }

    protected virtual void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.TryGetComponent(out Player player))
        {
            player.Die();
            Die();
        }
    }
    
    public override void Die()
    {
        ScoreManager.Instance.AddScore(_scorePoint);
        PlayerEnergy.Instance.Obtain(_deathEnergyBouns);
        EnemyManager.Instance.RemoveFromList(gameObject);
        _lootSpawner.Spawan(transform.position);
        base.Die();
    }

    protected virtual void SetHealth()
    {
        _maxHealth += (int)(EnemyManager.Instance.WaveNumber / _healthFactor);
    }
}
