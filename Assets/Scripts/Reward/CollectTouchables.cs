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

        // Raycast to check if mouse is above chest
        Ray ray = Camera.allCameras[0].ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        LayerMask mask = LayerMask.GetMask("Collect");

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, mask))
        {
            if (hit.collider.gameObject == gameObject)
            {
                Destroy(obj.gameObject);
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
