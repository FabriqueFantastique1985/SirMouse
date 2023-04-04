using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialFocusObject : MonoBehaviour
{
    public delegate void TutorialFocalDelegate();
    public event TutorialFocalDelegate OnTutorialStarted;

    private static bool _isTutorialPlaying = false;

    [SerializeField] private TutorialData _tutorialData;
    private StepTracker _stepTracker;

    private float _screenBorder = .2f;

    private void Update()
    {
        // Disable update if this tutorial is completed
        if (_tutorialData.IsTutorialFinished)
        {
            enabled = false;
            return;
        }

        // Do not start tutorial if another tutorial is playing
        if (_isTutorialPlaying)
        {
            return;
        }

        // Check if object is within borders of screen
        Vector3 screenPoint = Camera.main.WorldToViewportPoint(transform.position);
        bool isOnScreen = screenPoint.x > _screenBorder && screenPoint.x < 1f - _screenBorder && screenPoint.y > _screenBorder && screenPoint.y < 1f - _screenBorder;
        if (isOnScreen)
        {
            PlayTutorial();
        }
    }

    private void PlayTutorial()
    {
        _isTutorialPlaying = true;

        // Instantiate object with steps to go through tutorial
        var obj = Instantiate(_tutorialData.TutorialObject, gameObject.transform.position, Quaternion.identity);

        // Get step tracker and listen to event
        _stepTracker = obj.GetComponent<StepTracker>();
        _stepTracker.OnStepsCompleted += OnTutorialFinished;

        OnTutorialStarted?.Invoke();

        // Stop player movement when entering tutorial
        GameManager.Instance.Player.Agent.SetDestination(GameManager.Instance.Player.gameObject.transform.position);

        // Enter tutorial system
        GameManager.Instance.EnterTutorialSystem();
    }

    private void OnTutorialFinished()
    {
        // Unsubscribe from event
        _stepTracker.OnStepsCompleted -= OnTutorialFinished;

        // Update tutorial data
        _tutorialData.IsTutorialFinished = true;
        _isTutorialPlaying = false;

        // Deactivate step tracker
        _stepTracker.gameObject.SetActive(false);

        GameManager.Instance.ExitTutorialSystem();
    }
}
