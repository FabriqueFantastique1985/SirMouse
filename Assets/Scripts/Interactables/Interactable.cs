using UnityEngine;

public class Interactable : MonoBehaviour
{
   [SerializeField]
   private InteractBalloon _balloon;

   //[SerializeField]
  // private InteractBalloon _interactBalloon;
   
   private float _minimumSqrDistanceToInteract;
   
   private void Start()
   {
      _balloon.OnBalloonClicked += OnInteractBalloonClicked;
      _balloon.gameObject.SetActive(false);
   }

   private void OnInteractBalloonClicked(InteractBalloon sender)
   {
      Debug.Log("Interacted with: " + sender.gameObject.name);
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
