using System.Collections;
using System.Collections.Generic;
using _Project.Scripts.Core;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public GameState CurrentState { get; private set; } = GameState.Booting;

    private void Start()
    {
        ObjectPoolManager.InitializePools();

        // SaveManager.LoadAll();
        // SaveManager.SaveAll();

        StartCoroutine(LoadMenuRoutine());
    }
    
    private IEnumerator LoadMenuRoutine()
    {
        ChangeState(GameState.Booting);

        yield return SceneLoader.LoadSceneRoutine(Scene.Menu); 
        ChangeState(GameState.Menu);
    }


    public void ChangeState(GameState newState)
    {
        if (newState == CurrentState)
            return;

        Debug.Log($"[GameManager] State changed: {CurrentState} → {newState}");
        CurrentState = newState;

        PublishEventForState(newState);
    }

    private void PublishEventForState(GameState state)
    {
        
        if (stateEventMap.TryGetValue(state, out GameEvents gameEvent))
        {
            EventManager.InvokeEvent(gameEvent);
        }
    }
    
    private static readonly Dictionary<GameState, GameEvents> stateEventMap = new()
    {
        { GameState.Playing, GameEvents.Start },
        { GameState.Paused, GameEvents.Pause },
        { GameState.Resuming, GameEvents.Resume },
        { GameState.Win, GameEvents.Pause },
        { GameState.Lose, GameEvents.Pause },
        { GameState.GameOver, GameEvents.End },
        { GameState.Exit, GameEvents.End },
    };
}