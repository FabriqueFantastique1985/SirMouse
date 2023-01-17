using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor((typeof(InteractableCreator)))]
public class InteractableCreatorEditor : Editor
{
    private SerializedProperty interactablePrefab;

    private void OnEnable()
    {
        interactablePrefab = serializedObject.FindProperty("_interactablePrefab");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        DrawDefaultInspector();
        EditorGUILayout.PropertyField(interactablePrefab);
        serializedObject.ApplyModifiedProperties();
    }
}