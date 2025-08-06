using UnityEngine;

public class Match3DUI : MonoBehaviour
{
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject losePanel;
    
    private void OnEnable()
    {
        EventManager.RegisterEvent(GameEvents.Win, Win);
        EventManager.RegisterEvent(GameEvents.Lose, Lose);
    }

    private void OnDisable()
    {
        EventManager.UnregisterEvent(GameEvents.Win, Win);
        EventManager.UnregisterEvent(GameEvents.Lose, Lose);
    }

    private void Win()
    {
        winPanel.SetActive(true);
    }

    private void Lose()
    {
        losePanel.SetActive(true);
    }
}
