using System;
using System.Collections;
using UnityEngine;

public class ShapeStation : WorkStation {

    public float LenghtChangePerHit = 0.025f;
    public static event Action OnShapeStationUsed;

    public override void UseStation(Ingredient ingredient) {
        base.UseStation(ingredient);

        StartCoroutine(ChangeLenghtTimer(ingredient));
    }

    private IEnumerator ChangeLenghtTimer(Ingredient ingredient) {
        yield return new WaitForSeconds(0.05f);
        ingredient.ChangeLenght(LenghtChangePerHit);
        if (OnShapeStationUsed != null) OnShapeStationUsed();
    }
}
