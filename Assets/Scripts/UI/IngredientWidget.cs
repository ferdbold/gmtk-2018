using UnityEngine;

public class IngredientWidget : MonoBehaviour {

    public Ingredient _IngredientPrefab;
    private Ingredient _Ingredient;
    private TMPro.TextMeshProUGUI _Label;
    private Transform _Holder;

    public void Start()
    {
        _Label = transform.Find("Label").GetComponent<TMPro.TextMeshProUGUI>();
        _Holder = transform.Find("Holder");
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
    }
}
