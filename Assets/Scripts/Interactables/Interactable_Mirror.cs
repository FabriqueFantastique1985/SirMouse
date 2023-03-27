using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable_Mirror : Interactable
{
    [SerializeField] private List<SpriteRenderer> _mirrorImages = new List<SpriteRenderer>();
    [SerializeField] private float _steps = 0.01f;

    private void Awake()
    {
        foreach (var image in _mirrorImages)
        {
            image.color = new Color(1f, 1f, 1f, 0f);
        }
    }

    protected override void OnTriggerEnter(Collider other)
    {
        foreach(var skinPiece in SkinsMouseController.Instance.EquipedSkinPieces) 
        { 
            if (skinPiece.Data.MyBodyType == Type_Body.Hat && skinPiece.Data.MySkinType == Type_Skin.Ostrich)
            {
                base.OnTriggerEnter(other);
                break;
            }
        }

        StartCoroutine(ShowMirror());
    }

    protected override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);
        StartCoroutine(HideMirror());
    }

    private IEnumerator ShowMirror()
    {
        while (_mirrorImages[0].color.a < 1f) 
        {
            foreach (var image in _mirrorImages)
            {
                image.color += new Color(0f, 0f, 0f, _steps);
            }
            yield return null;
        }
    }
    private IEnumerator HideMirror()
    {
        while (_mirrorImages[0].color.a > 0f) 
        {
            foreach (var image in _mirrorImages)
            {
                image.color -= new Color(0f, 0f, 0f, _steps);
            }
            yield return null;
        }
    }
}
