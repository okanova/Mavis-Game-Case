using System;
using UnityEngine;

public class GoldTextView : BaseTextView
{
    private int _gold;
    
    private void OnEnable()
    {
        EventManager.RegisterEvent<CurrencyArgs>(GameEvents.Currency, SetGoldText);
    }

    private void OnDisable()
    {
        EventManager.UnregisterEvent<CurrencyArgs>(GameEvents.Currency, SetGoldText);
    }

    private void SetGoldText(CurrencyArgs args)
    {
        if (args.type != Currencies.gold)
            return;
        
        _gold += (int)args.value;
        text.text = _gold.ToString();
    }
}
