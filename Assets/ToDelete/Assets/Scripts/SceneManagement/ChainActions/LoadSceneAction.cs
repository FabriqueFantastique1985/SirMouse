using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utilities;

public class LoadSceneAction : ChainActionMonoBehaviour
{

    [SerializeField] private SceneField _scene;


    public override void Execute()
    {
        base.Execute();
        Debug.Log(_scene.ToString());
        SceneManager.LoadScene(_scene);
    }
}