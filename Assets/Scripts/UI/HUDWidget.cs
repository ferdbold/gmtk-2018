using UnityEngine;

public class HUDWidget : MonoBehaviour {

    public void Start()
    {
        ObjectiveManager.OnGameStarted += OnGameStarted;
    }

    public void OnGameStarted()
    {
        GetComponent<Canvas>().enabled = true;
    }
}
