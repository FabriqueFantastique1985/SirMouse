using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class TutorialActivateEventVisible : TutorialActivateEvent
{
    private float _screenBorder = .2f;

    [SerializeField] private List<SpriteRenderer> _spriteRenderers = new List<SpriteRenderer>();

    private void Start()
    {
        Assert.IsFalse(_spriteRenderers.Count < 0, "SpriteRenderers list is empty");   
    }

    private void Update()
    {
        // Check if object is within borders of screen
        Vector3 screenPoint = Camera.main.WorldToViewportPoint(transform.position);
        bool isOnScreen = screenPoint.x > _screenBorder && screenPoint.x < 1f - _screenBorder && screenPoint.y > _screenBorder && screenPoint.y < 1f - _screenBorder;

        // Check if any of the sprites in the spriteRenderer array are visible to a camera
        bool isVisible = false;
        foreach (var spriteRenderer in _spriteRenderers)
        {
            if (spriteRenderer.isVisible) 
            { 
                isVisible = true;
                break;
            }
        }
        
        if (isOnScreen && isVisible)
        {
            ActivateTutorial();
        }
    }

    public override void TutorialActivated()
    {
        enabled = false;
    }
}
