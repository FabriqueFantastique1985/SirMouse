using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable_Mirror : Interactable, IDataPersistence
{
    [SerializeField] private List<SpriteRenderer> _mirrorImages = new List<SpriteRenderer>();
    [SerializeField] Type_Skin _requiredSkinType = Type_Skin.None;
    [SerializeField] private float _steps = 0.01f;

    private InteractionGetReward _getReward;
    private bool _hasReceivedReward = false;

    private void Awake()
    {
        foreach (var image in _mirrorImages)
        {
            image.color = new Color(1f, 1f, 1f, 0f);
        }

        _getReward = GetComponent<InteractionGetReward>();
        if (_getReward)
        {
            _getReward.GetReward += OnGetReward;
        }
    }

    private void OnGetReward()
    {
        _getReward.GetReward -= OnGetReward;
        _hasReceivedReward = true;
        var shineBehavior = GetComponent<ShineBehaviour>();
        if (shineBehavior)
        {
            shineBehavior.IsShineActive = false;
        }
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (_hasReceivedReward)
        {
            return;
        }

        foreach(var skinPiece in SkinsMouseController.Instance.EquipedSkinPieces) 
        { 
            if (skinPiece.Data.MyBodyType == Type_Body.Hat && skinPiece.Data.MySkinType == _requiredSkinType)
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

    public new void LoadData(GameData data)
    {
        base.LoadData(data);
        _hasReceivedReward = data.HasRecievedMirrorReward;
        if (_hasReceivedReward)
        {
            OnGetReward();
        }
    }

    public new void SaveData(ref GameData data)
    {
        base.SaveData(ref data);
        data.HasRecievedMirrorReward = _hasReceivedReward;
    }
    
    }
}
