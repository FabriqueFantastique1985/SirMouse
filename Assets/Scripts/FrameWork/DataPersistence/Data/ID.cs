using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class ID : MonoBehaviour
{ 
    [SerializeField]
    private string _idName = "";
    
    public string IDName => _idName;

    private void Awake()
    {
        if (string.IsNullOrEmpty(_idName))
        {
            GenerateGuid();
        }
    }

    public void GenerateGuid()
    {
        _idName = System.Guid.NewGuid().ToString();
        #if UNITY_EDITOR
        // set dirty so that it saves the changes
        EditorUtility.SetDirty(this);
        #endif
    }

    private void Reset()
    {
        GenerateGuid();
        #if UNITY_EDITOR
        EditorUtility.SetDirty(this);
        #endif
    }

    public static implicit operator string(ID id)
    {
        return id._idName;
    }
}
