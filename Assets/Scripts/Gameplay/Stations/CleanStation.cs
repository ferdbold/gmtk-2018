using System;
using System.Collections.Generic;
using UnityEngine;

public class CleanStation : WorkStation {

    public static event Action OnCleanStationUsed;

    public override void UseStation() {
        base.UseStation();

        if (OnCleanStationUsed != null) OnCleanStationUsed();
    }
}

