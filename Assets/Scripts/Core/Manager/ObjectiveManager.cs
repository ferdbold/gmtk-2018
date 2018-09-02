using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveManager : BaseManager<ObjectiveManager> {

    #region ATTRIBUTES

    public int _Score;

    public float _RecipeTickInterval;
    public float _RecipeTickRemaining;

    public class SRecipeScore
    {
        public List<Ingredient.SComparisonScore> _IngredientScores = new List<Ingredient.SComparisonScore>();
        public float _GlobalScore;
    }

    [SerializeField]
    public Recipes.SRecipe _CurrentRecipe;

    public Recipes _RecipeCollection;

    #endregion // ATTRIBUTES

    public static event Action<Recipes.SRecipe> OnRecipeChanged;
    public static event Action<SRecipeScore> OnRecipeShipped;

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
            if (OnRecipeChanged != null) OnRecipeChanged(_CurrentRecipe);
        }
        else
        {
            Debug.LogWarning("No recipes in collection calisse");
        }
    }

    public void ShipRecipe()
    {
        Debug.Log("Shipping recipe");

        SRecipeScore recipeScore = new SRecipeScore();

        int ingredientCount = _CurrentRecipe._Ingredients.Count;
        for (int i = 0; i < ingredientCount; ++i)
        {
            Ingredient.SComparisonScore ingredientScore = new Ingredient.SComparisonScore();
            
            if (PreparationStation._LaborOfLove.Count > i)
            {
                Ingredient laborIngredient = PreparationStation._LaborOfLove[i];
                ingredientScore = _CurrentRecipe._Ingredients[i].Compare(laborIngredient);
            }

            recipeScore._IngredientScores.Add(ingredientScore);
            recipeScore._GlobalScore += ingredientScore._globalScore / ingredientCount;
        }

        _Score += (int)(recipeScore._GlobalScore * 100);

        if (OnRecipeShipped != null) OnRecipeShipped(recipeScore);
    }
}
