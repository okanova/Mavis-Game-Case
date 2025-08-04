using System;
using TMPro;
using UnityEngine;

public class GameSelectPanel : MonoBehaviour
{
    [SerializeField] private GameSelectButtonView _templateGameButton;
    [SerializeField] private Scene[] _scenes;
    [SerializeField] private Transform _buttonParent;
    
    
    private void Start()
    {
        foreach (var scene in _scenes)
        {
            GameSelectButtonView currentButton = Instantiate(_templateGameButton, _buttonParent);
            currentButton.SelectScene = scene;
            currentButton.name = scene + "Button";

            TextMeshProUGUI currentText = currentButton.GetComponentInChildren<TextMeshProUGUI>();
            currentText.name = scene + "Text";
            currentText.text = scene.ToString();
        }
    }
}
