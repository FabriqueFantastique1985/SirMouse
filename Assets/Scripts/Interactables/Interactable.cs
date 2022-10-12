using System;
using System.Collections;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField]
    private InteractBalloon _balloon;

    [SerializeField]
    private Collider _balloonTrigger;

    [SerializeField]
    private Animator _balloonAnimator;

    private const string _animFloat = "Balloon_Floaty";
    private const string _animPop = "Balloon_Pop";

    private void Start()
    {
        _balloon.OnBalloonClicked += OnInteractBalloonClicked;
        _balloon.gameObject.SetActive(false);

        InitializeThings();
    }



    #region Virtual Functions

    protected virtual void InitializeThings()
    {
        // extra method that inheriting classes can use to still use the Start function
    }
    protected virtual void OnInteractBalloonClicked(InteractBalloon sender, Player player)
    {
        Debug.Log("Interacted with: " + sender.gameObject.name + " by player:" + player.gameObject.name);

        // what else should all balloons do ?
        // 1) they should have an animation playing on loop (floaty)
        // 2) they should all "POP" once tapped, --- only after this "POP" should the baloon object be set to false (prototype disabled the collider first, the object after the animation as a fix)
        _balloonAnimator.Play(_animPop);
        StartCoroutine(DisableBalloon());
    }

    #endregion

    #region Private Functions

    private IEnumerator DisableBalloon()
    {
        // disable the collider -> wait a bit -> disable the gameobject + enable the collider
        _balloonTrigger.enabled = false;

        yield return new WaitForSeconds(_balloonAnimator.GetCurrentAnimatorStateInfo(0).length + 0.1f);

        _balloon.gameObject.SetActive(false);
        _balloonTrigger.enabled = true;
    }

    #endregion



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
