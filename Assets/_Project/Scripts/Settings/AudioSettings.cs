using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

[CreateAssetMenu(fileName = "AudioSettings", menuName = "BaseSettings/Audio Settings")]
public class AudioSettings : ScriptableObject
{
    [Serializable]
    public class AudioEventData
    {
        public string eventName;
        public AudioClip clip;
        public bool loop;
        [Range(0f, 1f)]
        public float volume = 1f;
    }

    public List<AudioEventData> audioEvents = new List<AudioEventData>();

    private const string EnumFilePath = "Assets/_Project/Scripts/Consts/AudioEvents.cs";

    public void UpdateEnumFile()
    {
        using (StreamWriter writer = new StreamWriter(EnumFilePath, false))
        {
            writer.WriteLine("// Bu dosya otomatik oluşturulmuştur, manuel değiştirmeyin.");
            writer.WriteLine("public enum AudioEvents");
            writer.WriteLine("{");
            writer.WriteLine("    None = 0,");

            for (int i = 0; i < audioEvents.Count; i++)
            {
                string name = audioEvents[i].eventName;
                if (string.IsNullOrWhiteSpace(name))
                    continue;

                string cleanName = MakeSafeEnumName(name);
                string comma = (i == audioEvents.Count - 1) ? "" : ",";
                writer.WriteLine($"    {cleanName} = {i + 1}{comma}");
            }

            writer.WriteLine("}");
        }

#if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh();
        Debug.Log("AudioEvents enum güncellendi (buton ile).");
#endif
    }

    private string MakeSafeEnumName(string name)
    {
        return new string(name.Where(char.IsLetterOrDigit).ToArray());
    }
}