using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;


[CustomEditor((typeof(ChainActionSceneLoaded)))]
public class ChainActionSceneLoadedEditor : Editor
{
    private ReorderableList _chainActionList;
    private SerializedProperty _propertyList;

    private void OnEnable()
    {
        _propertyList = serializedObject.FindProperty("_chainActions");

        _chainActionList = new ReorderableList(serializedObject, _propertyList, true, true, true, true);

        _chainActionList.drawElementCallback = DrawListItems;
        _chainActionList.drawHeaderCallback = DrawHeader;
    }

    void DrawListItems(Rect rect, int index, bool isActive, bool isFocused)
    {
        SerializedProperty
            element = _chainActionList.serializedProperty.GetArrayElementAtIndex(index); //The element in the list

        // Create a property field and label field for each property. 

   //  // The 'mobs' property. Since the enum is self-evident, I am not making a label field for it. 
   //  // The property field for mobs (width 100, height of a single line)
   //  EditorGUI.PropertyField(
   //      new Rect(rect.x, rect.y, 100, EditorGUIUtility.singleLineHeight),
   //      element.FindPropertyRelative("ActionType"),
   //      GUIContent.none
   //  );

   var name = element.FindPropertyRelative("_nameChainAction");
   
   
    EditorGUI.LabelField(new Rect(rect.x, rect.y, 200, EditorGUIUtility.singleLineHeight), element.objectReferenceValue.name);
   //  // The 'level' property
      //  // The label field for level (width 100, height of a single line)
      //  EditorGUI.LabelField(new Rect(rect.x + 120, rect.y, 100, EditorGUIUtility.singleLineHeight), "MaxTime");
//
      //  //The property field for level. Since we do not need so much space in an int, width is set to 20, height of a single line.
      //  EditorGUI.PropertyField(
      //      new Rect(rect.x + 160, rect.y, 20, EditorGUIUtility.singleLineHeight),
      //      element.FindPropertyRelative("level"),
      //      GUIContent.none
      //  );
//
//
      //  // The 'quantity' property
      //  // The label field for quantity (width 100, height of a single line)
      //  EditorGUI.LabelField(new Rect(rect.x + 200, rect.y, 100, EditorGUIUtility.singleLineHeight), "Quantity");
//
      //  //The property field for quantity (width 20, height of a single line)
      //  EditorGUI.PropertyField(
      //      new Rect(rect.x + 250, rect.y, 20, EditorGUIUtility.singleLineHeight),
      //      element.FindPropertyRelative("quantity"),
      //      GUIContent.none
      //  );
    }

    void DrawHeader(Rect rect)
    {
        string name = "ChainActions";
        EditorGUI.LabelField(rect, name);
    }

    //This is the function that makes the custom editor work
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        serializedObject.Update();
        _chainActionList.DoLayoutList();
        serializedObject.ApplyModifiedProperties();
    }
}