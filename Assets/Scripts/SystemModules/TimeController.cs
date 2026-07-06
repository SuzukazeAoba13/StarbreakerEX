using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeController : Singleton<TimeController>
{
    [SerializeField,Range(0f,1f)] private float _bulletTimeScale = 0.1f;
    private float _defaultFixedDeltaTime;
    private float _timeScaleBeforePause;
    private float _t;
    
    protected override void Awake()
    {
        base.Awake();
        _defaultFixedDeltaTime = Time.fixedDeltaTime;
    }

    public void Pause()
    {
        _timeScaleBeforePause = Time.timeScale;
        Time.timeScale = 0f;
    }

    public void UnPause()
    {
        Time.timeScale = _timeScaleBeforePause;
    }
    
    public void BulletTime(float duration)
    {
        Time.timeScale = _bulletTimeScale;
        StartCoroutine(SlowOutCoroutine(duration));
    }
    
    public void BulletTime(float inDuration, float outDuration)
    {
        StartCoroutine(SlowInAndOutCoroutine(inDuration, outDuration));
    }
    
    public void BulletTime(float inDuration, float keepingDuration, float outDuration)
    {
        StartCoroutine(SlowInKeepAndOutCoroutine(inDuration, keepingDuration,outDuration));
    }

    public void SlowIn(float duration)
    {
        StartCoroutine(SlowInCoroutine(duration));
    }

    public void SlowOut(float duration)
    {
        StartCoroutine(SlowOutCoroutine(duration));
    }

    private IEnumerator SlowInKeepAndOutCoroutine(float inDuration, float keepingDuration, float outDuration)
    {
        yield return StartCoroutine(SlowInCoroutine(inDuration));
        yield return new WaitForSecondsRealtime(keepingDuration);
        StartCoroutine(SlowOutCoroutine(outDuration));
    }
    
    private IEnumerator SlowInAndOutCoroutine(float inDuration, float outDuration)
    {
        yield return StartCoroutine(SlowInCoroutine(inDuration));
        StartCoroutine(SlowOutCoroutine(outDuration));
    }
    
    private IEnumerator SlowInCoroutine(float duration)
    {
        _t = 0f;
        while (_t < 1f)
        {
            if (GameManager.GameState != GameState.Paused)
            {
                _t += Time.unscaledDeltaTime / duration;
                Time.timeScale = Mathf.Lerp(1, _bulletTimeScale,_t);
                Time.fixedDeltaTime = _defaultFixedDeltaTime * _bulletTimeScale;
            }
            yield return null;
        } 
    }
    
    private IEnumerator SlowOutCoroutine(float duration)
    {
        _t = 0f;
        while (_t < 1f)
        {
            if (GameManager.GameState != GameState.Paused)
            {
                _t += Time.unscaledDeltaTime / duration;
                Time.timeScale = Mathf.Lerp(_bulletTimeScale, 1,_t);
                Time.fixedDeltaTime = _defaultFixedDeltaTime * _bulletTimeScale;
            }
            yield return null;
        } 
    }
}
