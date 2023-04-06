using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Playables;

public class ClickAction : ChainActionMonoBehaviour
{
    [SerializeField] private RectTransform _focusMask;
    [SerializeField] private LayerMask _layerToClickOn;
    [SerializeField] private Transform _focus;

    private TutorialSystem _tutorialSystem;
    private TutorialFocusMask _tutorialFocus;

    private void Start()
    {
        _startMaxTime = Mathf.Infinity;
        _tutorialFocus =  new TutorialFocusMask();  
        enabled = false;
    }

    private void Update()
    {
        if (_focus)
        {
            _focusMask.position = _tutorialFocus.GetWorldPosToCameraPos(_focus.transform.position);
        }
    }

    public override void OnEnter()
    {
        base.OnEnter();

        enabled = true;
        _tutorialFocus.Initialize(ref _focusMask);

        // Subscribe to click event
        _tutorialSystem = GameManager.Instance.CurrentGameSystem as TutorialSystem;
        if (_tutorialSystem != null)
        {
            _tutorialSystem.OnClick += OnClick;
        }
    }

    private void OnClick(RaycastHit hit, Vector3 mousePos)
    {
        //var posInImage = mousePos - Camera.allCameras[0].WorldToScreenPoint(_focusMask.transform.position);
        var posInImage = mousePos - _focusMask.transform.position;

        // Check if mousepos is inside of mask
        if (posInImage.x >= -(_focusMask.rect.width / 2f) && posInImage.x < _focusMask.rect.width / 2f &&
            posInImage.y >= -(_focusMask.rect.height / 2f) && posInImage.y < _focusMask.rect.height / 2f)
        {
            // Check layer bitflag for correct layer clicked
            if (((1 << hit.collider.gameObject.layer) & _layerToClickOn) != 0)
            {
                if (hit.transform.TryGetComponent<IClickable>(out IClickable clickable))
                {
                    clickable.Click(GameManager.Instance.Player);
                    _maxTime = -1f;
                }
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
        enabled = false;
            
        if (_tutorialSystem != null)
        {
            _tutorialSystem.OnClick -= OnClick;
        }
    }
}
