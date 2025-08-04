using System;

public class CurrencyArgs : EventArgs
{
    public Currencies type;
    public float value;

    public CurrencyArgs(Currencies type, float v)
    {
        this.type = type;
        this.value = v;
    }
}