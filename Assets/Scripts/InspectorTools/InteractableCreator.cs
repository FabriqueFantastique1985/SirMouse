using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class InteractableCreator : EditorWindow
{
    private SerializedObject _serializedObject;
    private static InteractableCreator _window = null;

    [SerializeField]
    private GameObject _interactablePrefab;
    
    [SerializeField]
    private int _amountInteractionsToAdd = 1;

    public Rect WindowSize = new Rect(15, 15, 250, 250);

    private void OnEnable()
    {
        _serializedObject = new SerializedObject(this);
    }

    [MenuItem("FabriqueTools/Interactable Creator")]
    public static void ShowWindow()
    {
        if (_window == null)
        {
            _window = GetWindow<InteractableCreator>("InteractableCreator");
        }
    }

    void OnDestroy()
    {
        _window = null;
    }

  private void OnGUI()
  {
      GUILayout.Label("Interactable Creator", EditorStyles.boldLabel);
      EditorGUILayout.PropertyField(_serializedObject.FindProperty("_interactablePrefab"));
      EditorGUILayout.PropertyField(_serializedObject.FindProperty("_amountInteractionsToAdd"));
      
      if (GUILayout.Button("Create Interactable"))
      {
          // Instantiate new Interactable object
          Interactable interactable = Instantiate(_interactablePrefab).GetComponent<Interactable>();
          Undo.RegisterCreatedObjectUndo(interactable, "Created new Interactable");
          interactable.transform.position = Vector3.zero;
          interactable.name = "Interactable";

          // Instantiate Interactions GameObject and child under Interactable
          if (_amountInteractionsToAdd > 0)
          {
              GameObject interactionsObject = new GameObject("Interactions");
              interactionsObject.transform.position = Vector3.zero;
              interactionsObject.transform.rotation = Quaternion.identity;
              interactionsObject.transform.parent = interactable.transform;
              
              // Instantiate all Interaction components and their respective Game Objects, childed under Interactions
              for (int i = 0; i < _amountInteractionsToAdd; i++)
              {
                  GameObject newInteractionObject = new GameObject($"Interaction_{i}");
                  newInteractionObject.transform.position = Vector3.zero;
                  newInteractionObject.transform.rotation = Quaternion.identity;
                  newInteractionObject.transform.parent = interactionsObject.transform;
                  Interaction newInteraction = newInteractionObject.AddComponent<Interaction>();
                  interactable.Interactions.Add(newInteraction);
              }
          }
      }
      
      _serializedObject.ApplyModifiedProperties();
  }
}



