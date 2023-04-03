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

    private void Awake()
    {
        _tutorialData.IsTutorialFinished = false;
    }

    private void OnBecameVisible()
    {
        if (!_tutorialData.IsTutorialFinished && !_isTutorialPlaying)
        {
            _isTutorialPlaying = true;

            // Instantiate object with steps to go through tutorial
            var obj = Instantiate(_tutorialData.TutorialObject, gameObject.transform.position, Quaternion.identity);

            // Get step tracker and listen to event
            _stepTracker = obj.GetComponent<StepTracker>();
            _stepTracker.OnStepsCompleted += OnTutorialFinished;

            OnTutorialStarted?.Invoke();

            // Enter tutorial system
            GameManager.Instance.EnterTutorialSystem();
        }
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
