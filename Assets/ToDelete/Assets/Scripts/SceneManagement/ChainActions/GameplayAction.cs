using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayAction : ChainActionMonoBehaviour
{
    [SerializeField]
    private bool _blockInput = true;
    
    public override void Execute()
    {
        base.Execute();
        GameManager.Instance.BlockInput = _blockInput;
    }
}
