using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace _Project.Scripts.Core
{
    public static class ObjectPoolManager
    {
        private class Pool
        {
            public Queue<GameObject> availableObjects = new();
            public GameObject prefab;
            public int maxSize;
            public bool expandable;
            public int createdCount;
            public float autoReleaseTime;

            // Aktif auto-return coroutine takip etmek istersen:
            private readonly HashSet<GameObject> activeAutoReturns = new();

            public Pool(GameObject prefab, int initialSize, int maxSize, bool expandable, float autoReleaseTime)
            {
                this.prefab = prefab;
                this.maxSize = maxSize;
                this.expandable = expandable;
                this.autoReleaseTime = autoReleaseTime;
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
                GameObject obj = null;

                while (availableObjects.Count > 0)
                {
                    obj = availableObjects.Dequeue();

                    // Eğer obje Destroy edilmişse, geç
                    if (obj == null)
                        continue;

                    break;
                }

                if (obj == null)
                {
                    if ((expandable || createdCount < maxSize) && prefab != null)
                    {
                        obj = Object.Instantiate(prefab);
                        createdCount++;
                    }
                    else
                    {
                        Debug.LogWarning($"[Pool] Kapasite dolu veya prefab null: {prefab?.name}");
                        return null;
                    }
                }

                obj.SetActive(true);

                if (autoReleaseTime > 0)
                {
                    if (!activeAutoReturns.Contains(obj))
                    {
                        activeAutoReturns.Add(obj);
                        GameManager.Instance.StartCoroutine(AutoReturnRoutine(obj, autoReleaseTime));
                    }
                }

                return obj;
            }
            

            public void Return(GameObject obj)
            {
                obj.SetActive(false);
                availableObjects.Enqueue(obj);
                activeAutoReturns.Remove(obj);
            }

            private IEnumerator AutoReturnRoutine(GameObject obj, float delay)
            {
                float timer = delay;
            
                while (timer > 0f)
                {
                    if (GameManager.Instance.CurrentState == GameState.Resuming)
                    {
                        timer -= Time.deltaTime;
                    }

                    yield return null;
                }
            
                if (!obj.activeInHierarchy)
                {
                    activeAutoReturns.Remove(obj);
                    yield break;
                }
            
                obj.SetActive(false);
                availableObjects.Enqueue(obj);
                Debug.Log($"[AutoReturn] {obj.name} otomatik geri döndü.");

                activeAutoReturns.Remove(obj);
            }
        }

        private static Dictionary<PoolObjectType, Pool> pools = new();
        private static ObjectPoolSettings settings;
        private static bool initialized = false;

        public static void InitializePools()
        {
            Init();
        }

        private static void Init()
        {
            if (initialized) return;
            initialized = true;

            settings = Resources.Load<ObjectPoolSettings>("ObjectPoolSettings");
            if (settings == null)
            {
                Debug.LogError("ObjectPoolSettings bulunamadı! Assets/Resources/ObjectPoolSettings.asset yoluna bak.");
                return;
            }

            foreach (var data in settings.poolObjects)
            {
                if (!System.Enum.TryParse<PoolObjectType>(data.typeName, out var type)) continue;

                if (!data.addressableTick && data.prefab != null)
                {
                    pools[type] = new Pool(data.prefab, data.initialSize, data.maxSize, data.expandable, data.autoReturnTime);
                }
            }
        }

        public static GameObject GetObject(PoolObjectType type)
        {
            Init();

            if (pools.TryGetValue(type, out var pool))
            {
                return pool.Get();
            }
            else
            {
                Debug.LogError($"'{type}' için pool bulunamadı. Sahne yüklendi mi? Prefab doğru yüklendi mi?");
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
                Debug.LogWarning($"[Pool] '{type}' için pool bulunamadı. Objeyi yok ediyorum.");
                Object.Destroy(obj);
            }
        }

        public static bool HasPool(PoolObjectType type)
        {
            return pools.ContainsKey(type);
        }

        public static void LoadAddressablePoolsForScene(Scene targetScene)
        {
            GameManager.Instance.StartCoroutine(LoadSceneSpecificPoolsRoutine(targetScene));
        }

        private static IEnumerator LoadSceneSpecificPoolsRoutine(Scene scene)
        {
            var keysToRemove = new List<PoolObjectType>();

            foreach (var kvp in pools)
            {
                var objData = settings.poolObjects.Find(p => p.typeName == kvp.Key.ToString());
                if (objData != null && objData.addressableTick && objData.scene != scene)
                {
                    keysToRemove.Add(kvp.Key);
                }
            }

            foreach (var key in keysToRemove)
            {
                pools.Remove(key);
            }

            foreach (var data in settings.poolObjects)
            {
                if (!data.addressableTick || data.scene != scene) continue;
                if (!System.Enum.TryParse<PoolObjectType>(data.typeName, out var type)) continue;

                if (data.prefabRef == null)
                {
                    Debug.LogError($"Addressable prefab reference null: {data.typeName}");
                    continue;
                }

                // HATA KORUMASI: Zaten yüklenmişse tekrar yükleme
                if (data.prefabRef.OperationHandle.IsValid() && data.prefabRef.OperationHandle.Status == AsyncOperationStatus.Succeeded)
                {
                    GameObject prefab = data.prefabRef.OperationHandle.Result as GameObject;
                    pools[type] = new Pool(prefab, data.initialSize, data.maxSize, data.expandable, data.autoReturnTime);
                    Debug.Log($"✅ [Cached] Addressable prefab yüklendi: {data.typeName}");
                }
                else
                {
                    var handle = data.prefabRef.LoadAssetAsync<GameObject>();
                    yield return handle;

                    if (handle.Status == AsyncOperationStatus.Succeeded)
                    {
                        var prefab = handle.Result;
                        pools[type] = new Pool(prefab, data.initialSize, data.maxSize, data.expandable, data.autoReturnTime);
                        Debug.Log($"✅ Addressable prefab yüklendi: {data.typeName}");
                    }
                    else
                    {
                        Debug.LogError($"❌ Addressable prefab yüklenemedi: {data.typeName}");
                    }
                }
            }
        }

    }
}
