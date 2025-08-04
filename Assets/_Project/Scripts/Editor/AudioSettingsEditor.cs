#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AudioSettings))]
public class AudioSettingsEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        AudioSettings settings = (AudioSettings)target;

        GUILayout.Space(10);
        if (GUILayout.Button("Reflesh"))
        {
            settings.UpdateEnumFile();
        }
    }
}
#endif