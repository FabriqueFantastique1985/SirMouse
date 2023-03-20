using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class FishingResultBehaviour : MonoBehaviour
{
    [SerializeField] private List<BackAndForth_Event> _fishingResults = new List<BackAndForth_Event>();
    private int _index = 0;

    private void Start()
    {
        Assert.IsFalse(_fishingResults.Count == 0, "No fishing results in fishing result behavior.");

        foreach (var fishingResult in _fishingResults)
        {
            fishingResult.OnMoveComplete += OnMoveComplete;
            fishingResult.gameObject.SetActive(false);
        }

        _fishingResults[_index].gameObject.SetActive(true);
    }

    private void OnMoveComplete()
    {
        _fishingResults[_index].gameObject.SetActive(false);

        ++_index;
        _index %= _fishingResults.Count;

        _fishingResults[_index].gameObject.SetActive(true);
        _fishingResults[_index].Reset();
    }

    public void RemoveFromList(BackAndForth_Event backAndForthEvent)
    {
        for (int i = 0; i < _fishingResults.Count; i++)
        {
            if (_fishingResults[i] == backAndForthEvent)
            {
                _fishingResults.Remove(backAndForthEvent);
                _index %= _fishingResults.Count;

                _fishingResults[_index].gameObject.SetActive(true);
                _fishingResults[_index].Reset();
                return;
            }
        }
    }
}
