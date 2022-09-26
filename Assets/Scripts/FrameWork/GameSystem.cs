using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameSystem
{
    protected Player _player;
    protected int[] _layersToIgnore;

    protected GameSystem(Player player, int[] ignoreLayers)
    {
        _player = player;
        _layersToIgnore = ignoreLayers;
    }
    
    public virtual void HandleInput()
    {
        
    }

    public virtual void Update()
    {
        
    }
}
