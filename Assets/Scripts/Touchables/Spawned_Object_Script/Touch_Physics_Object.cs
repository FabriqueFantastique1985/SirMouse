using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Touch_Physics_Object : MonoBehaviour
{
    public bool LetGo;

    void FixedUpdate()
    {
        transform.localEulerAngles = new Vector3(30, 0, transform.localEulerAngles.z);
    }
}
