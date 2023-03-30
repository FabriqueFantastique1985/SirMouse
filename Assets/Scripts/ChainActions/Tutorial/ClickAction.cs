using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class ClickAction : ChainActionMonoBehaviour
{
    [SerializeField] private RectTransform _focusMask;
    [SerializeField] private LayerMask _layerToClickOn;

    private TutorialSystem _tutorialSystem;

    private void Start()
    {
        _startMaxTime = Mathf.Infinity;
    }

    public override void OnEnter()
    {
        base.OnEnter();

        // Subscribe to click event
        _tutorialSystem = GameManager.Instance.CurrentGameSystem as TutorialSystem;
        if (_tutorialSystem != null)
        {
            _tutorialSystem.OnClick += OnClick;
        }
    }

    private void OnClick(LayerMask layer, Vector3 mousePos, Vector3 hitPoint)
    {
        var posInImage = mousePos - _focusMask.transform.position;

        // Check if mousepos is inside of mask
        if (posInImage.x >= -(_focusMask.rect.width / 2f) && posInImage.x < _focusMask.rect.width / 2f &&
            posInImage.y >= -(_focusMask.rect.height / 2f) && posInImage.y < _focusMask.rect.height / 2f)
        {
            // Check layer bitflag for correct layer clicked
            if (((1 << layer) & _layerToClickOn) != 0)
            {
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
    }
}
