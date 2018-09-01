using System.Collections.Generic;
using UnityEngine;

public class RecipeWidget : MonoBehaviour {

    #region ATTRIBUTES

    public List<IngredientWidget> _IngredientWidgets;

    #endregion // ATTRIBUTES

    public void Start()
    {
        ObjectiveManager.OnRecipeChanged += OnRecipeChanged;
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
}
