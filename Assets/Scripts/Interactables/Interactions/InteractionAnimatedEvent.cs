using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionAnimatedEvent : Interaction
{
    [SerializeField]
    protected Collider _interactableTrigger;

    [SerializeField]
    protected Animation _animationcomponent;

    [SerializeField]
    protected bool _oneTimeUse;

    [SerializeField]
    protected float _cooldownTime;

    protected override void SpecificAction(Player player)
    {
        _animationcomponent.Play();

        if (_oneTimeUse == true)
        {
            // disable collider
            _interactableTrigger.enabled = false;
        }
        else
        {
            StartCoroutine(ActivateCooldown());
        }      
    }

    protected override bool Prerequisite(Player player)
    {
        return player.EquippedItem == null;
    }


    private IEnumerator ActivateCooldown()
    {
        _interactableTrigger.enabled = false;

        yield return new WaitForSeconds(_cooldownTime);

        _interactableTrigger.enabled = true;
    }
}
