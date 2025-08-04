using _Project.Scripts.Core;
using UnityEngine;

public class SaveTest : MonoBehaviour, ISavable
{
    public float height = 1.8f;
    public float weight = 75f;
    public string hairColor = "Brown";

    public string SaveKey => "Player_Character";

    [System.Serializable]
    public class CharacterData
    {
        public float height;
        public float weight;
        public string hairColor;
    }

    public object CaptureState()
    {
        return new CharacterData
        {
            height = this.height,
            weight = this.weight,
        };
    }

    public void RestoreState(object state)
    {
        var data = state as CharacterData;
        height = data.height;
        weight = data.weight;
    }

    public void OnEnable()
    {
        SaveManager.RegisterSavable(this);
    }

    public void OnDisable()
    {
        SaveManager.UnregisterSavable(this);
    }
}