using System;

public class StringArgs : EventArgs
{
    public String value;

    public StringArgs(String v)
    {
        this.value = v;
    }
}