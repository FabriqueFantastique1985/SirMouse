using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionAction : ChainActionMonoBehaviour
{
   [SerializeField]
   private Interactable _interactable;

   private void Awake()
   {
      _maxTime = Mathf.Infinity;
      _interactable.OnInteracted += OnInteracted;
   }

   private void OnInteracted()
   {
      _maxTime = -1.0f;
   }
}
