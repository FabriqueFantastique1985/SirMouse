using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableStepHolder : MonoBehaviour
{
    private TutorialFocusObject _focusObject;

    private void Start()
    {
        _focusObject = GetComponent<TutorialFocusObject>();
        _focusObject.OnTutorialStarted += OnTutorialStarted;
    }

    private void OnTutorialStarted()
    {
        _focusObject.OnTutorialStarted -= OnTutorialStarted;

        var stepHolder = gameObject.GetComponentInChildren<StepHolder>(true);
        stepHolder.gameObject.SetActive(true);
    }
}
