using UnityEngine;
using UnityEngine.UI;

public abstract class BaseButtonView : MonoBehaviour
{
    protected Button button;

    public virtual void Start()
    {
        button = GetComponent<Button>();
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(Click);
    }

    protected abstract void Click();
}
