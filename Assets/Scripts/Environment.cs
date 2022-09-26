using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Environment : MonoBehaviour
{

    [SerializeField]
    private float _environmentAngle = 30.0f;
    // Start is called before the first frame update

    private void OnValidate()
    {
     //  for (int i = 0; i < transform.childCount; i++)
     //  {
     //      var child = transform.GetChild(i);
     //      if (child == transform) continue;
     //      var localRot = child.localRotation.eulerAngles;
     //      child.transform.localRotation = Quaternion.Euler(_environmentAngle, localRot.y, localRot.z);
     //  }
    }
}
