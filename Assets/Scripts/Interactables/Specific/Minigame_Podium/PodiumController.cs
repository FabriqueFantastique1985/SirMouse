using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PodiumController : MiniGame
{
    [SerializeField]
    List<ButtonPodium> _buttonsPodium = new List<ButtonPodium>();

    [SerializeField]
    private float _noInputTimer;

    private int _buttonClickedAmount;

    [SerializeField]
    private Canvas _canvas;

    private void Start()
    {
        foreach(var button in _buttonsPodium)
        {
            button.OnButtonClicked += ButtonClicked;
        }

        _canvas.worldCamera = Camera.main;
        //_canvas.enabled = false;
        _canvas.gameObject.SetActive(false);
    }

    private void ButtonClicked()
    {
        ++_buttonClickedAmount;
        if (_buttonClickedAmount < 4)
        {
            StartCoroutine(ClickTimer());
            return;
        }
        OnButtonsClicked();
    }

    public override void StartMiniGame()
    {
        base.StartMiniGame();
        StartCoroutine(ClickTimer());
        _canvas.gameObject.SetActive(true);
    }

    private IEnumerator ClickTimer()
    {
        int amount = _buttonClickedAmount;
        yield return new WaitForSeconds(_noInputTimer);

        if (amount == _buttonClickedAmount)
        {
            EndMiniGame(false);
        }
    }

    private void EndMinigame(bool hasSucceeded)
    {
        base.EndMiniGame(hasSucceeded);
        _canvas.gameObject.SetActive(false);
    }

    private void OnButtonsClicked()
    {
        EndMinigame(true);
    }
}
