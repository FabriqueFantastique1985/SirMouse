using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// interaction on pileLeaves -> confetti
public class InteractionNeedyTouchableCompleted : Interaction
{
    [Header("interactable to reset")]
    [SerializeField]
    private InteractableNeedyTouchables _interactableToReset;

    [Header("Confetti to play")]
    [SerializeField]
    private Type_Confetti _confettiToPlay;


    // play specific confetti effect
    // reset the InteractableNeedyTouchable so the loop can be done again

    protected override void SpecificAction(Player player)
    {
        base.SpecificAction(player);

        // play confetti
        RewardController.Instance.PlayConfetti(_confettiToPlay);

        // reset the previous interactable
        if (_interactableToReset.ResetAble == true)
        {
            _interactableToReset.ResetMyInteractable();
        }       
    }
}
