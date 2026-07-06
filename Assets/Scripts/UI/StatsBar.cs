using System;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class StatsBar : MonoBehaviour
{
    [SerializeField] private Image _fillImageBack;
    [SerializeField] private Image _fillImageFront;
    [SerializeField] private float _fillSpeed = 0.1f;
    [SerializeField] private bool _delayFill = true;
    [SerializeField] private float _fillDelay = 0.5f;
    private float _currentFillAmount;
    protected float _targetFillAmount;
    private float _previousFillAmount;
    private float _t;

    private WaitForSeconds _waitForDelayFill;
    private Coroutine _bufferedFillingCoroutine; 
    
    private void Awake()
    {
        if (TryGetComponent(out Canvas canvas))
        {
            canvas.worldCamera = Camera.main;
        }
        
        _waitForDelayFill = new WaitForSeconds(_fillDelay);
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    public virtual void Initialize(float currentValue, float maxValue)
    {
        _currentFillAmount = currentValue / maxValue;
        _targetFillAmount = _currentFillAmount;
        _fillImageBack.fillAmount = _currentFillAmount;
        _fillImageFront.fillAmount = _currentFillAmount;
    }

    public void UpdateStats(float currentValue, float maxValue)
    {
        _targetFillAmount = currentValue / maxValue;
        if (_bufferedFillingCoroutine != null)
            StopCoroutine(_bufferedFillingCoroutine);
        
        if (_currentFillAmount > _targetFillAmount)
        {
            _fillImageFront.fillAmount = _targetFillAmount;
            _bufferedFillingCoroutine = StartCoroutine(BufferedFillingCoroutine(_fillImageBack));
            return;
        }

        if (_currentFillAmount < _targetFillAmount)
        {
            _fillImageBack.fillAmount = _targetFillAmount;
            _bufferedFillingCoroutine = StartCoroutine(BufferedFillingCoroutine(_fillImageFront));
        }
    }

    protected virtual IEnumerator BufferedFillingCoroutine(Image image)
    {
        if (_delayFill)
        {
            yield return _waitForDelayFill;
        }

        _previousFillAmount = _currentFillAmount;
        _t = 0;
        while (_t < 1)
        {
            _t += Time.deltaTime * _fillSpeed;
            _currentFillAmount = Mathf.Lerp(_previousFillAmount, _targetFillAmount, _t);
            image.fillAmount = _currentFillAmount;
            yield return null;
        }
    }
}
