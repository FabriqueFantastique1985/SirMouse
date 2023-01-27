using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recipe_Script : MonoBehaviour
{
    public List<Ingredient> MyIngredients = new List<Ingredient>();

    public Type_Difficulty MyDifficultyLevel;

    [Header("Difficulty Limits")]
    public Type_Difficulty MyLowestDifficulty;
    public Type_Difficulty MyHightestDifficulty;
}
