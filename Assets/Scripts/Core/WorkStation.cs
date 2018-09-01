using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkStation : MonoBehaviour {

    public static event Action<WorkStation> OnStationSelected;
    public static event Action<WorkStation> OnStationUnselected;

    [SerializeField] private Transform _anchor;
    [SerializeField] private float _selectionAngle;

    public Transform Anchor { get { return _anchor; } }
    public float SelectionAngle { get { return _selectionAngle; } }

    public void SelectStation() {
        if (OnStationSelected != null) OnStationSelected(this);
    }

    public void UnselectStation() {
        if (OnStationUnselected != null) OnStationUnselected(this);
    }
}
