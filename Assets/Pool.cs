using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;


public class Pool : MonoBehaviour
{
    public static ObjectPool<GameObject> pool;
    public GameObject type;


    public bool collectionChecks = true;
    public int maxPoolSize = 10;

    private void Awake()
    {
        pool = new ObjectPool<GameObject>(createPooledItem, OnGet, OnReturnedToPool, OnDestroyPoolObject, collectionChecks, 5, 10);
    }

    public static GameObject Get()
    {
        return pool.Get();
    }

    public static GameObject Get(Transform data)
    {
        GameObject gameObject = pool.Get();
        gameObject.transform.position = data.position;
        gameObject.transform.rotation = data.rotation;

        return gameObject;
    }

    
    public GameObject createPooledItem()
    {
        return Instantiate<GameObject>(type);
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
        Destroy(obj);
    }

}
