using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.Core
{
    public static class SaveManager
    {
        private static List<ISavable> savables = new List<ISavable>();

        public static void RegisterSavable(ISavable savable)
        {
            if (!savables.Contains(savable))
                savables.Add(savable);
        }

        public static void UnregisterSavable(ISavable savable)
        {
            savables.Remove(savable);
        }

        public static void SaveAll()
        {
            foreach (var savable in savables)
            {
                var data = savable.CaptureState();
                string json = JsonUtility.ToJson(data);
                PlayerPrefs.SetString(savable.SaveKey, json);
            }

            PlayerPrefs.Save();
        }

        public static void LoadAll()
        {
            foreach (var savable in savables)
            {
                if (PlayerPrefs.HasKey(savable.SaveKey))
                {
                    string json = PlayerPrefs.GetString(savable.SaveKey);
                    var dataType = savable.CaptureState().GetType();  // tipi öğren
                    var state = JsonUtility.FromJson(json, dataType);
                    savable.RestoreState(state);
                }
            }
        }
    }
}