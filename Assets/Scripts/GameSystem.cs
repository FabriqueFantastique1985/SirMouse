using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameSystem
{
    protected Player _player;

    protected GameSystem(Player player)
    {
        _player = player;
    }
    
    public virtual void HandleInput()
    {
        
    }

    public virtual void Update()
    {
        
    }
}
