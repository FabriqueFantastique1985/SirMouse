using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPlayerObjectAction : ChainActionMonoBehaviour
{
    [SerializeField]
    private bool _activate = true;
    public override void Execute()
    {
        base.Execute();
        GameManager.Instance.Player.gameObject.transform.GetChild(0).gameObject.SetActive(_activate);
    }
}