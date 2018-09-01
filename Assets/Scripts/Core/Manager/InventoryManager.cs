using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class InventoryManager : BaseManager<InventoryManager> {

    public Transform _grabbedTransform;
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
            KillTweens();
            _grabbedTransform.transform.parent = null;
            _objectMoveTween = _grabbedTransform.DOMove(station.Anchor.position, _objectAnimationTime).SetEase(_objectAnimationCurve);
            _objectRotateTween = _grabbedTransform.DORotate(station.Anchor.rotation.eulerAngles, _objectAnimationTime).SetEase(_objectAnimationCurve);
        }
    }

    private void OnStationUnselected(WorkStation station) {
        if(_grabbedTransform != null) {
            KillTweens();
            _grabbedTransform.transform.parent = PlayerManager.InventoryTransform;
            _objectMoveTween = _grabbedTransform.DOLocalMove(Vector3.zero, _objectAnimationTime).SetEase(_objectAnimationCurve);
            _objectRotateTween = _grabbedTransform.DOLocalRotate(Quaternion.identity.eulerAngles, _objectAnimationTime).SetEase(_objectAnimationCurve);
        }
    }   

    private void KillTweens() {
        if (_objectMoveTween != null) _objectMoveTween.Kill();
        if (_objectRotateTween != null) _objectRotateTween.Kill();
    }

    #endregion
}
