using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class RecipeTickWidget : MonoBehaviour {

    #region ATTRIBUTES

    public TMPro.TextMeshProUGUI _Value;
    public Image _NewRecipeLabel;
    public Image _Arrow;

    public Color _NormalColor;
    public Color _InterludeColor;

    #endregion // ATTRIBUTES

    public void Start()
    {
        OnRecipeInterlude();
    }

    public void OnEnable()
    {
        ObjectiveManager.OnRecipeChanged += OnRecipeChanged;
        ObjectiveManager.OnRecipeInterlude += OnRecipeInterlude;
    }

    public void OnDisable()
    {
        ObjectiveManager.OnRecipeChanged -= OnRecipeChanged;
        ObjectiveManager.OnRecipeInterlude -= OnRecipeInterlude;
    }

    public void Update()
    {
        // Update remaining recipe time
        bool isInterlude = ObjectiveManager.Instance.IsInterlude();
        float time = (isInterlude)
            ? ObjectiveManager.Instance._RecipeInterludeRemaining
            : ObjectiveManager.Instance._RecipeTickRemaining;
        
        _Value.text = time.ToString("N0");
        _Value.color = (isInterlude) ? _InterludeColor : _NormalColor;

        // Update arrow
        float rotation = ObjectiveManager.Instance._RemainingGameTime / ObjectiveManager.Instance._GameDuration * 360f;
        _Arrow.rectTransform.rotation = Quaternion.Euler(0, 0, rotation);
    }

    public void OnRecipeChanged(Recipes.SRecipe newRecipe)
    {
        _NewRecipeLabel.color = new Color(1f, 1f, 1f, 0f);
    }

    public void OnRecipeInterlude()
    {
        _NewRecipeLabel.rectTransform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        _NewRecipeLabel.rectTransform.DOScale(1f, 0.5f).SetEase(Ease.OutCirc);
        _NewRecipeLabel.color = new Color(1f, 1f, 1f, 0f);
        _NewRecipeLabel.DOFade(1f, 0.5f);
    }
}
