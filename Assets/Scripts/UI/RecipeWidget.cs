using System;
using System.Collections.Generic;
using UnityEngine;

public class RecipeWidget : MonoBehaviour {

    #region ATTRIBUTES

    public List<IngredientWidget> _IngredientWidgets;

    [SerializeField]
    public List<SGrade> _grades;

    #endregion // ATTRIBUTES

    [Serializable]
    public struct SGrade
    {
        public Sprite _sprite;
        public float _scoreThreshold;
    }

    public void Start()
    {
        ObjectiveManager.OnRecipeChanged += OnRecipeChanged;
        ObjectiveManager.OnRecipeShipped += OnRecipeShipped;
    }

    public void OnRecipeChanged(Recipes.SRecipe newRecipe)
    {
        int recipeCount = newRecipe._Ingredients.Count;
        for (int i = 0; i < _IngredientWidgets.Count; i++)
        {
            _IngredientWidgets[i].SetIngredient((recipeCount > i) ? newRecipe._Ingredients[i] : null);
            _IngredientWidgets[i].gameObject.SetActive(recipeCount > i);
        }
    }

    public void OnRecipeShipped(ObjectiveManager.SRecipeScore recipeScore)
    {
        Debug.Log("Recipe SHIPPED");
    }
}
