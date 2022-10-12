using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityCore.Scene;
using UnityCore.Menus;

public class NextLevelInteractable : Interactable
{
    private SceneController _sceneController;

    [SerializeField]
    private SceneType _sceneToTeleportTo;

    public int SpawnValue;


    protected override void InitializeThings()
    {
        base.InitializeThings();

        _sceneController = FindObjectOfType<SceneController>();
    }


    protected override void OnInteractBalloonClicked(InteractBalloon sender, Player player)
    {
        base.OnInteractBalloonClicked(sender, player);

        // load the level
        _sceneController.Load(_sceneToTeleportTo, null, false, PageType.Loading, SpawnValue);
    }
}
