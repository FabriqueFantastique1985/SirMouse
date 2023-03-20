using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectTouchables : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Material _baseMaterial;
    [SerializeField] private Material _outlineMaterial;

    private List<GameObject> _enteredGameObjects = new List<GameObject>();


    private void Start()
    {
        var moveObjects = FindObjectsOfType<Touch_Move>();
        foreach (var moveObject in moveObjects)
        {
            moveObject.OnPickup += OnPickup;
            moveObject.OnDrop += OnDrop;
        }
        _spriteRenderer.material = _baseMaterial;
    }

    private void OnDestroy()
    {
        var moveObjects = FindObjectsOfType<Touch_Move>();
        foreach (var moveObject in moveObjects)
        {
            moveObject.OnPickup -= OnPickup;
            moveObject.OnDrop -= OnDrop;
        }
    }

    private void OnPickup(Touch_Move obj)
    {
        // hightlight box
        _spriteRenderer.material = _outlineMaterial;
    }

    private void OnDrop(Touch_Move obj)
    {
        // remove hightlight box
        _spriteRenderer.material = _baseMaterial;

        foreach (var go in _enteredGameObjects)
        {
            if (obj.gameObject == go)
            {
                _enteredGameObjects.Remove(go);
                Destroy(go);
                break;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!_enteredGameObjects.Contains(other.gameObject))
        {
            _enteredGameObjects.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        _enteredGameObjects.Remove(other.gameObject);
    }
}
