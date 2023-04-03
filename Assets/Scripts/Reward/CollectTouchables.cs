using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class CollectTouchables : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Material _baseMaterial;
    [SerializeField] private Material _outlineMaterial;

    [SerializeField] private GameObject _gradient;

    [Header("Reward")]
    [SerializeField] private List<SkinPieceElement> _skinsToReward;

    private List<GameObject> _enteredGameObjects = new List<GameObject>();

    private int _toCollectAmount;
    private int _collectedAmount = 0;

    bool _hasCollectedEverything = false;

    private void Start()
    {
        var moveObjects = FindObjectsOfType<Touch_Move>();
        foreach (var moveObject in moveObjects)
        {
            moveObject.OnPickup += OnPickup;
            moveObject.OnDrop += OnDrop;
        }
        _toCollectAmount = moveObjects.Count();

        _spriteRenderer.material = _baseMaterial;
        _gradient.SetActive(false);
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
        _gradient.SetActive(true);
    }

    private void OnDrop(Touch_Move obj)
    {
        // remove hightlight box
        _spriteRenderer.material = _baseMaterial;
        _gradient.SetActive(false);

        // Raycast to check if mouse is above chest
        Ray ray = Camera.allCameras[0].ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        LayerMask mask = LayerMask.GetMask("Collect");

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, mask))
        {
            if (hit.collider.gameObject == gameObject)
            {
                ++_collectedAmount;

                if (!_hasCollectedEverything && _collectedAmount >= _toCollectAmount && _skinsToReward.Count > 0) 
                {
                    RewardController.Instance.GiveReward(_skinsToReward);
                    _hasCollectedEverything = true;
                }

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
