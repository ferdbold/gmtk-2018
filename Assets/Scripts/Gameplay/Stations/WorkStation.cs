using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkStation : MonoBehaviour {

    public enum StationType {
        ColorStation = 0,
        ShapeStation = 1,
        CleanStation = 2,
        PreparationStation = 3,
    }
    
    public static event Action<WorkStation> OnStationSelected;
    public static event Action<WorkStation> OnStationUnselected;
    public static event Action<WorkStation> OnStationUsed;

    [SerializeField] private Transform _anchor;
    [SerializeField] private float _selectionAngle;
    [SerializeField] private StationType _stationType;

    [Header("Animation")]
    [SerializeField] private Animator _animator;
    private int _animatorSelectedHash;
    private int _animatorUsedHash;

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
        if (OnStationSelected != null) OnStationSelected(this);
    }

    public virtual void UnselectStation() {
        _animator.SetBool(_animatorSelectedHash, false);
        _animator.ResetTrigger(_animatorUsedHash);
        if (OnStationUnselected != null) OnStationUnselected(this);
    }

    public virtual void UseStation() {
        _animator.SetTrigger(_animatorUsedHash);
        if (OnStationUsed != null) OnStationUsed(this);
    }
}
