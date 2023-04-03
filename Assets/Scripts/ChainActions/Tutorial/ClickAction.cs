using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Playables;

public class ClickAction : ChainActionMonoBehaviour
{
    [SerializeField] private TutorialFocusMask _focusMask;
    [SerializeField] private LayerMask _layerToClickOn;

    private TutorialSystem _tutorialSystem;

    private void Start()
    {
        _startMaxTime = Mathf.Infinity;
    }

    public override void OnEnter()
    {
        base.OnEnter();

        _focusMask.Initialize();

        // Subscribe to click event
        _tutorialSystem = GameManager.Instance.CurrentGameSystem as TutorialSystem;
        if (_tutorialSystem != null)
        {
            _tutorialSystem.OnClick += OnClick;
        }
    }

    private void OnClick(RaycastHit hit, Vector3 mousePos)
    {
        var posInImage = mousePos - _focusMask.Mask.transform.position;

        // Check if mousepos is inside of mask
        if (posInImage.x >= -(_focusMask.Mask.rect.width / 2f) && posInImage.x < _focusMask.Mask.rect.width / 2f &&
            posInImage.y >= -(_focusMask.Mask.rect.height / 2f) && posInImage.y < _focusMask.Mask.rect.height / 2f)
        {
            // Check layer bitflag for correct layer clicked
            if (((1 << hit.collider.gameObject.layer) & _layerToClickOn) != 0)
            {
                if (hit.transform.TryGetComponent<IClickable>(out IClickable clickable))
                {
                    clickable.Click(GameManager.Instance.Player);
                }

                _maxTime = -1f;
            }
        }
    }

    public override void Execute()
    {
        base.Execute();
    }

    public override void OnExit()
    {
        base.OnExit();
        if (_tutorialSystem != null)
        {
            _tutorialSystem.OnClick -= OnClick;
        }
    }
}
