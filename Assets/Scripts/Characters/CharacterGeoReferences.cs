using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterGeoReferences : MonoBehaviour
{
    public Material SirMouseBody;
    public Material SirMouseHands;

    public Texture2D HeadLight; // 0
    public Texture2D LegsLight; // 1
    public Texture2D ArmsLight; // 2

    public Texture2D DefaultTex; 

    [Header("Hands")]
    public Texture2D HandsLight;
    public Texture2D HandsDark;

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


    public void ChangeTextureSirMouse(int index)
    {
        switch (index)
        {
            case 0:
                SirMouseBody.mainTexture = HeadLight;
                SirMouseHands.mainTexture = HandsDark;
                break;
            case 1:
                SirMouseBody.mainTexture = LegsLight;
                SirMouseHands.mainTexture = HandsDark;
                break;
            case 2:
                SirMouseBody.mainTexture = ArmsLight;
                SirMouseHands.mainTexture = HandsLight;
                break;
            case 3:
                SirMouseBody.mainTexture = HeadLight;
                SirMouseHands.mainTexture = HandsDark;
                break;
        }
    }

    public void DefaultTextureSirMouse()
    {
        SirMouseBody.mainTexture = DefaultTex;
        SirMouseHands.mainTexture = HandsLight;
    }

}
