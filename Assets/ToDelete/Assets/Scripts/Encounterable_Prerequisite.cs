using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Encounterable_Prerequisite : Encounterable
{
    [SerializeField] private Interactable _requiredItem;
    private bool _triggerEntered = true;

    //protected override void GenericBehaviour()
    //{
    //    base.GenericBehaviour();
       
    //}

    //protected override void OnTriggerEnter(Collider other)
    //{
    //    //if (other.gameObject.layer.ToString() == "Player")
    //    //{
    //    //    _triggerEntered = true;
    //    //}
    //}

    protected override void OnTriggerEnter(Collider other)
    {
        //if (GameManager.Instance.Player.EquippedItem == _requiredItem)
        //{
            base.OnTriggerEnter(other);
        //}
        if (GameManager.Instance.Player.EquippedItem == _requiredItem)
        {

            // Make all animations stop
            Animation[] animations;
            animations = GetComponentsInChildren<Animation>();

            foreach (Animation animation in animations)
            {
                animation.enabled = false;
            }

            // Enable gravity on all rigidbodies
            Rigidbody[] rigidbodies;
            rigidbodies = GetComponentsInChildren<Rigidbody>();

            foreach (Rigidbody rigidbody in rigidbodies)
            {
                rigidbody.useGravity = true;
            }
        }
    }
}