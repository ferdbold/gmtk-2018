using UnityEngine;

public class ScoreWidget : MonoBehaviour {

    #region ATTRIBUTES

    public TMPro.TextMeshProUGUI _Value;

    #endregion // ATTRIBUTES

    void Update () {
        _Value.text = ObjectiveManager.Instance._Score.ToString();
	}
}
