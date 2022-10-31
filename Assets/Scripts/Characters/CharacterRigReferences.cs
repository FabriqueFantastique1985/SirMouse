using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterRigReferences : MonoBehaviour
{
    public Transform HatTransform;
    public Transform HeadTransform;
    public Transform BodyTransform;

    public Transform ArmUpperLeftTransform;
    public Transform ArmUpperRightTransform;
    public Transform ArmLowerLeftTransform;
    public Transform ArmLowerRightTransform;
    public Transform HandLeftTransform;
    public Transform HandRightTransform;

    public Transform TailTransform;

    public Transform LegUpperLeftTransform;
    public Transform LegUpperRightTransform;
    public Transform KneeLeftTransform;
    public Transform KneeRightTransform;
    public Transform LegLowerLeftTransform;
    public Transform LegLowerRightTransform;

    public Transform FootLeftTransform;
    public Transform FootRightTransform;


    public Transform FindCorrectTransform(SkinType skinType)
    {
        switch ((int)skinType)
        {
            case 0:
                return HatTransform;
            case 1:
                return HeadTransform;
            case 2:
                return BodyTransform;
            case 3:
                return ArmUpperLeftTransform;
            case 4:
                return ArmUpperRightTransform;
            case 5:
                return ArmLowerLeftTransform;
            case 6:
                return ArmLowerRightTransform;
            case 7:
                return HandLeftTransform;
            case 8:
                return HandRightTransform;
            case 9:
                return TailTransform;
            case 10:
                return LegUpperLeftTransform;
            case 11:
                return LegUpperRightTransform;
            case 12:
                return KneeLeftTransform;
            case 13:
                return KneeRightTransform;
            case 14:
                return LegLowerLeftTransform;
            case 15:
                return LegLowerRightTransform;
            case 16:
                return FootLeftTransform;
            case 17:
                return FootRightTransform;
            default:
                Debug.Log("could not find a corresponding transform for the skin in CharacterRigReferences");
                return null;
        }
    }
}
