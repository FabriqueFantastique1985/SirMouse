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

    [Header("Perhaps I activate a new needy wrap ?")]
    [SerializeField]
    private GameObject _interactableNeedyWrapToActivate;

    protected override void SpecificAction(Player player)
    {
        base.SpecificAction(player);

        if (SpriteParent != null)
        {
            ClosetController.Instance.StartCoroutine(ClosetController.Instance.SetObjectToFalseAfterDelay(this.gameObject, SpriteParent));
        }

        RewardController.Instance.GiveReward(_mySkinPieceElements);

        // activate new interactable if there is any
        if (_interactableNeedyWrapToActivate != null)
        {
            _interactableNeedyWrapToActivate.gameObject.SetActive(true);
        }

        // disable this one
        ClosetController.Instance.StartCoroutine(ClosetController.Instance.SetObjectToFalseAfterDelay(this.gameObject, SpriteParent));
    }
}
