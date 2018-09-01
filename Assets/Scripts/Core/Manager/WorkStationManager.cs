using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkStationManager : BaseManager<WorkStationManager> {

    [SerializeField] private LayerMask _workstationLayer;

    private WorkStation[] _workStations;
    private WorkStation _selectedStation = null;

    #region LIFECYCLE
    public override void OnStartManager() {
        _workStations = FindObjectsOfType(typeof(WorkStation)) as WorkStation[];
        foreach (WorkStation station in _workStations) {
            station.Setup();
        }
    }

    public override void OnRegisterCallbacks() {
    }

    public override void OnUnregisterCallbacks() {

    }

    public override void OnUpdateManager(float deltaTime) {

        if (InventoryManager.IsGrabbingIngredient()) {
            UpdateSelection();
            HandleStationUse();
        } else {
            UnselectStation();
        }
    }

    #endregion

    #region GAMEPLAY 
    private void UpdateSelection() {
        float closestAngle = 360f;
        WorkStation closestStation = null;
        Vector3 playerForward = Vector3.ProjectOnPlane(PlayerManager.Forward, Vector3.up);

        for(int i = 0; i < _workStations.Length; ++i) {
            WorkStation station = _workStations[i];
            Vector3 fromTo = Vector3.ProjectOnPlane(station.Anchor.position - PlayerManager.Position, Vector3.up);
            float angle = Vector3.Angle(fromTo, playerForward);

            if (station.SelectionAngle > angle && closestAngle > angle) {
                closestAngle = angle;
                closestStation = station;
            }
        }

        if (closestStation != null && closestStation != _selectedStation) {
            UnselectStation();
            _selectedStation = closestStation;
            _selectedStation.SelectStation();
        } else if (closestStation == null) {
            UnselectStation();
            _selectedStation = null;
        }
    }

    private void UnselectStation() {
        if (_selectedStation != null) {
            _selectedStation.UnselectStation();
        }
    }

    private void HandleStationUse() {
        if (Input.GetMouseButtonDown(0)) {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100.0f, _workstationLayer)) {
                if(hit.collider.gameObject.gameObject == _selectedStation.gameObject) {
                    _selectedStation.UseStation();
                }
            }
        }
    }

    #endregion
}
