using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkStation : MonoBehaviour {

    public enum StationType {
        ColorStation = 0,
        ShapeStation = 1
    }
    
    public static event Action<WorkStation> OnStationSelected;
    public static event Action<WorkStation> OnStationUnselected;

    [SerializeField] private Transform _anchor;
    [SerializeField] private float _selectionAngle;
    [SerializeField] private StationType _stationType;

    public Transform Anchor { get { return _anchor; } }
    public float SelectionAngle { get { return _selectionAngle; } }

    public void SelectStation() {
        if (OnStationSelected != null) OnStationSelected(this);
    }

    public void UnselectStation() {
        if (OnStationUnselected != null) OnStationUnselected(this);
    }
}
