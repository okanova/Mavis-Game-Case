using UnityEngine;

[CreateAssetMenu(fileName = "FruitData", menuName = "Settings/Fruit Data")]
public class FruitData : ScriptableObject
{
    public string fruitName;
    public PoolObjectType poolType;
}