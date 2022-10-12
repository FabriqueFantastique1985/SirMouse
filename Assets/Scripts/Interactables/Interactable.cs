using System;
using UnityEngine;

public class Interactable : MonoBehaviour
{
   [SerializeField]
   private InteractBalloon _balloon;

  private void Start()
   {
      _balloon.OnBalloonClicked += OnInteractBalloonClicked;
      _balloon.gameObject.SetActive(false);

      InitializeThings();
   }

    protected virtual void InitializeThings()
    {
        // extra method that inheriting classes can use to still use the Start function
    }

    protected virtual void OnInteractBalloonClicked(InteractBalloon sender, Player player)
   {
      Debug.Log("Interacted with: " + sender.gameObject.name + " by player:" + player.gameObject.name);
   }

   private void OnTriggerEnter(Collider other)
   {
      var player = other.transform.GetComponent<Player>();
      if (player != null)
      {
         _balloon.gameObject.SetActive(true);
      }
   }

   private void OnTriggerExit(Collider other)
   {
      var player = other.transform.GetComponent<Player>();
      if (player != null)
      {
         _balloon.gameObject.SetActive(false);
      }
   }
}
