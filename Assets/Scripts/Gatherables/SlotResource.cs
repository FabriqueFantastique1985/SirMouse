using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

[RequireComponent(typeof(ID))]
public class SlotResource : MonoBehaviour, IDataPersistence
{
    [Serializable]
    public struct SlotResourceData
    {
        public Type_Resource ResourceType;
        public bool SlotTaken => Amount > 0;
        public int Amount;
    }

    private void OnValidate()
    {
        if (ID == null) ID = GetComponent<ID>();
    }

    public ID ID;

    [Header("References Children")]
    public GameObject Visuals;

    public GameObject ParentInstantiatedPrefab;
    public Image ImageAmount;

    [HideInInspector]
    public SlotResourceData Data;

    public void LoadData(GameData data)
    {
        // Data gets loaded via the ResourceController
    }

    public void SaveData(ref GameData data)
    {
        if (ID == null)
        {
            Debug.LogError($"ID for object {gameObject.name} is null");
            return;
        }

        data.SlotResourceDatas[ID] = Data;
    }
}