using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class InventoryManager : BaseManager<InventoryManager> {

    [SerializeField] private LayerMask _objectLayerMask;
    [SerializeField] private float _objectAnimationTime = 2f;
    [SerializeField] private AnimationCurve _objectAnimationCurve;

    private Ingredient _grabbedIngredient = null;
    private Tweener _objectMoveTween = null;
    private Tweener _objectRotateTween = null;

    #region LIFECYCLE
    public override void OnStartManager() {

    }

    public override void OnRegisterCallbacks() {
        WorkStation.OnStationSelected += OnStationSelected;
        WorkStation.OnStationUnselected += OnStationUnselected;
        ShapeStation.OnShapeStationUsed += OnShapeStationUsed;
        PreparationStation.OnPreparationStationUsed += OnPreparationStationUsed;
    }

    public override void OnUnregisterCallbacks() {
        WorkStation.OnStationSelected -= OnStationSelected;
        WorkStation.OnStationUnselected -= OnStationUnselected;
        ShapeStation.OnShapeStationUsed -= OnShapeStationUsed;
        PreparationStation.OnPreparationStationUsed -= OnPreparationStationUsed;
    }

    public override void OnUpdateManager(float deltaTime) {
        HandleMouseInput();
    }

    #endregion

    #region GAMEPLAY

    private void HandleMouseInput() {
        if (_grabbedIngredient == null && Input.GetMouseButtonDown(0)) {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100.0f, _objectLayerMask)) {
                Ingredient ing = hit.collider.gameObject.GetComponent<Ingredient>();
                _grabbedIngredient = ing;
                AttachObjectToInventory();
            }
        }
    }

    private void KillTweens() {
        if (_objectMoveTween != null) _objectMoveTween.Kill();
        if (_objectRotateTween != null) _objectRotateTween.Kill();
    }

    private void AttachObjectToInventory() {
        if (_grabbedIngredient != null) {
            KillTweens();
            _grabbedIngredient._OnConveyorBelt = false;
            _grabbedIngredient.transform.transform.parent = PlayerManager.InventoryTransform;
            _objectMoveTween = _grabbedIngredient.transform.DOLocalMove(Vector3.zero, _objectAnimationTime).SetEase(_objectAnimationCurve);
            _objectRotateTween = _grabbedIngredient.transform.DOLocalRotate(Quaternion.identity.eulerAngles, _objectAnimationTime).SetEase(_objectAnimationCurve);
        }
    }

    private void DropObject() {
        if (_grabbedIngredient != null) {
            KillTweens();
            _grabbedIngredient = null;
        }
    }

    #endregion

    #region CALLBACKS

    private void OnStationSelected(WorkStation station) {
        if (_grabbedIngredient != null) {
            KillTweens();
            _grabbedIngredient.transform.transform.parent = station.Anchor;
            _objectMoveTween = _grabbedIngredient.transform.DOLocalMove(Vector3.zero, _objectAnimationTime).SetEase(_objectAnimationCurve);
            _objectRotateTween = _grabbedIngredient.transform.DOLocalRotate(Quaternion.identity.eulerAngles, _objectAnimationTime).SetEase(_objectAnimationCurve);
        }
    }

    private void OnStationUnselected(WorkStation station) {
        AttachObjectToInventory();
    }

    private void OnShapeStationUsed() {
        if(_grabbedIngredient != null) {
            _grabbedIngredient.transform.DOShakeRotation(0.1f, 20, 2, 90, false);
            _grabbedIngredient.transform.DOShakePosition(0.1f, 0.05f, 2, 90, false);
        }
    }

    private void OnPreparationStationUsed() {
        DropObject();
    }

    #endregion

    #region Static API

    public static bool IsGrabbingIngredient() {
        return Instance._grabbedIngredient != null;
    }

    public static Ingredient GrabbedIngredient() {
        return Instance._grabbedIngredient;
    }

    #endregion
}
