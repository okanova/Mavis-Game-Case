using System;

public class FruitArgs : EventArgs
{
    public FruitController value;

    public FruitArgs(FruitController v)
    {
        value = v;
    }
}