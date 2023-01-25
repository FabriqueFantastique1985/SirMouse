using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Touch_Ingredient : Touch_Physics
{
    [SerializeField]
    private Type_Ingredient _typeOfIngredient;

    private Touch_Physics_Object_Ingredient _touchPhysicIngr;

    protected override void AddPhysicsScript(GameObject spawnedObj = null)
    {
        _touchPhysicIngr = spawnedObj.AddComponent<Touch_Physics_Object_Ingredient>();
        _touchPhysicIngr.TypeOfIngredient = _typeOfIngredient;

        _physicsScriptOnSpawnedObject = spawnedObj.GetComponent<Touch_Physics_Object>();      
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Kettle")
        {
            RemoveObjectFromList(this.gameObject);
        }
    }
}
