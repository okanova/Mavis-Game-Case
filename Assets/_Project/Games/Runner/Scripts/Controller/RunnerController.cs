using System;
using _Project.Scripts.Core;
using UnityEngine;

public class RunnerController : MonoBehaviour, IMiniGame
{
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
        StartGame();
    }

    public void StartGame()
    {
        PrefabManager.LoadAndSpawn("Plane", new Vector3(0, -1, 0));
        PrefabManager.LoadAndSpawn("Character", Vector3.zero);
    }

    public void EndGame()
    {
       PrefabManager.Unload("Plane");
       PrefabManager.Unload("Character");
    }
}