using System;
using System.Collections;
using UnityEngine;

public class CleanStation : WorkStation {

    public static event Action OnCleanStationUsed;

    [SerializeField] private ParticleSystem _cleanParticles;


    private Coroutine _useCoroutine = null;
    private bool _inCooldown = false;

    public override void UseStation(Ingredient ingredient) {
        base.UseStation(ingredient);

        if (_useCoroutine != null) StopCoroutine(_useCoroutine);
        _useCoroutine = StartCoroutine(ChangeColorTimer(ingredient));
    }

    private IEnumerator ChangeColorTimer(Ingredient ingredient) {
        yield return new WaitForSeconds(0.15f);

        if (!_inCooldown) {
            _cleanParticles.Emit(15);
        }
        _inCooldown = true;

        ingredient.ResetColor();
        if (OnCleanStationUsed != null) OnCleanStationUsed();

        yield return new WaitForSeconds(0.3f);
        _inCooldown = false;
    }
}

