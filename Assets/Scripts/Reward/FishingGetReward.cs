using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingGetReward : MonoBehaviour
{
    [SerializeField] private FishingResultBehaviour _fishResult;
    [SerializeField] private BackAndForth_Continous _fishHookEvent;
    [SerializeField] private SkinPieceElement _reward;

    private BackAndForth_Event _backAndForthEvent;

    private void Start()
    {
        _fishHookEvent.OnReachedStart += OnGiveReward;
        _backAndForthEvent = gameObject.GetComponent<BackAndForth_Event>();
    }

    private void OnDestroy()
    {
        _fishHookEvent.OnReachedStart -= OnGiveReward;
    }

    private void OnGiveReward()
    {
        RewardController.Instance.GiveReward(new List<SkinPieceElement> { _reward });
        if (_backAndForthEvent && _fishResult)
        {
            _fishResult.RemoveFromList(_backAndForthEvent);
        }
        Destroy(gameObject);
    }
}
