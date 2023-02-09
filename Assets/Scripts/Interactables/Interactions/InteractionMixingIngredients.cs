using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionMixingIngredients : Interaction
{
    // KITCHEN
    /*
     * 1) move Sir Mouse to correct position
     *   ) move objects to correct positions
     *   ) move camera to correct position
     *   ) block the input of sir mouse
     * 2) enable the touchables
     * 
     * 3) show the first recipe
     *  ) after the amount of objects on the recipe has been thrown in, check if it's correct
     *  ) if correct, continue, else explode
     *  
     * 4) after all recipes, stir the pot
     * 
     * 5) after stirring, move objects to original positions
     * 6) get reward
     * 
     * */

    public RecipeController RecipeControllerScript;
    

    protected override void SpecificAction(Player player)
    {
        base.SpecificAction(player);

        // MISTER WITCH

        // 1) move Sir Mouse to correct position
        //   ) move objects to correct positions
        //   ) move camera to correct position
        //   ) block the input of sir mouse -- (happens in RecipeController)

        // 2) enable the touchables -- (happens in RecipeController)

        // 3) show the first recipe
        //  ) after the amount of objects on the recipe has been thrown in, check if it's correct
        //  ) if correct, continue, else explode
        //  ) get reward

        RecipeControllerScript.StartMiniGame();
    }
}
