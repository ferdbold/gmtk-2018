using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MainMenuWidget : MonoBehaviour {

    #region ATTRIBUTES

    public Button _StartGameButton;
    public CanvasGroup _group;
    public Transform _tutorialCameraAnchor;
    private bool _inTutorial = false;
    public Transform _modelScale;

    private Vector3 _prevCamPos;
    private Vector3 _prevCamRot;
    private Transform _prevCamParent;
    #endregion

    public void OnEnable()
    {
        ObjectiveManager.OnGameStarted += OnGameStarted;
    }

    public void OnDisable()
    {
        ObjectiveManager.OnGameStarted -= OnGameStarted;
    }

    public void Update() {
        if (_inTutorial && Input.GetMouseButtonDown(0)) {
            BackToMenu();
        }
    }

    public void RequestStartGame()
    {
        GameManager.StartGame();
    }

    public void OnGameStarted()
    {
        gameObject.SetActive(false);
    }

    public void RequestTutorial() {
        _group.interactable = false;
        _group.DOFade(0f, 1f);
        _inTutorial = true;

        _prevCamPos = Camera.main.transform.position;
        _prevCamRot = Camera.main.transform.rotation.eulerAngles;
        _prevCamParent = Camera.main.transform.parent;

        Camera.main.transform.DOMove(_tutorialCameraAnchor.position, 1.5f).SetEase(Ease.InOutCubic);
        Camera.main.transform.DORotate(_tutorialCameraAnchor.rotation.eulerAngles, 1.5f).SetEase(Ease.InOutCubic);
        _modelScale.DOScale(Vector3.zero, 0.5f);
    }

    public void BackToMenu() {
        _group.DOFade(1f, 1f).OnComplete(ReactivateMenu);
        _inTutorial = false;

        Camera.main.transform.DOMove(_prevCamPos, 1.5f).SetEase(Ease.InOutCubic);
        Camera.main.transform.DORotate(_prevCamRot, 1.5f).SetEase(Ease.InOutCubic);
        _modelScale.DOScale(Vector3.one, 0.5f);
    }

    private void ReactivateMenu() {
        _group.interactable = true;
    }
}
