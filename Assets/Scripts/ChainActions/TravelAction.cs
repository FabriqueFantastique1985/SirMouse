using System.Collections;
using System.Collections.Generic;
using Fabrique;
using UnityEngine;
using Utilities;

public class TravelAction : ChainAction
{
    [SerializeField]
    private SceneField _scene;
    
    public override void Execute()
    {
        base.Execute();
        Loader.Instance.LoadScene(_scene);
    }
}
