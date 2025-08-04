using System.Collections.Generic;
using UnityEngine;

public static class ObjectPoolManager
{
    private class Pool
    {
        public Queue<GameObject> availableObjects = new();
        public GameObject prefab;
        public int maxSize;
        public bool expandable;
        public int createdCount;

        public Pool(GameObject prefab, int initialSize, int maxSize, bool expandable)
        {
            this.prefab = prefab;
            this.maxSize = maxSize;
            this.expandable = expandable;
            this.createdCount = 0;

            for (int i = 0; i < initialSize; i++)
            {
                GameObject obj = Object.Instantiate(prefab);
                obj.SetActive(false);
                availableObjects.Enqueue(obj);
                createdCount++;
            }
        }

        public GameObject Get()
        {
            if (availableObjects.Count > 0)
            {
                GameObject obj = availableObjects.Dequeue();
                obj.SetActive(true);
                return obj;
            }
            else if (expandable && createdCount < maxSize)
            {
                GameObject obj = Object.Instantiate(prefab);
                createdCount++;
                obj.SetActive(true);
                return obj;
            }
            else if (createdCount < maxSize)
            {
                // Expandable false olsa da maxSize'a kadar üretim yapılabilir.
                GameObject obj = Object.Instantiate(prefab);
                createdCount++;
                obj.SetActive(true);
                return obj;
            }
            else
            {
                return null;
            }
        }

        public void Return(GameObject obj)
        {
            obj.SetActive(false);
            availableObjects.Enqueue(obj);
        }
    }

    private static Dictionary<PoolObjectType, Pool> pools = new();

    private static ObjectPoolSettings settings;

    private static bool initialized = false;

    private static void Init()
    {
        if (initialized) return;
        initialized = true;

        settings = Resources.Load<ObjectPoolSettings>("ObjectPoolSettings");
        if (settings == null)
        {
            Debug.LogError("ObjectPoolSettings asset bulunamadı! Assets/Resources/ObjectPoolSettings.asset dosyasını oluşturun.");
            return;
        }

        foreach (var poolData in settings.poolObjects)
        {
            if (System.Enum.TryParse<PoolObjectType>(poolData.typeName, out var type))
            {
                pools[type] = new Pool(poolData.prefab, poolData.initialSize, poolData.maxSize, poolData.expandable);
            }
            else
            {
                Debug.LogWarning($"PoolObjectType enum'da {poolData.typeName} bulunamadı!");
            }
        }
    }

    // Oyun başında manuel çağrılması önerilir.
    public static void InitializePools()
    {
        Init();
    }

    public static GameObject GetObject(PoolObjectType type)
    {
        Init();

        if (pools.TryGetValue(type, out var pool))
        {
            GameObject obj = pool.Get();
            if (obj == null)
            {
                Debug.LogWarning($"Havuzda yeterli nesne yok ve genişleme kapalı: {type}");
            }
            return obj;
        }
        else
        {
            Debug.LogWarning($"Havuz bulunamadı: {type}");
            return null;
        }
    }

    public static void ReturnObject(PoolObjectType type, GameObject obj)
    {
        Init();

        if (pools.TryGetValue(type, out var pool))
        {
            pool.Return(obj);
        }
        else
        {
            Debug.LogWarning($"Havuz bulunamadı: {type}");
            Object.Destroy(obj);
        }
    }
}
