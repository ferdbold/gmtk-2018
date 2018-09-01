using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorStation : WorkStation {

    public static event Action OnColorStationUsed;

    public override void UseStation() {
        base.UseStation();

        if (OnColorStationUsed != null) OnColorStationUsed();
    }
}