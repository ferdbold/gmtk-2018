using System;
using UnityEngine;

public class ObjectiveManager : BaseManager<ObjectiveManager> {

    #region ATTRIBUTES

    public int _Score;

    public float _RecipeTickInterval;
    public float _RecipeTickRemaining;

    [SerializeField]
    public Recipes.SRecipe _CurrentRecipe;

    public Recipes _RecipeCollection;

    #endregion // ATTRIBUTES

    public static event Action<Recipes.SRecipe> OnRecipeChanged;

    public override void OnStartGame()
    {
        _RecipeTickRemaining = _RecipeTickInterval;
        GenerateNewRecipe();
    }

    public override void OnUpdateManager(float deltaTime)
    {
        _RecipeTickRemaining -= deltaTime;

        if (_RecipeTickRemaining < 0)
        {
            GenerateNewRecipe();
            _RecipeTickRemaining = _RecipeTickInterval;
        }
    }

    private void GenerateNewRecipe()
    {
        int recipeCount = _RecipeCollection._Recipes.Count;
        if (recipeCount > 0)
        {
            string previousRecipe = _CurrentRecipe._Name;
            Recipes.SRecipe newRecipe = _CurrentRecipe;

            while (newRecipe._Name == previousRecipe)
            {
                newRecipe = _RecipeCollection._Recipes[UnityEngine.Random.Range(0, recipeCount)];
            }

            _CurrentRecipe = newRecipe;
            OnRecipeChanged(_CurrentRecipe);
        }
        else
        {
            Debug.LogWarning("No recipes in collection calisse");
        }
    }
}
