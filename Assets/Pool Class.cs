using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PoolHelper
{
    private ObjectPool<GameObject> pool = null;
    private GameObject obj = null;
    public bool collectionChecks = true;
    public int maxPoolSize = 10;

    public PoolHelper(GameObject type)
    {
        obj = type;
        pool = new ObjectPool<GameObject>(createPooledItem, OnGet, OnReturnedToPool, OnDestroyPoolObject, collectionChecks, 5, maxPoolSize);
    }

    public GameObject Get()
    {
        return pool.Get();
    }


    public GameObject Get(Transform data, bool _scale = false)
    {
        GameObject gameObject = pool.Get();
        gameObject.transform.position = data.position;
        gameObject.transform.rotation = data.rotation;

        if(_scale)
            gameObject.transform.localScale = data.localScale;

        return gameObject;
    }


    public void Release(GameObject obj)
    {
        pool.Release(obj);
    }
    
    private GameObject createPooledItem()
    {
        return Object.Instantiate<GameObject>(obj);
    }

    private void OnGet(GameObject obj)
    {
        obj.SetActive(true);
    }

    private void OnReturnedToPool(GameObject obj)
    {
        obj.SetActive(false);
    }

    void OnDestroyPoolObject(GameObject obj)
    {
        Object.Destroy(obj);
    }

}
