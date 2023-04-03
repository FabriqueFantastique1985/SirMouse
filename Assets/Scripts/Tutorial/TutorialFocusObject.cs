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

    private SpriteRenderer _spriteRenderer;

    private float _screenBorder = .2f;

    private void Awake()
    {
        _tutorialData.IsTutorialFinished = false;
        _spriteRenderer = GetComponent<SpriteRenderer>();   
    }

    private void Update()
    {
        Vector3 screenPoint = Camera.main.WorldToViewportPoint(transform.position);
        bool onScreen = screenPoint.x > _screenBorder && screenPoint.x < 1f - _screenBorder && screenPoint.y > _screenBorder && screenPoint.y < 1f - _screenBorder;
        if (onScreen && _spriteRenderer.isVisible)
        {
            if (!_tutorialData.IsTutorialFinished)
            {
                StartCoroutine(WaitForTutorial());
            }
        }
    }

    private IEnumerator WaitForTutorial()
    {
        // Wait for other tutorials to finish
        while (_isTutorialPlaying)
        {
            yield return null;
        }

        // Check if completed tutorial is not this tutorial
        if (_tutorialData.IsTutorialFinished)
        {
            _stepTracker.gameObject.SetActive(false);
            yield break;
        }

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
