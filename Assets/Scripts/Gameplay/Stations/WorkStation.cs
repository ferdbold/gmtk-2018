using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class WorkStation : MonoBehaviour {

    public enum StationType {
        ColorStation = 0,
        ShapeStation = 1,
        CleanStation = 2,
        PreparationStation = 3,
        OvenStation = 4,
        GlossStation = 5
    }
    
    public static event Action<WorkStation> OnStationSelected;
    public static event Action<WorkStation> OnStationUnselected;
    public static event Action<WorkStation> OnStationUsed;

    [SerializeField] private Transform _anchor;
    [SerializeField] private float _selectionAngle;
    [SerializeField] private StationType _stationType;
    [SerializeField] private Light _stationSpotLight;
    [SerializeField] private CanvasGroup _stationUI;

    [Header("Animation")]
    [SerializeField] protected Animator _animator;
    private int _animatorSelectedHash;
    private int _animatorUsedHash;
    private Sequence _selectionSequence;

    public Transform Anchor { get { return _anchor; } }
    public float SelectionAngle { get { return _selectionAngle; } }

    public virtual void Setup() {
        if (_animator == null)
            _animator = GetComponent<Animator>();
        gameObject.layer = 10; //WorkStation layer
        _animatorSelectedHash = Animator.StringToHash("selected");
        _animatorUsedHash = Animator.StringToHash("used");
        _animator.SetBool(_animatorSelectedHash, false);
        _animator.ResetTrigger(_animatorUsedHash);
    }

    public virtual void UpdateStation(float deltaTime) {
        
    }

    public virtual void SelectStation() {
        _animator.SetBool(_animatorSelectedHash, true);
        HandleSelectionSequence(true);
        if (OnStationSelected != null) OnStationSelected(this);
    }

    public virtual void UnselectStation() {
        _animator.SetBool(_animatorSelectedHash, false);
        _animator.ResetTrigger(_animatorUsedHash);
        HandleSelectionSequence(false);
        if (OnStationUnselected != null) OnStationUnselected(this);
    }

    public virtual void UseStation(Ingredient ingredient) {
        _animator.SetTrigger(_animatorUsedHash);
        if (OnStationUsed != null) OnStationUsed(this);
    }

    private void HandleSelectionSequence(bool selected) {
        if(_selectionSequence != null) {
            _selectionSequence.Kill();
        }
        _selectionSequence = DOTween.Sequence();

        if(_stationSpotLight != null) {
            if(selected)
                _selectionSequence.Insert(0f, _stationSpotLight.DOIntensity(1f, 1f));
            else
                _selectionSequence.Insert(0f, _stationSpotLight.DOIntensity(0f, 0.5f));
        }

        if(_stationUI != null) {
            if (selected)
                _selectionSequence.Insert(0f, _stationUI.DOFade(1f, 2f));
            else
                _selectionSequence.Insert(0f, _stationUI.DOFade(0f, 0.5f));
        }


    }

}
