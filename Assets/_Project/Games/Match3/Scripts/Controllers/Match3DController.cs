using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using _Project.Scripts.Core;

public class Match3DController : MonoBehaviour, IMiniGame
{
    [SerializeField] private FruitDatabase fruitDatabase;

    private NestController _nestController;
    private int _total;

    private void OnEnable()
    {
        Initialize();
        EventManager.RegisterEvent(GameEvents.Exit, EndGame);
        EventManager.RegisterEvent<BoolArgs>(GameEvents.CheckNest, CheckGame);
    }
    
    private void OnDisable()
    {
        EventManager.UnregisterEvent(GameEvents.Exit, EndGame);
        EventManager.UnregisterEvent<BoolArgs>(GameEvents.CheckNest, CheckGame);
    }

    private IEnumerator SpawnFruitsRoutine()
    {
        if (fruitDatabase == null || fruitDatabase.fruits.Count == 0) yield break;

        int totalToSpawn = fruitDatabase.spawnCount;
        List<FruitData> guaranteedFruits = new List<FruitData>();
        List<FruitData> allFruits = fruitDatabase.fruits;
        
        foreach (var fruit in allFruits)
        {
            guaranteedFruits.Add(fruit);
            guaranteedFruits.Add(fruit);
            guaranteedFruits.Add(fruit);
        }

        int remaining = totalToSpawn - guaranteedFruits.Count;
        int extraTripletCount = remaining / 3;
        
        for (int i = 0; i < extraTripletCount; i++)
        {
            FruitData randomFruit = allFruits[Random.Range(0, allFruits.Count)];
            guaranteedFruits.Add(randomFruit);
            guaranteedFruits.Add(randomFruit);
            guaranteedFruits.Add(randomFruit);
        }
        
        for (int i = 0; i < guaranteedFruits.Count; i++)
        {
            var temp = guaranteedFruits[i];
            int randomIndex = Random.Range(i, guaranteedFruits.Count);
            guaranteedFruits[i] = guaranteedFruits[randomIndex];
            guaranteedFruits[randomIndex] = temp;
        }
        
        foreach (var fruit in guaranteedFruits)
        {
            GameObject go = ObjectPoolManager.GetObject(fruit.poolType);
            FruitController obj = go.GetComponent<FruitController>();
            obj.Intialize(fruitDatabase, fruit.poolType);

            yield return new WaitForSeconds(fruitDatabase.spawnDelay);
        }
    }

    private void CheckGame(BoolArgs args)
    {
        if (args.value)
        {
            _total += 3;

            if (_total == fruitDatabase.spawnCount)
            {
                GameManager.Instance.ChangeState(GameState.Win);
            }
        }
        else
        {
            foreach (var nest in _nestController.Nests)
            {
                if (nest.fruit == null)
                    return;
            }
            
            GameManager.Instance.ChangeState(GameState.Lose);
        }
    }


    public string GameId { get; }
    public void Initialize()
    {
        ObjectPoolManager.InitializePools();
        StartCoroutine("DelayedStart");
    }
    
    private IEnumerator DelayedStart()
    {
        ObjectPoolManager.LoadAddressablePoolsForScene(Scene.Match3D);
        PrefabManager.LoadAndSpawn("Environment", Vector3.zero);

        yield return new WaitUntil(() =>
            ObjectPoolManager.HasPool(PoolObjectType.GreenApple) &&
            ObjectPoolManager.HasPool(PoolObjectType.RedApple) &&
            ObjectPoolManager.HasPool(PoolObjectType.Pear) &&
            ObjectPoolManager.HasPool(PoolObjectType.Eggplant) &&
            ObjectPoolManager.HasPool(PoolObjectType.Potato) &&
            ObjectPoolManager.HasPool(PoolObjectType.Tomato));

        StartGame();
    }

    public void StartGame()
    {
        _nestController = FindFirstObjectByType<NestController>();
        StartCoroutine("SpawnFruitsRoutine");
    }

    public void EndGame()
    {
        PrefabManager.Unload("Environment");
    }
}
