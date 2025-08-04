#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GameEventSettings))]
public class EventManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GameEventSettings settings = (GameEventSettings)target;

        GUILayout.Space(10);

        if (GUILayout.Button("Refresh"))
        {
            settings.UpdateEnumFile();
        }
    }
}
#endif