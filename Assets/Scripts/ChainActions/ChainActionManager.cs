using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainActionManager : MonoBehaviour
{
    private bool _blockInput = false;
    private GameSystem _currentGameSystem;
    private Chain _chain = new Chain(false);
    private ChainMono _chainMono = new ChainMono(false);
    public Chain Chain => _chain;
    public ChainMono ChainMono => _chainMono;
    public GameSystem CurrentGameSystem => _currentGameSystem;


    private void Update()
    {
        _currentGameSystem.HandleInput(_blockInput);
        _currentGameSystem.Update();

        _chain.UpdateChain(Time.deltaTime);
        _chainMono.UpdateChain(Time.deltaTime);
    }
}
