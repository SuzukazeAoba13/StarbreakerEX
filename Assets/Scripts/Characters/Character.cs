using UnityEngine;
using System.Collections;

public class Character : MonoBehaviour
{
    [Header("---- Death ----")]
    [SerializeField] private GameObject _deathVFX;
    [SerializeField] private AudioData[] _deathSFX;
    
    [Header("---- Health ----")]
    [SerializeField] protected float _maxHealth;
    [SerializeField] protected bool _showOnHeadHealthBar = true;
    [SerializeField] protected StatsBar _onHeadHealthBar;
    protected float _health;

    protected virtual void OnEnable()
    {
        _health = _maxHealth;
        if (_showOnHeadHealthBar)
            ShowOnHeadHealthBar();
        else
            HideOnHeadHealthBar();
    }

    public void ShowOnHeadHealthBar()
    {
        _onHeadHealthBar.gameObject.SetActive(true);
        _onHeadHealthBar.Initialize(_health, _maxHealth);
    }

    public void HideOnHeadHealthBar()
    {
        _onHeadHealthBar.gameObject.SetActive(false);
    }
    
    public virtual void TakeDamage(float damage)
    {
        if (_health == 0f) return;
        _health -= damage;
        if (_showOnHeadHealthBar)
            _onHeadHealthBar.UpdateStats(_health, _maxHealth);
        
        if (_health <= 0f)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        _health = 0f;
        AudioManager.Instance.PlayRandomSFX(_deathSFX);
        PoolManager.Release(_deathVFX,transform.position);
        gameObject.SetActive(false);
    }
    
    public virtual void RestoreHealth(float value)
    {
        if(_health == _maxHealth)
            return;
        //_health += value;
        _health = Mathf.Clamp(_health + value, 0f, _maxHealth);
        if (_showOnHeadHealthBar)
            _onHeadHealthBar.UpdateStats(_health, _maxHealth);
    }

    protected IEnumerator HealthRegenerateCoroutine(WaitForSeconds waitTime,float percent)
    {
        while (_health < _maxHealth)
        {
            yield return waitTime;
            RestoreHealth(_maxHealth * percent);
        }
    }
    
    protected IEnumerator DamageOverTimeCoroutine(WaitForSeconds waitTime,float percent)
    {
        while (_health > 0)
        {
            yield return waitTime;
            TakeDamage(_maxHealth * percent);
        }
    }
}
