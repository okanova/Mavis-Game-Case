using UnityEngine;

[CreateAssetMenu(fileName = "RunnerMapGeneratorSettings", menuName = "Settings/Runner Map Generator Settings")]
public class RunnerMapGeneratorSettings : ScriptableObject
{
    [Header("Generation Rates")]
    [Range(0, 101)] public int rateOfCreatingObstacle = 30;
    [Range(0, 101)] public int rateOfCreatingGold = 15;

    [Header("Gold Settings")]
    public int maxGoldCount = 5;

    [Header("Lane Settings")]
    public float scaleX = 1.75f;
}