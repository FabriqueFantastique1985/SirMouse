using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionCollectible : Interaction
{
    [SerializeField]
    CollectibleType _type;
    [SerializeField]
    CollectibleSpecificType _specificSlotType;

    [SerializeField]
    private SpriteRenderer _spriteRendererCollectible;
    private Sprite _spriteToDuplicateInUI;

    [SerializeField]
    private Animation _animationComponentSpriteParent;


    private void Start()
    {
        _spriteToDuplicateInUI = _spriteRendererCollectible.sprite;
    }

    protected override void SpecificAction(Player player)
    {
        base.SpecificAction(player);

        // play the animation, destroy the interactable
        _animationComponentSpriteParent.Play();
        Destroy(this.gameObject, 2f);

        CollectibleController.Instance.FoundCollectible(_type, _specificSlotType, _spriteToDuplicateInUI);
    }

}
