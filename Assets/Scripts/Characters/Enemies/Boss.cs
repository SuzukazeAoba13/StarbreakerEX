using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{
    private BossHealthBar _healthBar;
    private Canvas _healthBarCanvas;

    protected override void Awake()
    {
        base.Awake();
        _healthBar = FindObjectOfType<BossHealthBar>();
        _healthBarCanvas = _healthBar.GetComponentInChildren<Canvas>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        _healthBar.Initialize(_health,_maxHealth);
        _healthBarCanvas.enabled = true;
    }

    protected override void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.TryGetComponent(out Player player))
        {
            player.Die();
        }
    }

    public override void Die()
    {
        _healthBarCanvas.enabled = false;
        base.Die();
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        _healthBar.UpdateStats(_health, _maxHealth);
    }

    protected override void SetHealth()
    {
        _maxHealth += EnemyManager.Instance.WaveNumber * _healthFactor;
    }
}
