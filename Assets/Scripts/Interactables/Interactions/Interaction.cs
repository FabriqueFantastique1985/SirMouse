using System;
using System.Collections;
using System.Collections.Generic;
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
    private List<UnityEvent> _completeActions;
    
    //public Sprite InteractionSprite => _interactionSprite;
    public GameObject SpriteObjectInteractionBalloon => _spriteObjectInteractionInBalloon;
    

    public void Execute(Player player)
    {
        if (Prerequisite(player) == false)
        {
            Debug.Log($"Prerequisite was not met for this interaction on gameObject '{gameObject.name}' to execute", this);
            return;
        }

        SpecificAction(player);
        
        Debug.Log("Interaction Executed");
        _completeActions.ForEach(x => x?.Invoke());
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
