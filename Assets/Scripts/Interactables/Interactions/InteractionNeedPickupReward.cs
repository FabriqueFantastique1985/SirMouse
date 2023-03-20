using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SkinPieceElement))]
public class InteractionNeedPickupReward : Interaction
{
    [Header("Skin Reward params")]
    [SerializeField]
    private List<SkinPieceElement> _mySkinPieceElements;

    [Header("Sprite Parent")]
    public GameObject SpriteParent;

    [Header("Objects to Activate/Disable")]
    [SerializeField]
    private GameObject ObjectToActivate;
    [SerializeField]
    private GameObject ObjectToDisable;

    [Header("Perhaps I activate a new needy wrap ?")]
    [SerializeField]
    private GameObject _interactableNeedyWrapToActivate;

    protected override void SpecificAction(Player player)
    {
        base.SpecificAction(player);

        // disable this one
        if (SpriteParent != null)
        {
            ClosetController.Instance.StartCoroutine(ClosetController.Instance.SetObjectToFalseAfterDelay(this.gameObject, SpriteParent));
        }

        // give the reward
        if (_mySkinPieceElements.Count > 0)
        {
            RewardController.Instance.GiveReward(_mySkinPieceElements);
        }

        // activate new interactable if there is one
        if (_interactableNeedyWrapToActivate != null)
        {
            _interactableNeedyWrapToActivate.gameObject.SetActive(true);
        }
    
        // activate new obj (example: disable rat without instrument, enable rat with instrument)
        if (ObjectToActivate != null)
        {
            ObjectToActivate.SetActive(true);
        }
        if (ObjectToDisable != null)
        {
            ObjectToDisable.SetActive(false);
        }
    }
}
