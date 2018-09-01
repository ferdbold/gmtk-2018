using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class InventoryManager : BaseManager<InventoryManager> {

    private Transform _grabbedTransform;
    private Tweener _objectMoveTween = null;
    private Tweener _objectRotateTween = null;

    [SerializeField] private float _objectAnimationTime = 2f;
    [SerializeField] private AnimationCurve _objectAnimationCurve;

    #region LIFECYCLE
    public override void OnStartManager() {
        
    }

    public override void OnRegisterCallbacks() {
        WorkStation.OnStationSelected += OnStationSelected;
        WorkStation.OnStationUnselected += OnStationUnselected;
    }

    public override void OnUnregisterCallbacks() {
        WorkStation.OnStationSelected -= OnStationSelected;
        WorkStation.OnStationUnselected -= OnStationUnselected;
    }

    public override void OnUpdateManager(float deltaTime) {

    }

    #endregion

    #region CALLBACKS

    private void OnStationSelected(WorkStation station) {
        if (_grabbedTransform != null) {
            _objectMoveTween = _grabbedTransform.DOMove(station.Anchor.position, _objectAnimationTime).SetEase(_objectAnimationCurve);
            _objectRotateTween = _grabbedTransform.DORotate(station.Anchor.rotation.eulerAngles, _objectAnimationTime).SetEase(_objectAnimationCurve);
        }
    }

    private void OnStationUnselected(WorkStation station) {
        if(_grabbedTransform != null) {
            _objectMoveTween = _grabbedTransform.DOMove(PlayerManager.InventoryTransform.position, _objectAnimationTime).SetEase(_objectAnimationCurve);
            _objectRotateTween = _grabbedTransform.DORotate(PlayerManager.InventoryTransform.rotation.eulerAngles, _objectAnimationTime).SetEase(_objectAnimationCurve);
        }
    }   

    private void KillTweens() {
        if (_objectMoveTween != null) _objectMoveTween.Kill();
        if (_objectRotateTween != null) _objectRotateTween.Kill();
    }

    #endregion
}
