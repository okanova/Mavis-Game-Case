using _Project.Scripts.Core;
using UnityEngine;

public class GameSelectButtonView : BaseButtonView
{
    private Scene selectScene;

    public Scene SelectScene
    {
        get => selectScene;
        set => selectScene = value;
    }
    
    protected override void Click()
    {
        SceneLoader.LoadScene(selectScene);
    }
}
