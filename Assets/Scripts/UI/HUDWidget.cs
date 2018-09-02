using UnityEngine;
using UnityEngine.UI;

public class HUDWidget : MonoBehaviour {

    #region ATTRIBUTES

    public GameObject _GameEndWidget;

    #endregion // ATTRIBUTES

    public void Start()
    {
        ObjectiveManager.OnGameStarted += OnGameStarted;
        ObjectiveManager.OnGameEnded += OnGameEnded;
    }

    public void OnGameStarted()
    {
        GetComponent<Canvas>().enabled = true;
    }

    public void OnGameEnded()
    {
        _GameEndWidget.SetActive(true);
    }
}
