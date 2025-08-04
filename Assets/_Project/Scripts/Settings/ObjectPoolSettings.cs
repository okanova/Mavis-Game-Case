using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Linq;

[CreateAssetMenu(fileName = "ObjectPoolSettings", menuName = "BaseSettings/Object Pool Settings")]
public class ObjectPoolSettings : ScriptableObject
{
    [System.Serializable]
    public class PoolObjectData
    {
        public string typeName;
        public GameObject prefab;
        public int initialSize = 10;
        public int maxSize = 50;
        public bool expandable = true;
    }

    public List<PoolObjectData> poolObjects = new List<PoolObjectData>();

    private const string EnumFilePath = "Assets/_Project/Scripts/Consts/PoolObjectType.cs";

    public void UpdateEnumFile()
    {
        using (StreamWriter writer = new StreamWriter(EnumFilePath, false))
        {
            writer.WriteLine("// Bu dosya otomatik oluşturulmuştur, manuel değiştirmeyin.");
            writer.WriteLine("public enum PoolObjectType");
            writer.WriteLine("{");
            writer.WriteLine("    None = 0,");

            for (int i = 0; i < poolObjects.Count; i++)
            {
                string name = poolObjects[i].typeName;
                if (string.IsNullOrWhiteSpace(name))
                    continue;

                string cleanName = MakeSafeEnumName(name);
                string comma = (i == poolObjects.Count - 1) ? "" : ",";
                writer.WriteLine($"    {cleanName} = {i + 1}{comma}");
            }

            writer.WriteLine("}");
        }

#if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh();
        Debug.Log("PoolObjectType enum güncellendi (buton ile).");
#endif
    }

    private string MakeSafeEnumName(string name)
    {
        return new string(name.Where(char.IsLetterOrDigit).ToArray());
    }
}