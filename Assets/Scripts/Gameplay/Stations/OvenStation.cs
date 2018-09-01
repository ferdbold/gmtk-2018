using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OvenStation : WorkStation {

    public static event Action OnOvenStationUsed;

    [SerializeField] private float _heatChangePerSecond = 0.5f;
    private bool _stationInUse = false;
    private int _animatorUsingHash;

    public override void Setup() {
        base.Setup();

        _animatorUsingHash = Animator.StringToHash("using");
    }

    public override void UseStation(Ingredient ingredient) {
        base.UseStation(ingredient);
        _stationInUse = true;

        StartCoroutine(ChangeHeatTimer(ingredient));
        if (OnOvenStationUsed != null) OnOvenStationUsed();
    }

    public override void UnselectStation() {
        base.UnselectStation();

        _stationInUse = false;
    }

    public override void UpdateStation(float deltaTime) {
        base.UpdateStation(deltaTime);

        if(Input.GetMouseButtonUp(0))
            _stationInUse = false;
    }

    private IEnumerator ChangeHeatTimer(Ingredient ingredient) {
        _animator.SetBool(_animatorUsingHash, true);

        while (_stationInUse) {
            yield return new WaitForSeconds(0.05f);
            ingredient.AddHeat(_heatChangePerSecond * 0.05f);
        }

        _animator.SetBool(_animatorUsingHash, false);
    }
}
