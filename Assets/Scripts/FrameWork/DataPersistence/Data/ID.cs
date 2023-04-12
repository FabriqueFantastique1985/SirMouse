using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ID : MonoBehaviour
{ 
    [SerializeField]
    private string _idName;
    
    public string IDName => _idName;
    
    public void GenerateGuid()
    {
        _idName = System.Guid.NewGuid().ToString();
    }
    
    public static implicit operator string(ID id)
    {
        // TODO throw error when ID is null
        if (id == null)
            return "";

        return id._idName;
    }
}
