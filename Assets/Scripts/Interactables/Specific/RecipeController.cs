using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeController : MonoBehaviour
{
    // this bool needs to be changed depending on save file/objective data
    public bool CompletedMainQuest;

    // assign in inspector
    public List<Recipe_Script> MyRecipes = new List<Recipe_Script>();
    public List<Sprite> PossibleSprites = new List<Sprite>();
    public List<Touchable> TouchableIngredients = new List<Touchable>();

    //public Sprite SpriteStatusSuccess; // might just assign this to the Ingredient prefab
    //public Sprite SpriteStatusFailure; // might end up obsolete 

    public Animator RecipeScrollAnimator;

    // assigned at runtime
    public List<Type_Ingredient> CurrentRequiredIngredients = new List<Type_Ingredient>();


    private int _recipesCompleted = 0;
    private int _recipesRequiredToEndMainQuest = 5;
    private Type_Difficulty _currentDifficulty;
    private Recipe_Script _currentRecipe;

    private const string _enterScroll = "Scroll_Enter_Anim";
    private const string _endScroll = "Scroll_Hide_Anim";
    private const string _openScroll = "Scroll_Open_Anim";
    private const string _closeScroll = "Scroll_Close_Anim";

    private const string _ingredientDefaultIdle = "Ingredient_Default_Idle_Anim";
    private const string _ingredientPoof = "Ingredient_Poof_Anim";


    // below values could be adjusted in inspector for reproduction purposes
    #region DifficultyIncreasingValues 

    private const int _difficultyToIncrease_0_To_1 = 1;
    private const int _difficultyToIncrease_1_To_2 = 2;
    private const int _difficultyToIncrease_2_To_3 = 3;
    private const int _difficultyToIncrease_3_To_4 = 4;
    private const int _difficultyToIncrease_4_To_5 = 5;
    private const int _difficultyToIncrease_5_To_6 = 6;
    private const int _difficultyToIncrease_6_To_7 = 7;

    #endregion



    private void Start()
    {
        // disable touchables on start
        for (int i = 0; i < TouchableIngredients.Count; i++)
        {
            TouchableIngredients[i].Collider.enabled = false;
        }
    }



    // called from interaction
    public void StartMinigame()
    {
        //GameManager.Instance.BlockInput = true;

        // enables touchables
        for (int i = 0; i < TouchableIngredients.Count; i++)
        {
            TouchableIngredients[i].Collider.enabled = true;
        }

        // show first scroll
        FirstScroll();
    }
    // called when failing/completing the minigame
    public void EndMinigame()
    {
        RecipeScrollAnimator.Play(_endScroll);
        _recipesCompleted = 0;

        // disable touchables 
        for (int i = 0; i < TouchableIngredients.Count; i++)
        {
            TouchableIngredients[i].Collider.enabled = false;
        }

        //GameManager.Instance.BlockInput = false;
    }


    public void FirstScroll()
    {
        StartCoroutine(SequenceScrollRefresh(true));
    }
    public void RefreshScroll()
    {
        StartCoroutine(SequenceScrollRefresh(false));
    }


    private IEnumerator SequenceScrollRefresh(bool firstScroll)
    {
        if (firstScroll == true)
        {
            RecipeScrollAnimator.Play(_enterScroll);
            _currentDifficulty = Type_Difficulty.Easiest;

            yield return new WaitForSeconds(1.2f);
        }
        else
        {
            // animate the ingredients popping out (poofing)
            for (int i = 0; i < _currentRecipe.MyIngredients.Count; i++)
            {
                _currentRecipe.MyIngredients[i].MyAnimationComponent.Play(_ingredientPoof);
            }
            // reset the needed values (color of hidden ingredients, object states ...
            for (int i = 0; i < _currentRecipe.MyIngredients.Count; i++)
            {
                _currentRecipe.MyIngredients[i].SpriteRendererIngredient.color = new Color(255, 255, 255);
                _currentRecipe.MyIngredients[i].Hidden = false;
                _currentRecipe.MyIngredients[i].ObjectIngredient.SetActive(false);
                _currentRecipe.MyIngredients[i].ObjectStatus.SetActive(false);
            }


            yield return new WaitForSeconds(0.3f);
            // animate the scroll up
            RecipeScrollAnimator.Play(_closeScroll);

            yield return new WaitForSeconds(0.5f);
        }

        // ----------
        RandomizeRecipe(); // assigns a new _currentRecipe and its values
        // ----------

        // animate the scroll down
        RecipeScrollAnimator.Play(_openScroll);
        yield return new WaitForSeconds(0.3f);


        // pop in all ingredient objects 1 by 1
        for (int i = 0; i < _currentRecipe.MyIngredients.Count; i++)
        {
            _currentRecipe.MyIngredients[i].MyAnimationComponent.Play(_ingredientDefaultIdle);
            _currentRecipe.MyIngredients[i].ObjectIngredient.SetActive(true);
            yield return new WaitForSeconds(0.2f);
        }
    }



    /// <summary>
    /// Called on the OnTriggerEnter of "Trigger_To_Check_Ingredients"
    /// </summary>
    public void UpdateRecipeStatus(Type_Ingredient typeEntered)
    {
        // figure out if we entered a correct ingredient...
        bool correctIngredient = false;
        for (int i = 0; i < CurrentRequiredIngredients.Count; i++)
        {
            if (CurrentRequiredIngredients[i] == typeEntered)
            {
                correctIngredient = true;
                break;
            }
        }

        // if we did enter a correct ingredient ...
        if (correctIngredient == true)
        {
            // enable the status on the objectIngredient
            Ingredient ingredientToUpdate = null;
            for (int i = 0; i < _currentRecipe.MyIngredients.Count; i++)
            {
                if (_currentRecipe.MyIngredients[i].TypeOfIngredient == typeEntered && _currentRecipe.MyIngredients[i].HasBeenGiven == false) 
                {
                    ingredientToUpdate = _currentRecipe.MyIngredients[i];

                    ingredientToUpdate.HasBeenGiven = true;
                    ingredientToUpdate.ObjectStatus.gameObject.SetActive(true);

                    break;
                }
            }

            // remove the entered ingredient from the requirements
            CurrentRequiredIngredients.Remove(typeEntered);

            // check if this was the last required ingredient
            CheckRecipeCompletion();

        }
        else
        {
            // what do we do on a wrong ingredient ? (lose, refresh, life system ???)
            PunishThePlayer();         
        }
    }



    private void CheckRecipeCompletion()
    {
        if (CurrentRequiredIngredients.Count < 1)
        {
            // update _recipesCompleted
            _recipesCompleted += 1;

            // check if this ends the minigame, else refresh the recipe to a newer (more difficult) one
            if (_recipesCompleted >= _recipesRequiredToEndMainQuest)
            {
                // end the minigame
                Debug.Log("YOU FINISHED THE GAME");
            }
            else // else, refresh the recipe
            {
                // decide the difficulty
                switch (_recipesCompleted)
                {
                    case _difficultyToIncrease_0_To_1:
                        _currentDifficulty = Type_Difficulty.Easy;                      
                        break;
                    case _difficultyToIncrease_1_To_2:
                        _currentDifficulty = Type_Difficulty.SubNormal;
                        break;
                    case _difficultyToIncrease_2_To_3:
                        _currentDifficulty = Type_Difficulty.Normal;
                        break;
                    case _difficultyToIncrease_3_To_4:
                        _currentDifficulty = Type_Difficulty.Hard;
                        break;
                    case _difficultyToIncrease_4_To_5:
                        _currentDifficulty = Type_Difficulty.VeryHard;
                        break;
                    case _difficultyToIncrease_5_To_6:
                        _currentDifficulty = Type_Difficulty.Expert;
                        break;
                    case _difficultyToIncrease_6_To_7:
                        _currentDifficulty = Type_Difficulty.God;
                        break;
                }

                RefreshScroll();
            }          
        }
    }
    private void RandomizeRecipe()
    {
        // use the assigned difficultyLevel...
        // find the recipes that fall within this difficulty...
        // after finding the possible recipes, pick one at random from them, assign as current recipe
        List<Recipe_Script> possibleRecipes = new List<Recipe_Script>();
        for (int i = 0; i < MyRecipes.Count; i++)
        {
            // if I find recipes with difficulties I comply with...
            if (((int)_currentDifficulty) >= (int)(MyRecipes[i].MyLowestDifficulty) && ((int)_currentDifficulty) <= ((int)MyRecipes[i].MyHightestDifficulty))
            {
                // add to list
                possibleRecipes.Add(MyRecipes[i]);
            }
        }
        // pick a random one from the list
        int randomRecipe = UnityEngine.Random.Range(0, possibleRecipes.Count);
        // assign it
        _currentRecipe = possibleRecipes[randomRecipe];

        // after this, randomize the ingredients types
        // -- assign its Type as well as sprite !!!
        // -- randomize it so that there can never be more than 2 of the same
        // -- make some ingredients hidden 
        for (int i = 0; i < _currentRecipe.MyIngredients.Count; i++)
        {
            SetRandomIngredient(i);
        }

        // ---- if difficulty is subNormal (or below) && ingredient count > 1  (max 1)
        // ---- if difficulty is above subNormal, below veryHard && ingredient count > 1  (max 1)
        // ---- if difficulty is veryHard (or below) && ingredient count > 2  (max 2)
        // ---- if difficulty above veryHard && ingredient count > 3  (max 2)
        // ---- if difficulty is God && ingredient count > 3  (max 3)
        // make some hidden if possible
        PossiblyHideSomeIngredients();
    }
    private void PossiblyHideSomeIngredients()
    {
        if (((int)_currentDifficulty) <= ((int)Type_Difficulty.SubNormal) && _currentRecipe.MyIngredients.Count > 1)
        {
            // hide 1
            int randomValue = UnityEngine.Random.Range(0, _currentRecipe.MyIngredients.Count);
            _currentRecipe.MyIngredients[randomValue].SpriteRendererIngredient.color = new Color(0,0,0);
            _currentRecipe.MyIngredients[randomValue].Hidden = true;
        }
        else if (((int)_currentDifficulty) > ((int)Type_Difficulty.SubNormal) && ((int)_currentDifficulty) < ((int)Type_Difficulty.VeryHard) && _currentRecipe.MyIngredients.Count > 1)
        {
            // hide 1
            int randomValue = UnityEngine.Random.Range(0, _currentRecipe.MyIngredients.Count);
            _currentRecipe.MyIngredients[randomValue].SpriteRendererIngredient.color = new Color(0, 0, 0);
            _currentRecipe.MyIngredients[randomValue].Hidden = true;
        }
        else if (((int)_currentDifficulty) <= ((int)Type_Difficulty.VeryHard) && _currentRecipe.MyIngredients.Count > 2)
        {
            // hide 2
            int randomValue = UnityEngine.Random.Range(0, _currentRecipe.MyIngredients.Count);
            _currentRecipe.MyIngredients[randomValue].SpriteRendererIngredient.color = new Color(0, 0, 0);
            _currentRecipe.MyIngredients[randomValue].Hidden = true;

            // if my randomValue+1 is bigger or equal to ..Count, ---> use lesser than 
            if ((randomValue + 1) >= _currentRecipe.MyIngredients.Count)
            {
                _currentRecipe.MyIngredients[randomValue - 1].SpriteRendererIngredient.color = new Color(0, 0, 0);
                _currentRecipe.MyIngredients[randomValue - 1].Hidden = true;
            }
            else
            {
                _currentRecipe.MyIngredients[randomValue + 1].SpriteRendererIngredient.color = new Color(0, 0, 0);
                _currentRecipe.MyIngredients[randomValue + 1].Hidden = true;
            }
        }
        else if (((int)_currentDifficulty) > ((int)Type_Difficulty.VeryHard) && ((int)_currentDifficulty) < ((int)Type_Difficulty.God) && _currentRecipe.MyIngredients.Count > 3)
        {
            // hide 3
            int randomValue = UnityEngine.Random.Range(0, _currentRecipe.MyIngredients.Count);
            _currentRecipe.MyIngredients[randomValue].SpriteRendererIngredient.color = new Color(0, 0, 0);
            _currentRecipe.MyIngredients[randomValue].Hidden = true;

            // if my randomValue+2 is bigger or equal to ..Count, ---> use lesser than 
            if ((randomValue + 2) >= _currentRecipe.MyIngredients.Count)
            {
                _currentRecipe.MyIngredients[randomValue - 1].SpriteRendererIngredient.color = new Color(0, 0, 0);
                _currentRecipe.MyIngredients[randomValue - 2].SpriteRendererIngredient.color = new Color(0, 0, 0);
                _currentRecipe.MyIngredients[randomValue - 1].Hidden = true;
                _currentRecipe.MyIngredients[randomValue - 2].Hidden = true;
            }
            else
            {
                _currentRecipe.MyIngredients[randomValue + 1].SpriteRendererIngredient.color = new Color(0, 0, 0);
                _currentRecipe.MyIngredients[randomValue + 2].SpriteRendererIngredient.color = new Color(0, 0, 0);
                _currentRecipe.MyIngredients[randomValue + 1].Hidden = true;
                _currentRecipe.MyIngredients[randomValue + 2].Hidden = true;
            }
        }
        else if (((int)_currentDifficulty) >= ((int)Type_Difficulty.God) && _currentRecipe.MyIngredients.Count > 3)
        {
            // hide 3
            int randomValue = UnityEngine.Random.Range(0, _currentRecipe.MyIngredients.Count);
            _currentRecipe.MyIngredients[randomValue].SpriteRendererIngredient.color = new Color(0, 0, 0);
            _currentRecipe.MyIngredients[randomValue].Hidden = true;

            // if my randomValue+2 is bigger or equal to ..Count, ---> use lesser than 
            if ((randomValue + 2) >= _currentRecipe.MyIngredients.Count)
            {
                _currentRecipe.MyIngredients[randomValue - 1].SpriteRendererIngredient.color = new Color(0, 0, 0);
                _currentRecipe.MyIngredients[randomValue - 2].SpriteRendererIngredient.color = new Color(0, 0, 0);
                _currentRecipe.MyIngredients[randomValue - 1].Hidden = true;
                _currentRecipe.MyIngredients[randomValue - 2].Hidden = true;
            }
            else
            {
                _currentRecipe.MyIngredients[randomValue + 1].SpriteRendererIngredient.color = new Color(0, 0, 0);
                _currentRecipe.MyIngredients[randomValue + 2].SpriteRendererIngredient.color = new Color(0, 0, 0);
                _currentRecipe.MyIngredients[randomValue + 1].Hidden = true;
                _currentRecipe.MyIngredients[randomValue + 2].Hidden = true;
            }
        }
    }
    private void SetRandomIngredient(int i)
    {
        // get random
        var randomInt = UnityEngine.Random.Range(1, PossibleSprites.Count + 1);

        // check if 2 ingredients alrdy have this (if so re-calculate the random)
        // TO DO

        // assign type according to random
        switch (randomInt)
        {
            case 1:
                _currentRecipe.MyIngredients[i].TypeOfIngredient = Type_Ingredient.Ingredient_1;
                _currentRecipe.MyIngredients[i].SpriteRendererIngredient.sprite = PossibleSprites[0];

                CurrentRequiredIngredients.Add(Type_Ingredient.Ingredient_1);
                break;
            case 2:
                _currentRecipe.MyIngredients[i].TypeOfIngredient = Type_Ingredient.Ingredient_2;
                _currentRecipe.MyIngredients[i].SpriteRendererIngredient.sprite = PossibleSprites[1];

                CurrentRequiredIngredients.Add(Type_Ingredient.Ingredient_2);
                break;
            case 3:
                _currentRecipe.MyIngredients[i].TypeOfIngredient = Type_Ingredient.Ingredient_3;
                _currentRecipe.MyIngredients[i].SpriteRendererIngredient.sprite = PossibleSprites[2];

                CurrentRequiredIngredients.Add(Type_Ingredient.Ingredient_3);
                break;
            case 4:
                _currentRecipe.MyIngredients[i].TypeOfIngredient = Type_Ingredient.Ingredient_4;
                _currentRecipe.MyIngredients[i].SpriteRendererIngredient.sprite = PossibleSprites[3];

                CurrentRequiredIngredients.Add(Type_Ingredient.Ingredient_4);
                break;
            case 5:
                _currentRecipe.MyIngredients[i].TypeOfIngredient = Type_Ingredient.Ingredient_5;
                _currentRecipe.MyIngredients[i].SpriteRendererIngredient.sprite = PossibleSprites[4];

                CurrentRequiredIngredients.Add(Type_Ingredient.Ingredient_5);
                break;
        }
    }
    private void PunishThePlayer()
    {
        Debug.Log("WRONG INGREDIENT FOOOOOOL");
    }

}
