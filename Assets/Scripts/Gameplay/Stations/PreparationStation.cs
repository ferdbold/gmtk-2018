using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreparationStation : WorkStation {

    public static event Action OnPreparationStationUsed;

    public override void UseStation(Ingredient ingredient) {
        base.UseStation(ingredient);

        if (OnPreparationStationUsed != null) OnPreparationStationUsed();
    }
}


