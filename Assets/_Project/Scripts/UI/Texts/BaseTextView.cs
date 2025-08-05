using TMPro;
using UnityEngine;

public class BaseTextView : MonoBehaviour
{
    protected TextMeshProUGUI text;
    protected virtual void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }
}
