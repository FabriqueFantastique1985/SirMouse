using System.Collections;
using System.Collections.Generic;
using UnityCore.Menus;
using UnityCore.Scene;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utilities;

public class LoadSceneAction : ChainActionMonoBehaviour
{
    [SerializeField] private SceneType _scene;
    private int spawnvalue;


    public override void Execute()
    {
        base.Execute();
        Debug.Log(_scene.ToString());
        SceneController.SceneControllerInstance.Load(_scene, null, false, PageType.Loading, spawnvalue);
    }
}