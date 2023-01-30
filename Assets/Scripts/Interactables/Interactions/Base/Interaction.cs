using System;
using System.Collections;
using System.Collections.Generic;
using Fabrique;
using UnityEngine;
using UnityEngine.Events;

public class Interaction : MonoBehaviour
{
    //[SerializeField]
    //private Sprite _interactionSprite;

    [SerializeField]
    private GameObject _spriteObjectInteractionInBalloon;

    //[SerializeField]
    //protected AnimationClip _animationClip;

    [SerializeField]
    private List<ChainAction> _instantActions;

    
    [SerializeField, Tooltip("When true, a new chain will be generated to play the ChainActions, " +
                             "rather than using the existing chain owned by the GameManager. ")]
    private bool _useNewChain = false;
    
    /// <summary>
    /// Reference to the list of chainActions
    /// </summary>
    [SerializeField]
    private List<ChainAction> _chainActions;
    
    //public Sprite InteractionSprite => _interactionSprite;
    public GameObject SpriteObjectInteractionBalloon
    {
        get => _spriteObjectInteractionInBalloon;
        set => _spriteObjectInteractionInBalloon = value;
    }
    

    public void Execute(Player player)
    {
        if (Prerequisite(player) == false)
        {
            Debug.Log($"Prerequisite was not met for this interaction on gameObject '{gameObject.name}' to execute", this);
            return;
        }

        SpecificAction(player);
        
        Debug.Log("Interaction Executed");

        Chain chain = _useNewChain ? GameManager.Instance.Chain : new Chain(true);
        
        _chainActions.ForEach(x =>chain.AddAction(x));
        _instantActions.ForEach(x => x.Execute());
        chain.StartNextChainAction();
    }

    /// <summary>
    /// To override
    /// </summary>
    /// <param name="player"></param>
    protected virtual void SpecificAction(Player player)
    {
        
    }
    
    /// <summary>
    /// Prerequisite that needs to be met for this Interaction to be executed.
    /// </summary>
    /// <returns></returns>
    protected virtual bool Prerequisite(Player player)
    {
        return true;
    }
}
