using System.Collections;
using System.Collections.Generic;
using UnityCore.Audio;
using UnityEngine;
using UnityEngine.UI;

public class ButtonEquipToggle : ButtonBaseNew
{
    [SerializeField]
    protected string _animationNameAppear;

    [Header("adittional audio")]
    [SerializeField]
    protected AudioElement _soundEffectDifferentState;

    [Header("Components")]
    public Button ButtonComponent;
    public Collider ColliderComponent;

    [Header("Reference children")]
    [SerializeField]
    private GameObject _buttonState0;
    [SerializeField]
    private GameObject _buttonState1;
    [SerializeField]
    private List<GameObject> _instrumentsInButtonNormal = new List<GameObject>();
    [SerializeField]
    private List<GameObject> _instrumentsInButtonBlue = new List<GameObject>();


    protected override void Start() // this start needs to happen after the InstrumentController start
    {
        base.Start();

        // SAVE DATA !!!!


        // if I have found an instrument -> show the button
        if (InstrumentController.Instance.CheckIfFoundInstrument() == true)
        {
            this.gameObject.SetActive(true);

            SetButtonState(true);
            SetInstrumentSprite();
        }
        else
        {
            this.gameObject.SetActive(false);
        }
    }


    public override void ClickedButton()
    {
        PlayAnimationPress();

        // additional equip logic
        if (InstrumentController.Instance.ActiveInstrumentPiece == null)
        {
            Debug.Log("not instrument has been activated, so pressing me does nothing");
        }
        else
        {
            if (InstrumentController.Instance.EquipedInstrument == Type_Instrument.None)
            {
                GameManager.Instance.Player.PushState(new InstrumentEquipState(GameManager.Instance.Player, InstrumentController.Instance.ActiveInstrumentPiece.InstrumentType));
                PlaySoundEffectMultiple(_soundEffect);
            }
            else
            {
                GameManager.Instance.Player.PushState(new InstrumentUnequipState(GameManager.Instance.Player));
                PlaySoundEffectMultiple(_soundEffectDifferentState);
            }

            SetInstrumentSprite();
        }

    }

    public void SetInstrumentSprite()
    {
        // disable instrument visual in button toggle
        for (int i = 0; i < _instrumentsInButtonNormal.Count; i++)
        {
            _instrumentsInButtonNormal[i].SetActive(false);
            _instrumentsInButtonBlue[i].SetActive(false);
        }
        // activate correct instrument visual
        _instrumentsInButtonNormal[((int)InstrumentController.Instance.ActiveInstrumentPiece.InstrumentType) - 1].SetActive(true);
        _instrumentsInButtonBlue[((int)InstrumentController.Instance.ActiveInstrumentPiece.InstrumentType) - 1].SetActive(true);
    }
    public void SetButtonState(bool showNormalButton)
    {
        if (showNormalButton == true)
        {
            _buttonState0.SetActive(true);
            _buttonState1.SetActive(false);
        }
        else
        {
            _buttonState1.SetActive(true);
            _buttonState0.SetActive(false);
        }
    }

    private void OnEnable()
    {
        _animationComponent.Play(_animationNameAppear);
    }
}
