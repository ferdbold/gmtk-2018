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
            _IngredientWidgets[i].gameObject.SetActive(recipeCount > i);
            if (recipeCount > i)
            {
                _IngredientWidgets[i]._IngredientPrefab = newRecipe._Ingredients[i];
            }
        }
    }
}
