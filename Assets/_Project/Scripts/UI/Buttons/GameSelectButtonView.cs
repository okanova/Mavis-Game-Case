using _Project.Scripts.Core;
using UnityEngine;

public class GameSelectButtonView : BaseButtonView
{
    private Scene selectScene = Scene.Menu;

    public Scene SelectScene
    {
        get => selectScene;
        set => selectScene = value;
    }
    
    protected override void Click()
    {
        SceneLoader.LoadScene(selectScene);
        GameManager.Instance.ChangeState(selectScene == Scene.Menu ? GameState.Exit : GameState.Playing);
    }
}
