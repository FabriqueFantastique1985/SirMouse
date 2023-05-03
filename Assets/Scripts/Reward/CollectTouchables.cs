using System;
using System.Collections.Generic;
using System.Linq;
using UnityCore.Audio;
using UnityEngine;

public class CollectTouchables : MonoBehaviour, IDataPersistence
{
    [SerializeField] private SpriteRenderer _insideSpriteRenderer;
    [SerializeField] private SpriteRenderer _topSpriteRenderer;
    [SerializeField] private Sprite _topOpen;
    [SerializeField] private Sprite _topClosed;
    [SerializeField] private Material _baseMaterial;
    [SerializeField] private Material _outlineMaterial;

    [SerializeField] private GameObject _gradient;

    [Header("Reward")]
    [SerializeField] private List<SkinPieceElement> _skinsToReward;

    private int _toCollectAmount;
    private int _collectedAmount = 0;

    bool _hasCollectedEverything = false;

    [SerializeField] private AudioElement _openSound;
    [SerializeField] private AudioElement _closeSound;
    [SerializeField] private AudioElement _DropInSound;

    private struct CleanableObject
    {
        public CleanableObject(Touch_Move touchable, bool hasBeenCleaned)
        {
            Touchable = touchable;
            HasBeenCleaned = hasBeenCleaned;
        }

        public Touch_Move Touchable;
        public bool HasBeenCleaned;
    }

    private Dictionary<string, CleanableObject> _cleanableOjects = new Dictionary<string, CleanableObject>();

    private void Start()
    {
        InitializeCleanables();

        _insideSpriteRenderer.material = _baseMaterial;
        _topSpriteRenderer.material = _baseMaterial;
        _gradient.SetActive(false);

        _toCollectAmount = _cleanableOjects.Count();
    }

    private void InitializeCleanables()
    {
        if (_cleanableOjects.Count > 0 || _hasCollectedEverything)
        {
            return;
        }

        var moveObjects = FindObjectsOfType<Touch_Move>();
        foreach (var moveObject in moveObjects)
        {
            moveObject.OnPickup += OnPickup;
            moveObject.OnDrop += OnDrop;

            if (moveObject.TryGetComponent(out ID id))
            {
                _cleanableOjects.Add(id, new CleanableObject(moveObject, false));
            }
        }
        _toCollectAmount = moveObjects.Count();
    }

    private void OnDestroy()
    {
        foreach (var cleanable in _cleanableOjects)
        {
            if (!cleanable.Value.HasBeenCleaned)
            {
                cleanable.Value.Touchable.OnPickup -= OnPickup;
                cleanable.Value.Touchable.OnDrop -= OnDrop;
            }
        }
    }

    private void OnPickup(Touch_Move obj)
    {
        // hightlight box
        _insideSpriteRenderer.material = _outlineMaterial;
        _topSpriteRenderer.material = _outlineMaterial;
        _gradient.SetActive(true);
        _topSpriteRenderer.sprite = _topOpen;
        AudioController.Instance.PlayAudio(_openSound);
    }

    private void OnDrop(Touch_Move obj)
    {
        // remove hightlight box
        _insideSpriteRenderer.material = _baseMaterial;
        _topSpriteRenderer.material = _baseMaterial;
        _gradient.SetActive(false);
        _topSpriteRenderer.sprite = _topClosed;
        AudioController.Instance.PlayAudio(_closeSound);


        // Raycast to check if mouse is above chest
        Ray ray = Camera.allCameras[0].ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        LayerMask mask = LayerMask.GetMask("Collect");

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, mask))
        {
            if (hit.collider.gameObject == gameObject)
            {
                ++_collectedAmount;
                AudioController.Instance.PlayAudio(_DropInSound);

                if (!_hasCollectedEverything && _collectedAmount >= _toCollectAmount && _skinsToReward.Count > 0) 
                {
                    RewardController.Instance.GiveReward(_skinsToReward);
                    _hasCollectedEverything = true;
                }

                // Set HasBeenCleaned to true
                if (obj.TryGetComponent(out ID id))
                {
                    _cleanableOjects[id] = new CleanableObject(_cleanableOjects[id].Touchable, true);
                }

                // Unsubscribe to event calls
                obj.OnPickup -= OnPickup;
                obj.OnDrop -= OnDrop;

                Destroy(obj.gameObject);
            }
        }
    }

    private void RemoveAll<TKey, TValue>(IDictionary<TKey, TValue> dict, Func<TValue, bool> predicate)
    {
        // https://stackoverflow.com/questions/469202/best-way-to-remove-multiple-items-matching-a-predicate-from-a-net-dictionary
        var keys = dict.Keys.Where(k => predicate(dict[k])).ToList();
        foreach (var key in keys)
        {
            dict.Remove(key);
        }
    }

    public void LoadData(GameData data)
    {
        InitializeCleanables();

        foreach (var cleanable in data.CleanedTouchables)
        {
            if (_cleanableOjects.ContainsKey(cleanable.Key))
            {
                bool hasBeenCleaned = cleanable.Value;
                _cleanableOjects[cleanable.Key] = new CleanableObject(_cleanableOjects[cleanable.Key].Touchable, hasBeenCleaned);
                if (hasBeenCleaned)
                {
                    Destroy(_cleanableOjects[cleanable.Key].Touchable.gameObject);
                }
            }
        }

        RemoveAll(_cleanableOjects, x => x.HasBeenCleaned);
        if (_cleanableOjects.Count == 0)
        {
            _hasCollectedEverything = true;
        }
    }

    public void SaveData(ref GameData data)
    {
        foreach (var cleanable in _cleanableOjects)
        {
            if (string.IsNullOrEmpty(cleanable.Key))
            {
                Debug.LogWarning("No id yet made! Please generate one!");
                return;
            }

            data.CleanedTouchables[cleanable.Key] = cleanable.Value.HasBeenCleaned;
        }

    }
}
