using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterGeoReferences : MonoBehaviour
{
    //public Material SirMouseBody;
    //public Material SirMouseHands;

    //public Texture2D HeadLight; // 0
    //public Texture2D LegsLight; // 1
    //public Texture2D ArmsLight; // 2

    //public Texture2D DefaultTex; 

    //[Header("Textures Hands")]
    //public Texture2D HandsLight;
    //public Texture2D HandsDark;

    [Header("Mesh References")]
    public SkinnedMeshRenderer Head;
    public SkinnedMeshRenderer EarL;
    public SkinnedMeshRenderer EarR;
    public SkinnedMeshRenderer ShoulderL;
    public SkinnedMeshRenderer ShoulderR;
    public SkinnedMeshRenderer ArmUpL;
    public SkinnedMeshRenderer ArmUpR;
    public SkinnedMeshRenderer ElbowL;
    public SkinnedMeshRenderer ElbowR;
    public SkinnedMeshRenderer HandL;
    public SkinnedMeshRenderer HandR;
    public SkinnedMeshRenderer Chest;
    public SkinnedMeshRenderer Skirt;
    public SkinnedMeshRenderer Tail;
    public SkinnedMeshRenderer LegUpL;
    public SkinnedMeshRenderer LegUpR;
    public SkinnedMeshRenderer KneeL;
    public SkinnedMeshRenderer KneeR;
    public SkinnedMeshRenderer LegLowL;
    public SkinnedMeshRenderer LegLowR;
    public SkinnedMeshRenderer FootL;
    public SkinnedMeshRenderer FootR;

    private List<SkinnedMeshRenderer> _skinMeshesMouseBody = new List<SkinnedMeshRenderer>(); // list for easy iterating

    [Header("More Mesh References")]
    public GameObject SwordAndShieldMeshParent;
    public SkinnedMeshRenderer Sword;
    public SkinnedMeshRenderer Shield;
    public MeshRenderer SwordBack;
    public MeshRenderer ShieldBack;
    public SpriteRenderer RedHeadOverlay;

    [Header("Material References")]

    public Material MaterialNormal;
    public Material MaterialExploded;

    private void Start()
    {
        _skinMeshesMouseBody.Add(Head);
        _skinMeshesMouseBody.Add(EarL);
        _skinMeshesMouseBody.Add(EarR);
        _skinMeshesMouseBody.Add(ShoulderL);
        _skinMeshesMouseBody.Add(ShoulderR);
        _skinMeshesMouseBody.Add(ArmUpL);
        _skinMeshesMouseBody.Add(ArmUpR);
        _skinMeshesMouseBody.Add(ElbowL);
        _skinMeshesMouseBody.Add(ElbowR);
        _skinMeshesMouseBody.Add(HandL);
        _skinMeshesMouseBody.Add(HandR);
        _skinMeshesMouseBody.Add(Chest);
        _skinMeshesMouseBody.Add(Skirt);
        _skinMeshesMouseBody.Add(Tail);
        _skinMeshesMouseBody.Add(LegUpL);
        _skinMeshesMouseBody.Add(LegUpR);
        _skinMeshesMouseBody.Add(KneeL);
        _skinMeshesMouseBody.Add(KneeR);
        _skinMeshesMouseBody.Add(LegLowL);
        _skinMeshesMouseBody.Add(LegLowR);
        _skinMeshesMouseBody.Add(FootL);
        _skinMeshesMouseBody.Add(FootR);
    }

    public void SwapToExplodedMaterial()
    {
        for (int i = 0; i < _skinMeshesMouseBody.Count; i++)
        {
            _skinMeshesMouseBody[i].material = MaterialExploded;
        }
    }
    public void SwapToNormalMaterial()
    {
        for (int i = 0; i < _skinMeshesMouseBody.Count; i++)
        {
            _skinMeshesMouseBody[i].material = MaterialNormal;
        }
    }

    //public void ChangeTextureSirMouse(int index)
    //{
    //    switch (index)
    //    {
    //        case 0:
    //            SirMouseBody.mainTexture = HeadLight;
    //            SirMouseHands.mainTexture = HandsDark;
    //            break;
    //        case 1:
    //            SirMouseBody.mainTexture = LegsLight;
    //            SirMouseHands.mainTexture = HandsDark;
    //            break;
    //        case 2:
    //            SirMouseBody.mainTexture = ArmsLight;
    //            SirMouseHands.mainTexture = HandsLight;
    //            break;
    //        case 3:
    //            SirMouseBody.mainTexture = HeadLight;
    //            SirMouseHands.mainTexture = HandsDark;
    //            break;
    //    }
    //}

    //public void DefaultTextureSirMouse()
    //{
    //    SirMouseBody.mainTexture = DefaultTex;
    //    SirMouseHands.mainTexture = HandsLight;
    //}

}
