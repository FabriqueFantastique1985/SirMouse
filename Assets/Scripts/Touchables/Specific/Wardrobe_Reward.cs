using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Wardrobe_Reward : MonoBehaviour
{
    [Header("Reward")]
    [SerializeField] private List<SkinPieceElement> _skinsToReward;

    private Animator _animator;

    private bool _hasCollected = false;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        Assert.IsNotNull( _animator );
    }

    private void Update()
    {
        if ( _hasCollected ) 
        {
            enabled = false;
            return;
        }

        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Touchable_WardrobeEmpty"))
        {
            if (_skinsToReward.Count > 0)
            {
                RewardController.Instance.GiveReward(_skinsToReward);
                _hasCollected = true;
                enabled = false;
            }
        }
    }
}
