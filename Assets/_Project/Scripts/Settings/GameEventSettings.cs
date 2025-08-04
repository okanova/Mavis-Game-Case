using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "GameEventSettings", menuName = "ScriptableObjects/Game Event Settings")]
public class GameEventSettings : ScriptableObject
{
    public List<string> eventNames = new List<string>();

    private const string EnumFilePath = "Assets/_Project/Scripts/Consts/GameEvents.cs";

    public void UpdateEnumFile()
    {
        // Enum dosyasını yaz
        using (StreamWriter writer = new StreamWriter(EnumFilePath, false))
        {
            writer.WriteLine("// Bu dosya otomatik oluşturulmuştur, manuel değiştirmeyin.");
            writer.WriteLine("public enum GameEvents");
            writer.WriteLine("{");
            writer.WriteLine("    None = 0,");

            for (int i = 0; i < eventNames.Count; i++)
            {
                string name = eventNames[i];
                if (string.IsNullOrWhiteSpace(name))
                    continue;

                string cleanName = MakeSafeEnumName(name);
                string comma = (i == eventNames.Count - 1) ? "" : ",";
                writer.WriteLine($"    {cleanName} = {i + 1}{comma}");
            }

            writer.WriteLine("}");
        }

#if UNITY_EDITOR
        EditorApplication.delayCall += () =>
        {
            AssetDatabase.Refresh();
            Debug.Log("GameEvents enum otomatik güncellendi.");
        };
#endif
    }

    private string MakeSafeEnumName(string name)
    {
        return new string(name.Where(char.IsLetterOrDigit).ToArray());
    }
}