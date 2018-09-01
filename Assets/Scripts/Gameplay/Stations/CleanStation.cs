using System;
using System.Collections.Generic;
using UnityEngine;

public class CleanStation : WorkStation {

    public static event Action OnCleanStationUsed;

    public override void UseStation(Ingredient ingredient) {
        base.UseStation(ingredient);

        if (OnCleanStationUsed != null) OnCleanStationUsed();
    }
}

