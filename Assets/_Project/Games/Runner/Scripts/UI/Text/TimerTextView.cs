using System;
using UnityEngine;

public class TimerTextView : BaseTextView
{
    private float _timer;
    private int _lastWholeSecond;

    private void Update()
    {
        if (GameManager.Instance.CurrentState != GameState.Playing)
            return;
        
        _timer += Time.deltaTime;
        
        int currentWholeSecond = Mathf.FloorToInt(_timer);
        if (currentWholeSecond > _lastWholeSecond)
        {
            _lastWholeSecond = currentWholeSecond;
            CurrencyArgs args = new CurrencyArgs(Currencies.point, 1);
            EventManager.InvokeEvent(GameEvents.Currency, args);
        }
        
        int minutes = Mathf.FloorToInt(_timer / 60f);
        int seconds = Mathf.FloorToInt(_timer % 60f);

        text.text = $"{minutes:00}:{seconds:00}";
    }
}
