using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IManager
{

    #region LIFECYCLE

    /// <summary>
    /// Setup References
    /// </summary>
    void OnPreStartManager();

    /// <summary>
    /// Setup and cache Data
    /// </summary>
    void OnStartManager();

    /// <summary>
    /// Setup data that needs info from other managers
    /// </summary>
    void OnPostStartManager();

    /// <summary>
    /// Register Callbacks, Done after start of managers
    /// </summary>
    void OnRegisterCallbacks();

    /// <summary>
    /// Setup Completed, Game Start
    /// </summary>
    void OnStartGame();

    /// <summary>
    /// Update Loop
    /// </summary>
    void OnUpdateManager(float deltaTime);

    /// <summary>
    /// Late Update Loop
    /// </summary>
    void OnLateUpdateManager(float deltaTime);

    /// <summary>
    /// Clear Data
    /// </summary>
    void OnStopGame();

    /// <summary>
    /// Unregister Callbacks
    /// </summary>
    void OnUnregisterCallbacks();

    /// <summary>
    /// Destroy and reset references
    /// </summary>
    void OnStopManager();

    #endregion

    #region PAUSE
    void OnPauseGame();

    void OnUnpauseGame();

    #endregion IN-GAME

}
