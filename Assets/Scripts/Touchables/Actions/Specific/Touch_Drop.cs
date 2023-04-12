using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

[Serializable]
struct SpawnedGatherable
{
    public GameObject Gatherable;
    public Animator GatherableAnimator;
}

public class Touch_Drop : Touch_Action, IDataPersistence
{
    [SerializeField] private List<SpawnedGatherable> _gatherablesToSpawn = new List<SpawnedGatherable>();

    [SerializeField] private ID _id;

    private int _gatherablesSpawnedIndex = 0;
    private int _gatherablesCollectedIndex = 0;

    private ShineBehaviour _shineBehaviour;

    private bool _isCompleted = false;

    protected override void Start()
    {
        base.Start();
        foreach (var gatherable in _gatherablesToSpawn)
        {
            gatherable.Gatherable.SetActive(false);
        }
        _shineBehaviour = GetComponent<ShineBehaviour>();
    }

    public override void Act()
    {
        base.Act();

        if (_isCompleted)
        {
            return;
        }

        // Only spawn gatherables according to max amount and amount already collected
        int maxCount = _gatherablesToSpawn.Count;
        if (_gatherablesSpawnedIndex < maxCount - _gatherablesCollectedIndex)
        {
            // Spawn new gatherable and play animation
            var nextGatherable = _gatherablesToSpawn[_gatherablesSpawnedIndex];
            nextGatherable.Gatherable.SetActive(true);
            nextGatherable.GatherableAnimator?.SetTrigger("Activate");
            ++_gatherablesSpawnedIndex;

            var gatherableObject = nextGatherable.Gatherable.GetComponent<GatherableObject>();
            if(gatherableObject)
                gatherableObject.ObjectGathered += CollectedGatherable;

        }

        // Check if completed
        if (_gatherablesSpawnedIndex == _gatherablesToSpawn.Count)
        {
            _isCompleted = true;
            if (_shineBehaviour)
                _shineBehaviour.IsShineActive = false;
        }
    }

    private void OnDisable()
    {
        if (_isCompleted)
            return;

        foreach (var gatherable in _gatherablesToSpawn)
        {
            var gatherableObject = gatherable.Gatherable.GetComponent<GatherableObject>();
            if (gatherableObject)
                gatherableObject.ObjectGathered -= CollectedGatherable;
        }
    }

    public void CollectedGatherable(GatherableObject gatheredObject)
    {
        ++_gatherablesCollectedIndex;
        gatheredObject.ObjectGathered -= CollectedGatherable;
    }

    public void LoadData(GameData data)
    {
        if (data.DroppedGatherable.ContainsKey(_id))
        {
            _gatherablesCollectedIndex = data.DroppedGatherable[_id];
        }
    }

    public void SaveData(ref GameData data)
    {
        if (_id == string.Empty)
        {
            Debug.LogWarning("No id yet made! Please generate one!");
            return;
        }

        data.DroppedGatherable[_id] = _gatherablesCollectedIndex;
    }
}
