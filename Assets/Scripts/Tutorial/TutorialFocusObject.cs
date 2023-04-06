using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class TutorialFocusObject : MonoBehaviour
{
    public delegate void TutorialFocalDelegate();
    public event TutorialFocalDelegate OnTutorialStarted;

    private static bool _isTutorialPlaying = false;

    [SerializeField] private TutorialData _tutorialData;
    private StepTracker _stepTracker;

    private TutorialActivateEvent _activateEvent;

    private void Start()
    {
        _activateEvent = GetComponent<TutorialActivateEvent>();
        Assert.IsNotNull(_activateEvent, "Event was null in " + gameObject.name);
        _activateEvent.OnTutorialActivate += PlayTutorial;
    }

    private void PlayTutorial()
    {
        // Disable update if this tutorial is completed or if another tutorial is playing
        if (TutorialTracker.Instance.IsTutorialComplete(_tutorialData) || _isTutorialPlaying)
        {
            return;
        }

        _isTutorialPlaying = true; 

        // Unsubscribe from event
        _activateEvent.OnTutorialActivate -= PlayTutorial;
        _activateEvent.TutorialActivated();

        OnTutorialStarted?.Invoke();

        // Instantiate object with steps to go through tutorial
        var obj = Instantiate(_tutorialData.TutorialObject, Camera.allCameras[0].transform.position, Camera.allCameras[0].transform.rotation, Camera.allCameras[0].transform);

        // Get step tracker and listen to event
        _stepTracker = obj.GetComponent<StepTracker>();
        _stepTracker.OnStepsCompleted += OnTutorialFinished;

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
        TutorialTracker.Instance.CompletedTutorial(_tutorialData);
        _isTutorialPlaying = false;

        // Deactivate step tracker
        _stepTracker.gameObject.SetActive(false);

        GameManager.Instance.ExitTutorialSystem();
    }
}
