using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

[RequireComponent(typeof(BoxCollider))]
public class FFButton : MonoBehaviour, IClickable
{
    [FormerlySerializedAs("_actionOnClick")] public UnityEvent ActionOnClick;

    public bool OnReleaseOnExecute { get; set; }

    private void Awake()
    {
       var rectTransform = GetComponent<RectTransform>();
       var collider = GetComponent<BoxCollider>();
       
       if (rectTransform != null)
       {
           collider.size = new Vector3(rectTransform.rect.width, rectTransform.rect.height, 0.01f);;
       }
    }

    public void Click(Player player)
    {
        ActionOnClick?.Invoke();
    }
}
