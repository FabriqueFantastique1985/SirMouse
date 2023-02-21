using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class FallingPancake : MonoBehaviour
{
    [SerializeField]
    private float _fallDistance;
    [SerializeField]
    private float _fallSpeed;

    [SerializeField]
    private List<Sprite> _sprites;

    private Vector3 _endPosition;

    public float FallDistance
    {
        get { return _fallDistance; }
        set
        {
            if (value > 0f)
            {
                _fallDistance = value;
            }
        }
    }

    private void Start()
    {
        Assert.AreNotEqual(_fallDistance, 0f, "Fall distance was not set before pancake was spawned");
        _endPosition = transform.position;
        _endPosition.y -= _fallDistance;

        // Use random sprite out of list

        StartCoroutine(Fall());
    }

    private void OnTriggerEnter(Collider other)
    {
        // check if hit pan

        // if hit pan
        //      notify point gained
        //      destroy self
        // else continue
    }

    private IEnumerator Fall()
    {
        // Code from Unity documentation

        while (Vector3.Distance(transform.position, _endPosition) > 0.001f)
        {
            // Move our position a step closer to the target.
            var step = _fallSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, _endPosition, step);

            yield return null;
        }

        // Touched the ground
        // 1. notify life lost

        Destroy(gameObject);
    }
}
