using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ObjectPooler : Singleton<ObjectPooler>
{
    public event Action OnDeSpawningAllFighters = delegate { };

    [System.Serializable]
    public class ObjectToPool
    {
        public GameObject objectHolder;
        public string name;
        public GameObject prefab;
        public int capacity;
    }

    public List<ObjectToPool> pools;
    public Dictionary<string, Queue<GameObject>> pooler;

    public Dictionary<string, Queue<GameObject>> Pooler { get => pooler; set => pooler = value; }

    public void Awake()
    {
        pooler = new Dictionary<string, Queue<GameObject>>();
        foreach (ObjectToPool pool in pools)
        {
            pool.objectHolder = new GameObject();
            pool.objectHolder.name = pool.name;
            pool.objectHolder.transform.parent = this.transform;
            Queue<GameObject> objectPool = new Queue<GameObject>();
            for (int i = 0; i < pool.capacity; i++)
            {
                GameObject gameObject = Instantiate(pool.prefab, pool.objectHolder.transform);
                gameObject.SetActive(false);
                objectPool.Enqueue(gameObject);
            }
            pooler.Add(pool.name, objectPool);
        }
    }

    public GameObject GetObject(GameObject gameObject)
    {
        if (pooler.TryGetValue(gameObject.name, out Queue<GameObject> objectQueue))
        {
            if (objectQueue.Count == 0)
            {
                return CreateNewObject(gameObject);
            }
            else
            {
                GameObject gameObjectInQueue = objectQueue.Dequeue();
                gameObjectInQueue.SetActive(true);
                return gameObjectInQueue;
            }
        }
        else
        {
            return CreateNewObject(gameObject);
        }
    }

    public GameObject CreateNewObject(GameObject gameObject)
    {
        GameObject newGameObject = Instantiate(gameObject, transform);
        return newGameObject;
    }

    public void ReturnGameObject(GameObject gameObject)
    {
        if (pooler.TryGetValue(gameObject.name, out Queue<GameObject> objectQueue))
        {
            objectQueue.Enqueue(gameObject);
        }
        else
        {
            Queue<GameObject> newObjectQueue = new Queue<GameObject>();
            newObjectQueue.Enqueue(gameObject);
            pooler.Add(gameObject.name, newObjectQueue);
        }
        gameObject.SetActive(false);
    }

    public void DeSpawnAllFighters()
    {
        OnDeSpawningAllFighters?.Invoke();
    }
}
