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
        public GameObject ObjectHolder;
        public string Name;
        public GameObject Prefab;
        public int Capacity;
    }

    public List<ObjectToPool> pools;
    public Dictionary<string, Queue<GameObject>> pooler;

    public Dictionary<string, Queue<GameObject>> Pooler { get => pooler; set => pooler = value; }

    public void Awake()
    {
        pooler = new Dictionary<string, Queue<GameObject>>();
        foreach (ObjectToPool pool in pools)
        {
            pool.ObjectHolder = new GameObject(pool.Name);
            pool.ObjectHolder.transform.parent = transform;
            Queue<GameObject> objectPool = new Queue<GameObject>();
            for (int i = 0; i < pool.Capacity; i++)
            {
                GameObject gameObject = Instantiate(pool.Prefab, transform);
                gameObject.transform.parent = pool.ObjectHolder.transform;
                gameObject.SetActive(false);
                objectPool.Enqueue(gameObject);
            }
            pooler.Add(pool.Name, objectPool);
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
