using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
   [SerializeField]
   private Collider _collider;

   [SerializeField]
   private Transform _interactTransform;

   [SerializeField]
   private float _minimumDistanceToInteract = 0.05f;

   //[SerializeField]
  // private InteractBalloon _interactBalloon;
   

   private float _minimumSqrDistanceToInteract;
   
   private void Start()
   {
      _minimumSqrDistanceToInteract = _minimumDistanceToInteract * _minimumDistanceToInteract;
   }

   private void OnTriggerEnter(Collider other)
   {
      var player = other.transform.GetComponent<Player>();
      if (player != null)
      {
         
      }
   }

   public virtual void Interact(Player controller)
   {
      float sqrDistance = (_interactTransform.position - controller.transform.position).sqrMagnitude;
      if (sqrDistance > _minimumSqrDistanceToInteract)
      {
         controller.SetTarget(_interactTransform.position);
      }
      else
      {
         
      }
   }
}
