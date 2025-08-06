using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "FruitDatabase", menuName = "Settings/Fruit Database")]
public class FruitDatabase : ScriptableObject
{
    public List<FruitData> fruits;
    public int spawnCount;
    public float spawnRadius;
    public float spawnDelay;
    public Vector3 spawnPosition;
}