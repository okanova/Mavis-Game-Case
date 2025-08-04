using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace _Project.Scripts.Core
{
    public static class PrefabManager
    {
        private static Dictionary<string, GameObject> _instantiatedPrefabs = new();

        public static void LoadAndSpawn(string key, Vector3 position, Action<GameObject> onSuccess = null)
        {
            Addressables.InstantiateAsync(key, position, Quaternion.identity).Completed += handle =>
            {
                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    GameObject obj = handle.Result;
                    _instantiatedPrefabs[key] = obj;
                    onSuccess?.Invoke(obj);
                }
                else
                {
                    Debug.LogError($"[PrefabManager] Prefab yüklenemedi: {key}");
                }
            };
        }

        public static void Unload(string key)
        {
            if (_instantiatedPrefabs.TryGetValue(key, out var obj))
            {
                Addressables.ReleaseInstance(obj);
                _instantiatedPrefabs.Remove(key);
            }
        }

        public static void UnloadAll()
        {
            foreach (var pair in _instantiatedPrefabs)
            {
                Addressables.ReleaseInstance(pair.Value);
            }
            _instantiatedPrefabs.Clear();
        }
    }
}