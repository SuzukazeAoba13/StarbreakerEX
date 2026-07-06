using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Pool
{
    public GameObject Prefab => _prefab;
    public int Size => _size;
    public int RuntimeSize => _quque.Count;
    [SerializeField] private GameObject _prefab;
    [SerializeField] private int _size = 1;
    private Queue<GameObject> _quque;
    private Transform _parent;
    
    public void Initialize(Transform parent)
    {
        _quque = new Queue<GameObject>();
        _parent = parent;
        for (int i = 0; i < _size; i++)
        {
            _quque.Enqueue(Copy());
        }
    }
    
    private GameObject Copy()
    {
        GameObject copy = GameObject.Instantiate(_prefab,_parent);
        copy.SetActive(false);
        return copy;
    }

    private GameObject AvaiableObject()
    {
        GameObject avaiableObject = null;
        if (_quque.Count > 0 && !_quque.Peek().activeSelf)
            avaiableObject = _quque.Dequeue();
        else
        {
            avaiableObject = Copy();
        }
        _quque.Enqueue(avaiableObject);
        return avaiableObject;
    }

    public GameObject PreparedObject()
    {
        GameObject preparedObject = AvaiableObject();
        preparedObject.SetActive(true);
        return preparedObject;
    }
    
    public GameObject PreparedObject(Vector3 position)
    {
        GameObject preparedObject = AvaiableObject();
        preparedObject.transform.position = position;
        preparedObject.SetActive(true);
        return preparedObject;
    }

    public GameObject PreparedObject(Vector3 position, Quaternion rotation)
    {
        GameObject preparedObject = AvaiableObject();
        preparedObject.SetActive(true);
        preparedObject.transform.position = position;
        preparedObject.transform.rotation = rotation;
        return preparedObject;
    }
    
    public GameObject PreparedObject(Vector3 position, Quaternion rotation,Vector3 localScale)
    {
        GameObject preparedObject = AvaiableObject();
        preparedObject.SetActive(true);
        preparedObject.transform.position = position;
        preparedObject.transform.rotation = rotation;
        preparedObject.transform.localScale = localScale;
        return preparedObject;
    }
}
