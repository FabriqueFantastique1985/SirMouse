using System.Collections;
using System.Collections.Generic;
using UnityCore.Menus;
using UnityCore.Scene;
using UnityEngine;

public class InteractionLevelChange : Interaction
{
    [SerializeField]
    private SceneType _sceneToTeleportTo;

    [Header("this Value should be == to Value of the desired spawn in next scene")]
    public int SpawnValue;

    protected override void SpecificAction(Player player)
    {
        base.SpecificAction(player);

        SceneController.Instance.Load(_sceneToTeleportTo, null, false, PageType.Loading, SpawnValue);
    }
}
