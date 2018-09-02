using UnityEngine;
using UnityEngine.UI;

public class MainMenuWidget : MonoBehaviour {

    #region ATTRIBUTES

    public Button _StartGameButton;

    #endregion

    public void OnEnable()
    {
        ObjectiveManager.OnGameStarted += OnGameStarted;
    }

    public void OnDisable()
    {
        ObjectiveManager.OnGameStarted -= OnGameStarted;
    }

    public void RequestStartGame()
    {
        GameManager.StartGame();
    }

    public void OnGameStarted()
    {
        gameObject.SetActive(false);
    }
}
