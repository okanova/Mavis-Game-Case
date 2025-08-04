using System;
using UnityEngine;

public class GameObjectArgs : EventArgs
{
    public GameObject gameObject;

    public GameObjectArgs(GameObject obj)
    {
        gameObject = obj;
    }
}