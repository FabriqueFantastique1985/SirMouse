using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimaterEventController : MonoBehaviour
{
    [SerializeField]
    private GameObject _objectToInfluence;

    public void EnableObject()
    {
        _objectToInfluence.SetActive(true);
    }

    public void DisableObject()
    {
        _objectToInfluence.SetActive(false);
    }
}
