using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ObjectPoolT<T> : MonoBehaviour where T : MonoBehaviour
{

    public T _prefab;
    [SerializeField]
    protected int _poolSize = 10;
    private List<T> _pool;

    private void Awake()
    {
        _pool = new List<T>();

        for(int i = 0; i< _poolSize; i++)
        {
            T obj = InstantiateObject();
            obj.gameObject.SetActive(false);
        }
        print(_pool.Count);
    }

    protected virtual void OnDisable()
    {
        foreach(T item in _pool)
        {
            item.gameObject.SetActive(false);
        }
    }

    public T GetPooledObject()
    {
        T result = null;
        print(_pool.Count);
        foreach(T obj in _pool)
        {
            if (obj == null) continue;
            if (!obj.gameObject.activeInHierarchy)
            {
                result = obj;
                break;
            }
        }

        if (result == null)
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
        T obj = Instantiate(_prefab, transform.position, transform.rotation);
        _pool.Add(obj);
        return obj;
    }
}
