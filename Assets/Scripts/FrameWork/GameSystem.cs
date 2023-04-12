using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameSystem
{
    protected Player _player;
    protected int[] _layersToIgnore;

    protected LayerMask _layerMask;

    protected GameSystem(Player player, int[] ignoreLayers)
    {
        _player = player;
        _layersToIgnore = ignoreLayers;
    }
    protected GameSystem(Player player, LayerMask layerMask)
    {
        _player = player;
        _layerMask = layerMask;
    }

    public virtual void HandleInput(bool isInputBlocked)
    {
        
    }

    public virtual void Update()
    {
        
    }
}
