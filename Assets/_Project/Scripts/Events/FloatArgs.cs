using System;

public class FloatArgs : EventArgs
{
    public float value;

    public FloatArgs(float v)
    {
        this.value = v;
    }
}