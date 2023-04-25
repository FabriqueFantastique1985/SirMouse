using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuPlayAnimations : MonoBehaviour
{
    [Header("starting animation ?")]
    [SerializeField]
    private bool _IAmFirstAnimation;

    [Header("My Animation Component")]
    [SerializeField]
    private Animation _myAnimation;

    [Header("How long does the next animation have to wait before it starts")]
    [SerializeField]
    private float _waitTimeToStartNextAnimation;

    [Header("Script to call new coroutine from")]
    [SerializeField]
    private MainMenuPlayAnimations _animationPlayerScriptToTrigger;


    void Start()
    {
        if (_IAmFirstAnimation == true && _animationPlayerScriptToTrigger != null)
        {
            StartCoroutine(PlayAnimations());
        }
        else if (_IAmFirstAnimation == true)
        {
            Debug.Log("I DONT HAVE A SCRIPT REFERENCE I NEED");
        }
    }

    public IEnumerator PlayAnimations()
    {
        this.gameObject.SetActive(true);
        _myAnimation.Play();

        Debug.Log("playing animation on " + this.gameObject.name);

        if (_animationPlayerScriptToTrigger != null)
        {
            yield return new WaitForSeconds(_waitTimeToStartNextAnimation);

            //_animationPlayerScriptToTrigger.StartCoroutine(_animationPlayerScriptToTrigger.PlayAnimations());
            StartCoroutine(_animationPlayerScriptToTrigger.PlayAnimations());
        }
        else
        {
            yield return null;
        }
    }
}
