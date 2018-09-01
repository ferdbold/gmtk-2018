using UnityEngine;

public class IngredientWidget : MonoBehaviour {

    #region ATTRIBUTES

    public Ingredient _IngredientPrefab;

    #endregion // ATTRIBUTES

    private Ingredient _Ingredient;
    private TMPro.TextMeshProUGUI _Label;

    public void Start()
    {
        _Label = transform.Find("Label").GetComponent<TMPro.TextMeshProUGUI>();
    }

    public void Update()
    {
        if (_Ingredient != _IngredientPrefab)
        {
            if (_IngredientPrefab != null)
            {
                _Label.text = _IngredientPrefab.name;
                _Ingredient = GameObject.Instantiate(_IngredientPrefab, transform);
            }
            else
            {
                _Label.text = "";
                GameObject.Destroy(_Ingredient.gameObject);
            }
        }
    }
}
