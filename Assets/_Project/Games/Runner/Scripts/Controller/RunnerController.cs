using System;
using System.Collections;
using System.Collections.Generic;
using _Project.Scripts.Core;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class RunnerController : MonoBehaviour, IMiniGame
{
    [SerializeField] private RunnerMapGeneratorSettings runnerMapGeneratorSettings;
    
    private int _currentPosition;
    
    [Header("For Select X Position")]
    private int _putRightMaxCount = 10;
    private int _putMidMaxCount = 10;
    private int _putLeftMaxCount = 10;

    private readonly HashSet<Vector2Int> usedPositions = new();

    private void OnEnable()
    {
        Initialize();
        EventManager.RegisterEvent(GameEvents.End, EndGame);
    }

    private void OnDisable()
    {
        EventManager.UnregisterEvent(GameEvents.End, EndGame);
    }

    public string GameId { get; }

    public void Initialize()
    {
        ObjectPoolManager.InitializePools();
        StartCoroutine(DelayedStart());
    }

    private IEnumerator DelayedStart()
    {
        ObjectPoolManager.LoadAddressablePoolsForScene(Scene.Runner);

        yield return new WaitUntil(() =>
            ObjectPoolManager.HasPool(PoolObjectType.Road) &&
            ObjectPoolManager.HasPool(PoolObjectType.Gold) &&
            ObjectPoolManager.HasPool(PoolObjectType.Obstacle));

        StartGame();
    }

    public void StartGame()
    {
        // PrefabManager.LoadAndSpawn("Character", Vector3.up);
        GenerateMap(50);
        GenerateMap(100);
    }

    private void GenerateMap(int newPosition)
    {
        CreateRoad();
        GenerateObjectsBetween(_currentPosition, newPosition);
        _currentPosition = newPosition;
    }

    private void CreateRoad()
    {
        GameObject pooledObj = ObjectPoolManager.GetObject(PoolObjectType.Road);
        if (pooledObj == null) return;

        Transform tempRoad = pooledObj.transform;
        tempRoad.transform.position = Vector3.forward * _currentPosition;
    }

    private void GenerateObjectsBetween(int startZ, int endZ)
    {
        usedPositions.Clear();

        for (int i = startZ; i < endZ - 5; i++)
        {
            int randomObs = Random.Range(1, 100);
            if (randomObs <= runnerMapGeneratorSettings.rateOfCreatingObstacle)
            {
                CreateObject(i, PoolObjectType.Obstacle);
            }

            int randomGold = Random.Range(1, 100);
            if (randomGold <= runnerMapGeneratorSettings.rateOfCreatingGold)
            {
                int randomCount = Random.Range(0, runnerMapGeneratorSettings.maxGoldCount);
                randomCount = Mathf.Min(endZ, i + randomCount) - i;
                CreateObject(i, PoolObjectType.Gold, randomCount);
            }
        }
    }

    private void CreateObject(int z, PoolObjectType type, int maxZ = 0)
    {
        int random = 0;

        if (z > _putRightMaxCount && z > _putMidMaxCount && z > _putLeftMaxCount)
        {
            random = Random.Range(-1, 2);
        }
        else if (z > _putRightMaxCount && z > _putMidMaxCount)
        {
            random = Random.Range(0, 2);
        }
        else if (z > _putMidMaxCount && z > _putLeftMaxCount)
        {
            random = Random.Range(-1, 1);
        }
        else
        {
            return;
        }

        maxZ = Mathf.Max(1, maxZ);

        for (int i = 0; i < maxZ; i++)
        {
            int currentZ = z + i;
            Vector2Int key = new(random, currentZ);
            if (usedPositions.Contains(key))
                continue;

            usedPositions.Add(key);

            GameObject pooledObj = ObjectPoolManager.GetObject(type);
            if (pooledObj == null) return;

            Transform temp = pooledObj.transform;
            temp.transform.position = new Vector3(runnerMapGeneratorSettings.scaleX * random, 1f, currentZ);
        }

        switch (random)
        {
            case -1: _putLeftMaxCount = z + maxZ; break;
            case 0: _putMidMaxCount = z + maxZ; break;
            case 1: _putRightMaxCount = z + maxZ; break;
        }
    }

    public void EndGame()
    {
        PrefabManager.Unload("Character");
    }
}
