using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlossStation : WorkStation {

    public static event Action OnGlossStationUsed;

    [SerializeField] private float _glossChangePerSecond = 0.5f;
    private bool _stationInUse = false;
    private int _animatorUsingHash;

    public override void Setup() {
        base.Setup();

        _animatorUsingHash = Animator.StringToHash("using");
    }

    public override void UseStation(Ingredient ingredient) {
        base.UseStation(ingredient);
        _stationInUse = true;

        StartCoroutine(ChangeGlossTimer(ingredient));
        if (OnGlossStationUsed != null) OnGlossStationUsed();
    }

    public override void UnselectStation() {
        base.UnselectStation();

        _stationInUse = false;
    }

    public override void UpdateStation(float deltaTime) {
        base.UpdateStation(deltaTime);

        if (Input.GetMouseButtonUp(0))
            _stationInUse = false;
    }

    private IEnumerator ChangeGlossTimer(Ingredient ingredient) {
        _animator.SetBool(_animatorUsingHash, true);

        while (_stationInUse) {
            yield return new WaitForSeconds(0.05f);
            ingredient.AddHeat(_glossChangePerSecond * 0.05f);
        }

        _animator.SetBool(_animatorUsingHash, false);
    }
}


