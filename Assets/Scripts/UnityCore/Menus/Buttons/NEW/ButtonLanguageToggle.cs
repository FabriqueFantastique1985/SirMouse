using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonLanguageToggle : ButtonMainMenuAnimationLogic
{
    [SerializeField]
    private GameObject _imageEng;
    [SerializeField]
    private GameObject _imageBel;


    public void SwapLanguageVisual(bool _currentlyInEnglish)
    {
        if (_currentlyInEnglish == true)
        {
            _imageEng.SetActive(true);
            _imageBel.SetActive(false);
        }
        else
        {
            _imageEng.SetActive(false);
            _imageBel.SetActive(true);
        }
    }



    protected override void OnEnable()
    {
        base.OnEnable();

        _imageBel.SetActive(true);
        _imageEng.SetActive(false);
    }
}
