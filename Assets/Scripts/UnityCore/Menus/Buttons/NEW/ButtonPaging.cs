using System.Collections;
using System.Collections.Generic;
using UnityCore.Audio;
using UnityCore.Menus;
using UnityEngine;

public class ButtonPaging : ButtonBaseNew
{
    [SerializeField]
    protected AudioElement _soundEffectOff;

    [SerializeField]
    protected PageType _turnThisPage;

    protected PageController _pageInstance;


    protected override void Start()
    {
        base.Start();

        //if (_soundEffectOff.Clip != null)
        //{
        //    _audioInstance.AddAudioElement(_soundEffectOff);
        //}

        _pageInstance = PageController.Instance;
    }



    public override void ClickedButton()
    {
        base.ClickedButton();

        TurnOnPage();
    }



    protected override void PlaySoundEffect()
    {
        // if this page is alrdy on...
        if (PageController.Instance.PageIsOn(_turnThisPage) == true)
        {
            _audioInstance.PlayAudio(_soundEffectOff);
        }
        else
        {
            _audioInstance.PlayAudio(_soundEffect);
        }
    }
    protected virtual void TurnOnPage()
    {
        if (_pageInstance.PageIsOn(_turnThisPage) == true)
        {
            _pageInstance.TurnPageOff(_turnThisPage);

            if (DoIHaveActivePages() == false)
            {
                GameManager.Instance.BlockInput = false;
            }
        }
        else
        {
            _pageInstance.TurnPageOn(_turnThisPage);

            GameManager.Instance.BlockInput = true;
        }
    }

    protected virtual bool DoIHaveActivePages()
    {
        for (int i = 0; i < _pageInstance.PagesScene.Length; i++)
        {
            // if even a single page is on ---> do not re-enable input
            if (_pageInstance.PagesScene[i].isOn == true)
            {
                return true;
            }          
        }

        return false;
    }
}
