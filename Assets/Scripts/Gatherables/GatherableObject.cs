using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatherableObject : MonoBehaviour, IDataPersistence
{
    [SerializeField]
    private string _id;

    private bool _isGathered = false;
    
    private void Awake()
    {
        if (_id == string.Empty)
        {
            Debug.LogError("No id yet made! Please generate one!");            
        }
    }
    
    [ContextMenu("Generate guid for id")]
    private void GenerateGuid()
    {
        _id = System.Guid.NewGuid().ToString();
    }

    public virtual void PickedUpGatherable()
    {
        _isGathered = true;
    }

    public void LoadData(GameData data)
    {
        if (data.GatherableData.ContainsKey(_id))
        {
            _isGathered = data.GatherableData[_id];
        }
    }

    public void SaveData(ref GameData data)
    {
        data.GatherableData[_id] = _isGathered;
    }
}
