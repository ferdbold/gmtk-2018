using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class OvenStation : WorkStation {

    public static event Action OnOvenStationUsed;

    [SerializeField] private Light _ovenLight;
    [SerializeField] private float _heatChangePerSecond = 0.5f;

    private bool _stationInUse = false;
    private int _animatorUsingHash;
    private float _defaultLightIntensity;

    public override void Setup() {
        base.Setup();

        _defaultLightIntensity = _ovenLight.intensity;
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
        _ovenLight.DOIntensity(_defaultLightIntensity * 3f, 0.5f);

        while (_stationInUse) {
            yield return new WaitForSeconds(0.05f);
            ingredient.AddHeat(_heatChangePerSecond * 0.05f);
        }

        _ovenLight.DOIntensity(_defaultLightIntensity, 0.5f);
        _animator.SetBool(_animatorUsingHash, false);
    }
}
