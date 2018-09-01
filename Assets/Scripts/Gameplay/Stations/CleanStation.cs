using System;
using System.Collections;
using UnityEngine;

public class CleanStation : WorkStation {

    public static event Action OnCleanStationUsed;

    public override void UseStation(Ingredient ingredient) {
        base.UseStation(ingredient);

        StartCoroutine(ChangeColorTimer(ingredient));
    }

    private IEnumerator ChangeColorTimer(Ingredient ingredient) {
        yield return new WaitForSeconds(0.15f);
        ingredient.ResetColor();
        if (OnCleanStationUsed != null) OnCleanStationUsed();
    }
}

