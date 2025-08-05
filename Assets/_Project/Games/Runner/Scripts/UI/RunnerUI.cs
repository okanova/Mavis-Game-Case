using System;
using UnityEngine;
using UnityEngine.Serialization;

public class RunnerUI : MonoBehaviour
{
    [SerializeField] private GameObject gameOverPanel;


    private void OnEnable()
    {
        EventManager.RegisterEvent(GameEvents.End, GameOver);
    }

    private void OnDisable()
    {
        EventManager.UnregisterEvent(GameEvents.End, GameOver);
    }

    private void GameOver()
    {
        gameOverPanel.SetActive(true);
    }
}
