using System.Collections;
using System.Collections.Generic;
using UnityCore.Audio;
using UnityEngine;

public class IngredientEntranceTrigger : MonoBehaviour
{
    [SerializeField]
    private RecipeController _recipeController;

    public List<Type_Ingredient> IngredientsInCauldron;

    private Animator _cauldronAnimator;
    private List<ParticleSystem> _currentActiveParticles = new List<ParticleSystem>();

    public AudioElement Splash;

    public Touch_Physics_Object_Ingredient CurrentlyDroppedIngredient;

    private void Start()
    {
        _cauldronAnimator = _recipeController.CauldronAnimator;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Touch_Physics_Object_Ingredient touchablePhysicsIngredient))
        {
            if (touchablePhysicsIngredient.LetGo == true)
            {
                AbsorbIngredient(touchablePhysicsIngredient);
            }
        }
    }


    // called when it falls into the cauldron OR raycast onto the kettle
    public void AbsorbIngredient(Touch_Physics_Object_Ingredient touchablePhysicsIngredient)
    {
        if (_recipeController.MinigameActive == true)
        {
            _recipeController.StartCoroutine(_recipeController.UpdateRecipeStatus(touchablePhysicsIngredient.TypeOfIngredient));
        }
        else
        {
            StartCoroutine(AddNonMinigameIngredientToCauldron(touchablePhysicsIngredient));
        }

        // destroy the ingredient AND first remove it from the list of its spawned objects
        touchablePhysicsIngredient.SourceIComeFrom.RemoveObjectFromList(touchablePhysicsIngredient.gameObject);
        Destroy(touchablePhysicsIngredient.gameObject);

        // play particle splash
        _recipeController.ParticleSplash.Play(true); // particle needs to be burst here!

        // play sound
        if (Splash.Clip != null)
        {
            AudioController.Instance.PlayAudio(Splash);
        }
    }




    private IEnumerator AddNonMinigameIngredientToCauldron(Touch_Physics_Object_Ingredient ingredient)
    {
        // 1) check what was thrown in
        IngredientsInCauldron.Add(ingredient.TypeOfIngredient);
        // 2) activate linked particle
        switch (ingredient.TypeOfIngredient)
        {
            case Type_Ingredient.Ingredient_1:
                _recipeController.ParticlesFromIngredientsForCauldron[0].Play();
                _currentActiveParticles.Add(_recipeController.ParticlesFromIngredientsForCauldron[0]);
                break;
            case Type_Ingredient.Ingredient_2:
                _recipeController.ParticlesFromIngredientsForCauldron[1].Play();
                _currentActiveParticles.Add(_recipeController.ParticlesFromIngredientsForCauldron[1]);
                break;
            case Type_Ingredient.Ingredient_3:
                _recipeController.ParticlesFromIngredientsForCauldron[2].Play();
                _currentActiveParticles.Add(_recipeController.ParticlesFromIngredientsForCauldron[2]);
                break;
            case Type_Ingredient.Ingredient_4:
                _recipeController.ParticlesFromIngredientsForCauldron[3].Play();
                _currentActiveParticles.Add(_recipeController.ParticlesFromIngredientsForCauldron[3]);
                break;
            case Type_Ingredient.Ingredient_5:
                _recipeController.ParticlesFromIngredientsForCauldron[4].Play();
                _currentActiveParticles.Add(_recipeController.ParticlesFromIngredientsForCauldron[4]);
                break;
            case Type_Ingredient.Ingredient_6:
                _recipeController.ParticlesFromIngredientsForCauldron[5].Play();
                _currentActiveParticles.Add(_recipeController.ParticlesFromIngredientsForCauldron[5]);
                break;
            case Type_Ingredient.Ingredient_7:
                _recipeController.ParticlesFromIngredientsForCauldron[6].Play();
                _currentActiveParticles.Add(_recipeController.ParticlesFromIngredientsForCauldron[6]);
                break;
            case Type_Ingredient.Ingredient_8:
                _recipeController.ParticlesFromIngredientsForCauldron[7].Play();
                _currentActiveParticles.Add(_recipeController.ParticlesFromIngredientsForCauldron[7]);
                break;
            case Type_Ingredient.Ingredient_9:
                _recipeController.ParticlesFromIngredientsForCauldron[8].Play();
                _currentActiveParticles.Add(_recipeController.ParticlesFromIngredientsForCauldron[8]);
                break;
        }
        // 3) play correct animation cauldron (setTrigger)
        switch (IngredientsInCauldron.Count)
        {
            case 1:
                _cauldronAnimator.SetTrigger(_recipeController.TriggerCauldronShakeWeak);
                break;
            case 2:
                _cauldronAnimator.SetTrigger(_recipeController.TriggerCauldronShakeNormal);
                break;
            case 3:
                _cauldronAnimator.SetTrigger(_recipeController.TriggerCauldronShakeStrong);
                break;
        }

        yield return new WaitForSeconds(1f);

        // 3.9) if this was the 3rd ingredient... 
        if (IngredientsInCauldron.Count >= 3)
        {
            // 4) ---> setTrigger(Cauldron_End)
            _cauldronAnimator.SetTrigger(_recipeController.TriggerCauldronEnd);

            // 5) --> stop older particles... 
            for (int i = 0; i < _currentActiveParticles.Count; i++)
            {
                _currentActiveParticles[i].Stop();
            }
            _recipeController.ParticleIdle.Stop();

            // play special particle depending on the ingredient mix
            _recipeController.ParticlesFinishers[0].Play();

            // 6) --> reset values
            IngredientsInCauldron.Clear();

            // 7) re-enable idle
            yield return new WaitForSeconds(2f);
            _recipeController.ParticleIdle.Play();
        }
    }

    public void ClearIngredientsAndParticlesAndAnimation()
    {
        for (int i = 0; i < _currentActiveParticles.Count; i++)
        {
            _currentActiveParticles[i].Stop();
        }

        _currentActiveParticles.Clear();
        IngredientsInCauldron.Clear();

        _cauldronAnimator.Play("None");
    }
}
