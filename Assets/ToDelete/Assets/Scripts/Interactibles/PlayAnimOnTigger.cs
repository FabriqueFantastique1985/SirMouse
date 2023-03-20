using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAnimOnTigger : MonoBehaviour
{
    public Animator animator;
    public string triggerName;

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject == GameManager.Instance.Player.gameObject)
        {
            animator.SetTrigger(triggerName);
        }
    }
}