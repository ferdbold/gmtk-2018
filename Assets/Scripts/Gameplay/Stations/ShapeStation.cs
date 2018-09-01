using System;
using UnityEngine;

public class ShapeStation : WorkStation {

    public float LenghtChangePerHit = 0.025f;
    public static event Action OnShapeStationUsed;

    public override void UseStation(Ingredient ingredient) {
        base.UseStation(ingredient);

        ingredient.ChangeLenght(LenghtChangePerHit);

        if (OnShapeStationUsed != null) OnShapeStationUsed();
    }
}
