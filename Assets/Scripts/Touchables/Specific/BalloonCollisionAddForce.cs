using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonCollisionAddForce : MonoBehaviour
{
    [SerializeField]
    private Rigidbody _rigidBodyToInfluence;

    [SerializeField]
    private float _forceStrength = 2;


    // if I collide with the player....
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 10)
        {
            Vector3 bounceDirection;
            Vector3 rotateDirection;
            if (GameManager.Instance.Player.Character.transform.localScale.x < 0)
            {
                // move balloon right
                bounceDirection = Vector3.right * (_forceStrength / 5f);
                rotateDirection = Vector3.forward * _forceStrength;
            }
            else
            {
                bounceDirection = Vector3.left * (_forceStrength / 5f);
                rotateDirection = Vector3.back * _forceStrength;
            }

            Vector3 bounceUp = Vector3.up * _forceStrength;

            _rigidBodyToInfluence.velocity = (bounceUp + bounceDirection);

            _rigidBodyToInfluence.AddRelativeTorque(rotateDirection);
        }
    }
}
