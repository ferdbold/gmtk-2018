using System;
using UnityEngine;

public class ShapeStation : WorkStation {

    public static event Action OnShapeStationUsed;

    public override void UseStation() {
        base.UseStation();

        if (OnShapeStationUsed != null) OnShapeStationUsed();
    }
}
