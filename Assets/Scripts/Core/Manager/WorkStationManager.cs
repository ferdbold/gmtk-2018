using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkStationManager : BaseManager<WorkStationManager> {

    private WorkStation[] _workStations;
    private WorkStation _selectedStation = null;

    #region LIFECYCLE
    public override void OnStartManager() {
        Object[] stationGOs = GameObject.FindObjectsOfType(typeof(WorkStation));
        _workStations = new WorkStation[stationGOs.Length];
        for (int i =0; i < stationGOs.Length; ++i){
            _workStations[i] = ((GameObject) stationGOs[i]).GetComponent<WorkStation>();
        }
    }

    public override void OnRegisterCallbacks() {

    }

    public override void OnUnregisterCallbacks() {

    }

    public override void OnUpdateManager(float deltaTime) {
        UpdateSelection();
    }

    #endregion

    private void UpdateSelection() {
        float closestAngle = 360f;
        WorkStation closestStation = null;
        Vector3 playerForward = Vector3.ProjectOnPlane(PlayerManager.Forward, Vector3.up);

        for(int i = 0; i < _workStations.Length; ++i) {
            WorkStation station = _workStations[i];
            Vector3 fromTo = station.Anchor.position - PlayerManager.Position;
            float angle = Vector3.Angle(fromTo, playerForward);

            if (station.SelectionAngle > angle && closestAngle > angle) {
                closestAngle = angle;
                closestStation = station;
            }

            if(closestStation != null && closestStation != _selectedStation) {
                if (_selectedStation != null)
                    _selectedStation.UnselectStation();
                _selectedStation = closestStation;
                _selectedStation.SelectStation();
            }

        }
    }

}
