using System.Collections;
using System.Collections.Generic;
using UnityCore.Menus;
using UnityCore.Scene;
using UnityEngine;

public enum LoadingScreenImage
{
    Castle,
    Cave,
    Forest,
    MisterWitchHouse,
    PrinceTower,
    Swamp,
}

public class InteractionLevelChange : Interaction
{
    [SerializeField]
    private SceneType _sceneToTeleportTo;

    [SerializeField]
    private Transform _spawnPosition;
    public Transform SpawnPosition => _spawnPosition;

    [Header("this Value should be == to Value of the desired spawn in next scene")]
    public int SpawnValue;

    [SerializeField]
    private LoadingScreenImage _loadingScreen;


    protected override void SpecificAction(Player player)
    {
        base.SpecificAction(player);

        player.SetState(new IdleState(player));
        SceneController.Instance.Load(_sceneToTeleportTo, null, false, PageType.Loading, SpawnValue);
        PageController.Instance.SetLoadingScreen(_loadingScreen);
    }
}