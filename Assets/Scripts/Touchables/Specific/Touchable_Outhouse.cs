using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Touchable_Outhouse : Touchable
{
    [SerializeField]
    private SpriteRenderer _bottleSprite;

    [SerializeField]
    private List<Sprite> _possibleBottleSprites = new List<Sprite>();

    private bool _gaveReward;  // save data here
    private int _indexAnimation = 0;

    [SerializeField]
    private List<SkinPieceElement> _skinRewards;


    protected override void Start()
    {
        base.Start();

        _indexAnimation = 0;
    }

    // animation event
    public void RandomizeBottleSprite()
    {
        var random = Random.Range(0, _possibleBottleSprites.Count - 1);

        _bottleSprite.sprite = _possibleBottleSprites[random];
    }



    protected override void TriggerAnimation()
    {
        if (Animator != null)
        {
            Animator.SetTrigger("Activate");
            _indexAnimation += 1;

            if (_indexAnimation > 2)
            {
                _indexAnimation = 0;

                if (_gaveReward == false)
                {
                    _gaveReward = true;

                    RewardController.Instance.StartCoroutine(GiveReward());
                }  
            }
        }
    }



    private IEnumerator GiveReward()
    {
        yield return new WaitForSeconds(0.6f); // short delay 

        RewardController.Instance.GiveReward(_skinRewards);
    }
}
