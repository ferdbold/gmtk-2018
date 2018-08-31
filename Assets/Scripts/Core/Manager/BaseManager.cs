using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseManager<T> : Singleton<T>, IManager where T : BaseManager<T> {

    #region LIFECYCLE

    public virtual void OnPreStartManager() { }
    public virtual void OnStartManager(){ }
    public virtual void OnPostStartManager(){ }
    public virtual void OnRegisterCallbacks(){ }

    public virtual void OnStartGame(){ }

    public virtual void OnUpdateManager(float deltaTime){ }
    public virtual void OnLateUpdateManager(float deltaTime){ }

    public virtual void OnStopGame(){ }

    public virtual void OnUnregisterCallbacks(){ }
    public virtual void OnStopManager(){ }

    #endregion

    #region PAUSE
    public virtual void OnPauseGame(){ }

    public virtual void OnUnpauseGame(){ }


    #endregion
}

