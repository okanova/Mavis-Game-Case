using System.Collections;
using System.Collections.Generic;
using _Project.Scripts.Core;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public GameState CurrentState { get; private set; } = GameState.Booting;
    
    public int HighScore { get; private set; }
    public int Gold { get; private set; }

    private void OnEnable()
    {
        EventManager.RegisterEvent<CurrencyArgs>(GameEvents.Currency, SetGold);
        EventManager.RegisterEvent<CurrencyArgs>(GameEvents.Currency, SetPoint);
    }

    private void OnDisable()
    {
        EventManager.UnregisterEvent<CurrencyArgs>(GameEvents.Currency, SetGold);
        EventManager.UnregisterEvent<CurrencyArgs>(GameEvents.Currency, SetPoint);
    }
    
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
        if (stateEventMap.TryGetValue(state, out var gameEvents))
        {
            foreach (var gameEvent in gameEvents)
            {
                EventManager.InvokeEvent(gameEvent);
            }
        }
    }
    
    private static readonly Dictionary<GameState, List<GameEvents>> stateEventMap = new()
    {
        { GameState.Playing, new List<GameEvents> { GameEvents.Start } },
        { GameState.Paused, new List<GameEvents> { GameEvents.Pause } },
        { GameState.Resuming, new List<GameEvents> { GameEvents.Play } },
        { GameState.Win, new List<GameEvents> { GameEvents.End, GameEvents.Win } },
        { GameState.Lose, new List<GameEvents> { GameEvents.End, GameEvents.Lose } },
        { GameState.GameOver, new List<GameEvents> { GameEvents.End } },
        { GameState.Exit, new List<GameEvents> { GameEvents.Exit } },
    };

    
    private void SetGold(CurrencyArgs args)
    {
        if (args.type != Currencies.gold)
            return;
        
        Gold += (int)args.value;
        HighScore += (int)args.value * 10;
    }

    private void SetPoint(CurrencyArgs args)
    {
        if (args.type != Currencies.point)
            return;
        
        HighScore += (int)args.value;
    }
}