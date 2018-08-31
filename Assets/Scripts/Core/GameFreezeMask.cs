using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct GameFreezeMask
{
    [Flags]
    public enum FreezeContext
    {
        Pause = 0x01,
        ScreenShake = 0x02,
    }

    [SerializeField]
    private FreezeContext _freezeContext;

    public FreezeContext Flags
    {
        get { return _freezeContext; }
    }

    public GameFreezeMask(FreezeContext context) : this()
    {
        _freezeContext = context;
    }

    public void Add(FreezeContext context)
    {
        _freezeContext |= context;
    }

    public void Remove(FreezeContext context)
    {
        FreezeContext inverted = ~context;
        _freezeContext = ~(inverted | context);
    }
}