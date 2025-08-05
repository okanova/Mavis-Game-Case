using _Project.Scripts.Core;
using UnityEngine;

public class HighPointTextView : BaseTextView, ISavable
{
    private int _highScore;
    
    public string SaveKey => "HighScore";

    public object CaptureState()
    {
        return new HighScoreData { value = _highScore };
    }

    public void RestoreState(object state)
    {
        HighScoreData data = (HighScoreData)state;

        _highScore = data.value;
        
        if (_highScore < GameManager.Instance.HighScore)
        {
            _highScore = GameManager.Instance.HighScore;
        }

        text.text = _highScore.ToString();
    }

    public void OnEnable()
    {
        SaveManager.RegisterSavable(this);
    }

    public void OnDisable()
    {
        SaveManager.UnregisterSavable(this);
    }

    [System.Serializable]
    private class HighScoreData
    {
        public int value;
    }
}