using UnityEngine;
using System.Collections.Generic;
using _Project.Scripts.Core;

public class ObjectPoolTester : MonoBehaviour
{
    [SerializeField] private PoolObjectType testType = PoolObjectType.None;
    [SerializeField] private Vector3 spawnPosition = Vector3.zero;
    [SerializeField] private Vector3 spawnOffset = new Vector3(1, 0, 0); // Her yeni obje biraz sağa gelsin

    private List<GameObject> spawnedObjects = new();

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            GameObject obj = ObjectPoolManager.GetObject(testType);
            if (obj != null)
            {
                Vector3 position = spawnPosition + spawnOffset * spawnedObjects.Count;
                obj.transform.position = position;
                spawnedObjects.Add(obj);

                Debug.Log($"Obj alındı ve konumlandırıldı: {position}");
            }
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            if (spawnedObjects.Count > 0)
            {
                GameObject lastObj = spawnedObjects[^1]; // son eleman
                ObjectPoolManager.ReturnObject(testType, lastObj);
                spawnedObjects.RemoveAt(spawnedObjects.Count - 1);

                Debug.Log("Obj geri iade edildi.");
            }
        }
    }
}