using System;
using System.Collections;
using System.Collections.Generic;
using _Project.Scripts.Core;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

public class NestController : MonoBehaviour
{
    [SerializeField] private Nest[] nests;
    public Nest[] Nests => nests;

    private void OnEnable()
    {
        EventManager.RegisterEvent<FruitArgs>(GameEvents.ChooseFruit, AddFruit);
    }

    private void OnDisable()
    {
        EventManager.UnregisterEvent<FruitArgs>(GameEvents.ChooseFruit, AddFruit);
    }

    private void AddFruit(FruitArgs args)
    {
        foreach (var nest in nests)
        {
            if (nest.fruit == null)
            {
                nest.fruit = args.value;
                nest.fruit.transform.DOJump(nest.pos.position, 
                    2, 1, 0.5f).OnComplete(CheckTrio);
                break;
            }
        }
    }

    private void CheckTrio()
    {
        for (int i = 0; i < nests.Length; i++)
        {
            var fruitA = nests[i].fruit;
            if (fruitA == null) continue;

            int matchCount = 1;
            List<int> matchingIndexes = new List<int> { i };

            for (int j = i + 1; j < nests.Length; j++)
            {
                var fruitB = nests[j].fruit;
                if (fruitB == null) continue;

                if (fruitA.Type == fruitB.Type)
                {
                    matchCount++;
                    matchingIndexes.Add(j);
                }
            }

            if (matchCount >= 3)
            {
                foreach (var index in matchingIndexes)
                {
                    var matchedFruit = nests[index].fruit;
                    ObjectPoolManager.ReturnObject(matchedFruit.Type, matchedFruit.gameObject);
                    nests[index].fruit = null;
                }
                
                StartCoroutine("SlideFruitsToLeft");
                return; 
            }
        }
        
        GameManager.Instance.ChangeState(GameState.Playing);
        BoolArgs args = new BoolArgs(false);
        EventManager.InvokeEvent(GameEvents.CheckNest, args);
    }
    
    private IEnumerator SlideFruitsToLeft()
    {
        List<FruitController> remainingFruits = new List<FruitController>();
        
        foreach (var nest in nests)
        {
            if (nest.fruit != null)
            {
                remainingFruits.Add(nest.fruit);
            }
        }

        foreach (var nest in nests)
        {
            nest.fruit = null;
        }
        
        for (int i = 0; i < remainingFruits.Count; i++)
        {
            var fruit = remainingFruits[i];
            fruit.transform.DOJump(nests[i].pos.position, 1, 1, 0.5f);
            nests[i].fruit = fruit;
        }

        yield return new WaitForSeconds(0.5f);
        
        GameManager.Instance.ChangeState(GameState.Playing);
        BoolArgs args = new BoolArgs(true);
        EventManager.InvokeEvent(GameEvents.CheckNest, args);
    }
}

[Serializable]
public class Nest
{
    public Transform pos;
    public FruitController fruit;
}
