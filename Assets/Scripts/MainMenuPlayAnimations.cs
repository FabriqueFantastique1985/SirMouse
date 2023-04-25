using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuPlayAnimations : MonoBehaviour
{
    [SerializeField]
    private float _durationIntro;

    [SerializeField]
    private Animation _animationComponentMainMenu;


    void Start()
    {
        StartCoroutine(PlayAnimations());
    }

    private IEnumerator PlayAnimations()
    {
        yield return new WaitForSeconds(_durationIntro);

        _animationComponentMainMenu.enabled = true;
    }
}
