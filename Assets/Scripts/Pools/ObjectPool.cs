using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ObjectPoolT<T> : MonoBehaviour where T : MonoBehaviour
{

    public T _prefab;
    [SerializeField]
    protected int _poolSize = 10;
    private List<T> _pool;
    public bool _willGrow = true;

    private void Awake()
    {
        _pool = new List<T>();

        for(int i = 0; i< _poolSize; i++)
        {
            T obj = InstantiateObject();
            obj.gameObject.SetActive(false);
        }
    }

    protected virtual void OnDisable()
    {
        foreach(T item in _pool)
        {
            if (item == null) continue;
            item.gameObject.SetActive(false);
        }
    }

    public T GetPooledObject()
    {
        T result = null;
        foreach(T obj in _pool)
        {
            if (obj == null) { Debug.Log("NULL!"); continue; } 
            if (!obj.gameObject.activeInHierarchy)
            {
                result = obj;
                break;
            }
        }

        if (result == null && _willGrow)
        {
            result = InstantiateObject();
        }

        if (result != null)
        {
            result.gameObject.SetActive(true);
        }

        return result;
    }

    private T InstantiateObject()
    {
        T obj = Instantiate(_prefab, transform.position, transform.rotation, transform);
        _pool.Add(obj);
        return obj;
    }
}
