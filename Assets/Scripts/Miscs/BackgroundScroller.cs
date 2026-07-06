using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    [SerializeField] private Vector2 _scrollVelocity;
    private Material _material;

    private void Awake()
    {
        _material = GetComponent<Renderer>().material;
    }

    private IEnumerator Start()
    {
        while (GameManager.GameState != GameState.GameOver)
        {
            _material.mainTextureOffset += _scrollVelocity * Time.deltaTime;
            yield return null;
        }
    }
}
