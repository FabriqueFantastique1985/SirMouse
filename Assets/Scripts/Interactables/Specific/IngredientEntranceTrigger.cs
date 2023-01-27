using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientEntranceTrigger : MonoBehaviour
{
    [SerializeField]
    private RecipeController _recipeController;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Touch_Physics_Object_Ingredient touchablePhysicsIngredient))
        {
            if (touchablePhysicsIngredient.LetGo == true)
            {
                _recipeController.UpdateRecipeStatus(touchablePhysicsIngredient.TypeOfIngredient);

                // destroy the ingredient AND first remove it from the list of its spawned objects
                touchablePhysicsIngredient.SourceIComeFrom.RemoveObjectFromList(touchablePhysicsIngredient.gameObject);
                Destroy(touchablePhysicsIngredient.gameObject);

                // play particle sploosh ??

            }

        }
    }
}
