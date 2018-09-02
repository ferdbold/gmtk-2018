using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveManager : BaseManager<ObjectiveManager> {

    #region ATTRIBUTES

    public int _Score;

    public float _GameDuration;
    public float _RemainingGameTime;

    public float _RecipeTickInterval;
    public float _RecipeTickRemaining;

    public float _RecipeInterludeInterval;
    public float _RecipeInterludeRemaining;

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
    public static event Action OnRecipeInterlude;

    private bool _firstRecipeReceived = false;

    public override void OnStartGame()
    {
        _RemainingGameTime = _GameDuration;
        _RecipeInterludeRemaining = _RecipeInterludeInterval;
        _RecipeTickRemaining = _RecipeTickInterval;
    }

    public override void OnUpdateManager(float deltaTime)
    {
        if (_firstRecipeReceived)
        {
            _RemainingGameTime -= deltaTime;
            
            if (_RemainingGameTime < 0f)
            {
                GameEnd();
            }
        }

        if (_RecipeInterludeRemaining > 0)
        {
            _RecipeInterludeRemaining -= deltaTime;

            if (_RecipeInterludeRemaining <= 0)
            {
                GenerateNewRecipe();
                _firstRecipeReceived = true;
            }
        }
        else
        {
            _RecipeTickRemaining -= deltaTime;

            if (_RecipeTickRemaining <= 0)
            {
                ShipRecipe();

                _RecipeInterludeRemaining = _RecipeInterludeInterval;
                _RecipeTickRemaining = _RecipeTickInterval;

                if (OnRecipeInterlude != null) OnRecipeInterlude();
            }
        }
    }

    public bool IsInterlude() { return _RecipeInterludeRemaining > 0; }

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
        _RecipeTickRemaining = 0f;

        if (OnRecipeShipped != null) OnRecipeShipped(recipeScore);
    }

    public void GameEnd()
    {
        // TODO
        Debug.Log("Game Over");
    }
}
