using UnityEngine;

public class RecipeTickWidget : MonoBehaviour {

    #region ATTRIBUTES

    public TMPro.TextMeshProUGUI _Value;

    #endregion // ATTRIBUTES

    public void Update()
    {
        _Value.text = ":" + ObjectiveManager.Instance._RecipeTickRemaining.ToString("N0");
    }
}
