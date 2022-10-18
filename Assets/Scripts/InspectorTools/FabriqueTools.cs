﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Experimental.TerrainAPI;

public class FabriqueTools : EditorWindow
{
    private static bool _leftCtrlHeldDown = false;
    private static FabriqueTools _window = null;
    
    private  SceneView.OnSceneFunc onSceneGUIFunc = null;
   
    public Rect WindowSize = new Rect(15, 15, 250, 250);

    [MenuItem("FabriqueTools/ShortCuts")]
    public static void ShowWindow()
    {
        if (_window == null)
        {
            _window = GetWindow<FabriqueTools>("FabriqueTools");
            _window.onSceneGUIFunc = new SceneView.OnSceneFunc(OnSceneGUI);
            SceneView.onSceneGUIDelegate += _window.onSceneGUIFunc;
        }
    }
    
    void OnDestroy()
    {
        SceneView.onSceneGUIDelegate -= onSceneGUIFunc;
        _window = null;
    }

    private void OnGUI()
    {
        GUILayout.Label("Fabrique Fantastique Tools", EditorStyles.boldLabel);
        if (GUILayout.Button("Align with Main Camera (Ctrl + C)"))
        {
            AlignCamera();
        }
        
       if (AlignCameraInput()) AlignCamera();
    }

   
    public static void OnSceneGUI(SceneView sceneview)
    {
        if (AlignCameraInput()) AlignCamera();
    }

    private static bool AlignCameraInput()
    {
        Event e = Event.current;
        // If statements are left separate in case
        // you intend to utilize more key/mouse buttons
        
        if(e.type == EventType.KeyDown)
        {
            if(e.keyCode == KeyCode.LeftControl)
            {
                _leftCtrlHeldDown = true;
            }
        }
        else if (e.type == EventType.KeyUp)
        {
            if (e.keyCode == KeyCode.LeftControl)
                _leftCtrlHeldDown = false;
        }
        
        if(_leftCtrlHeldDown && e.type == EventType.KeyUp)
        {
            if(e.keyCode == KeyCode.C)
            {
                return true;
            }
        }

        return false;
    }
    
    private static void AlignCamera()
    {
        var mainCamera = Camera.main;
        
        var camera = SceneView.lastActiveSceneView;
        camera.pivot = mainCamera.transform.position;
        camera.rotation = mainCamera.transform.rotation;
        camera.orthographic = true;
    }
}
