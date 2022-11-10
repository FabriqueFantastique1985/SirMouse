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


    private void Start()
    {
        SirMouseBody.mainTexture = DefaultTex;
        SirMouseHands.mainTexture = HandsLight;
    }


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
        }
    }

    public void DefaultTextureSirMouse()
    {
        SirMouseBody.mainTexture = DefaultTex;
        SirMouseHands.mainTexture = HandsLight;
    }

}
