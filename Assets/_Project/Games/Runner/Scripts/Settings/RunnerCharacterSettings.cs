using UnityEngine;

[CreateAssetMenu(fileName = "RunnerCharacterSettings", menuName = "Settings/Runner Character Settings")]
public class RunnerCharacterSettings : ScriptableObject
{
    [Header("Lane Settings")]
    public float laneDistance = 1.75f; 

    [Header("Movement")]
    public float forwardSpeed = 5f;     
    public float laneSwitchSpeed = 10f; 
}