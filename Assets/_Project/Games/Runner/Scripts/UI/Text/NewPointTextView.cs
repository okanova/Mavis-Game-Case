using UnityEngine;

public class NewPointTextView :  BaseTextView
{
    protected override void Start()
    {
        base.Start();
        text.text = GameManager.Instance.HighScore.ToString();
    }
}
