#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ObjectPoolSettings))]
public class PoolManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        ObjectPoolSettings settings = (ObjectPoolSettings)target;

        GUILayout.Space(10);
        if (GUILayout.Button("Enum Dosyasını Güncelle"))
        {
            settings.UpdateEnumFile();
        }
    }
}
#endif