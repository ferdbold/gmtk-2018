using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class IngredientWidget : MonoBehaviour {

    public Ingredient _IngredientPrefab;
    private Ingredient _Ingredient;

    private TMPro.TextMeshProUGUI _Label;
    private Transform _Holder;
    private Image _Grade;

    public void Start()
    {
        _Label = transform.Find("Label").GetComponent<TMPro.TextMeshProUGUI>();
        _Holder = transform.Find("Holder");
        _Grade = transform.Find("Grade").GetComponent<Image>();
    }

    public void SetIngredient(Ingredient ingredient)
    {
        if (ingredient == null)
        {
            _Label.text = "";
            GameObject.Destroy(_Ingredient);
            return;
        }

        if (_Ingredient != null)
        {
            GameObject.Destroy(_Ingredient.gameObject);
        }
        _Label.text = ingredient.name;
        _Ingredient = GameObject.Instantiate(ingredient, _Holder);
        _Ingredient.transform.localPosition = Vector3.zero;
        _Ingredient.gameObject.layer = 5;

        SetGrade(null);
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

    public void SetEmphasis() {
        if (_Ingredient != null)
            _Ingredient.transform.localScale = Vector3.one * 2.0f;
    }
    public void RemoveEmphasis() {
        if (_Ingredient != null)
            _Ingredient.transform.localScale = Vector3.one * 0.6f;
    }
}
