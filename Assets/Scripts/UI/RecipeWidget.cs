using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class RecipeWidget : MonoBehaviour {

    #region ATTRIBUTES

    public List<IngredientWidget> _IngredientWidgets;
    public TMPro.TextMeshProUGUI _RecipeName;
    public Image _Grade;

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

    private void OnEnable() {
        PreparationStation.OnPreparationStationUsed += OnPreparationStationUsed;
    }
    private void OnDisable() {
        PreparationStation.OnPreparationStationUsed -= OnPreparationStationUsed;
    }

    private void OnPreparationStationUsed() {
        int amtIngredients = PreparationStation.AmtIngredientsPlaced();

        for (int i = 0; i < _IngredientWidgets.Count; i++) {
            if (i == amtIngredients)
                _IngredientWidgets[i].SetEmphasis();
            else
                _IngredientWidgets[i].RemoveEmphasis();
        }
    }


    public void OnRecipeChanged(Recipes.SRecipe newRecipe)
    {
        int recipeCount = newRecipe._Ingredients.Count;
        for (int i = 0; i < _IngredientWidgets.Count; i++)
        {
            _IngredientWidgets[i].SetIngredient((recipeCount > i) ? newRecipe._Ingredients[i] : null);
            _IngredientWidgets[i].gameObject.SetActive(recipeCount > i);

            if (i == 0) _IngredientWidgets[i].SetEmphasis();
            else _IngredientWidgets[i].RemoveEmphasis();
        }

        _RecipeName.text = newRecipe._Name;
        SetGrade(null);
    }

    public void OnRecipeShipped(ObjectiveManager.SRecipeScore recipeScore)
    {
        for (int i = 0; i < _IngredientWidgets.Count; i++)
        {
            float ingredientScore = (recipeScore._IngredientScores.Count > i) ? recipeScore._IngredientScores[i]._globalScore : 0f;
            _IngredientWidgets[i].SetGrade(GetGradeSprite(ingredientScore));
        }

        SetGrade(GetGradeSprite(recipeScore._GlobalScore));

        Debug.Log("Recipe SHIPPED");
    }

    public Sprite GetGradeSprite(float score)
    {
        foreach (SGrade grade in _grades)
        {
            if (score >= grade._scoreThreshold)
            {
                return grade._sprite;
            }
        }

        return null;
    }

    public void SetGrade(Sprite sprite)
    {
        _Grade.sprite = sprite;
        _Grade.color = new Color(1f, 1f, 1f, 0f);

        if (sprite != null)
        {
            _Grade.rectTransform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
            _Grade.rectTransform.DOScale(1f, 0.5f).SetEase(Ease.OutCirc);
            _Grade.DOFade(1f, 0.5f);
        }
    }
}
