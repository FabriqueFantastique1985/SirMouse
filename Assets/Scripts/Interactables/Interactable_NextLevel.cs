using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityCore.Scene;
using UnityCore.Menus;

public class Interactable_NextLevel : Interactable
{
    [SerializeField]
    private SceneType _sceneToTeleportTo;

    [Header("this Value should be == to Value of the desired spawn in next scene")]
    public int SpawnValue;

    protected override void OnInteractBalloonClicked(InteractBalloon sender, Player player)
    {
        base.OnInteractBalloonClicked(sender, player);

        // load the level
        SceneController.SceneControllerInstance.Load(_sceneToTeleportTo, null, false, PageType.Loading, SpawnValue);
    }
}
