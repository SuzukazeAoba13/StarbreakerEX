using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ViewPort : Singleton<ViewPort>
{
    private float _minX;
    private float _maxX;
    private float _minY;
    private float _maxY;
    private float _middleX;
    
    public float MaxX => _maxX;

    private void Start()
    {
        Camera mainCamera = Camera.main;
        Vector2 bottomLeft = mainCamera.ViewportToWorldPoint(new Vector3(0, 0));
        Vector2 topRight = mainCamera.ViewportToWorldPoint(new Vector3(1, 1));
        
        _middleX = mainCamera.ViewportToWorldPoint(new Vector3(0.5f, 0f,0f)).x;
        _minX = bottomLeft.x;
        _minY = bottomLeft.y;
        _maxX = topRight.x;
        _maxY = topRight.y;
    }

    public Vector3 PlayerMoveablePosition(Vector3 playerPosition,float paddingX,float paddingY)
    {
        Vector3 position = Vector3.zero;
        position.x = Mathf.Clamp(playerPosition.x, _minX+paddingX, _maxX-paddingX);
        position.y = Mathf.Clamp(playerPosition.y, _minY+paddingY, _maxY-paddingY);
        return  position;
    }

    public Vector3 RandomEnemySpawnPosition(float paddingX,float paddingY)
    {
        Vector3 position = Vector3.zero;
        position.x = _maxX + paddingX;
        position.y = Random.Range(_minY + paddingY, _maxY - paddingY);
        return position;
    }

    public Vector3 RandomRightHalfPosition(float paddingX,float paddingY)
    {
        Vector3 position = Vector3.zero;
        position.x = Random.Range(_middleX, _maxX - paddingX);
        position.y = Random.Range(_minY + paddingY, _maxY - paddingY);
        return position;
    }
    
    public Vector3 RandomEnemyMovePosition(float paddingX,float paddingY)
    {
        Vector3 position = Vector3.zero;
        position.x = Random.Range(_minX + paddingX, _maxX - paddingX);
        position.y = Random.Range(_minY + paddingY, _maxY - paddingY);
        return position;
    }
}
