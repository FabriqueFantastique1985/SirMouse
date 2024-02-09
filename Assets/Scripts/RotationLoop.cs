using DG.Tweening;
using Newtonsoft.Json.Converters;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class RotationLoop : MonoBehaviour
{
    [SerializeField]
    private Vector3 _rotationSpeed;

    private void FixedUpdate()
    {
        transform.Rotate(_rotationSpeed.x * Time.deltaTime, _rotationSpeed.y * Time.deltaTime, _rotationSpeed.z * Time.deltaTime);
    }
}