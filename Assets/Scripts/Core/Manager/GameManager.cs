using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [System.Serializable]
    public class ManagerData
    {
        public GameObject Prefab = null;
    }

    [SerializeField] private ManagerData[] _managersPrefab;
    [SerializeField] private IManager[] _managers;

    private static bool _timescaleFrozen = false;
    private static float _timescale = 1f;
    private static GameFreezeMask _freezeMask;
    private static bool _isPaused = false;

    #region ACCESSORS
    public static bool TimescaleFrozen
    {
        get { return _timescaleFrozen; }
    }

    public static float TimeScale
    {
        get { return _timescale; }
        set { _timescale = value; }
    }
    #endregion

    #region LIFECYCLE
    private void Awake()
    {
        _freezeMask = new GameFreezeMask(0);
        _managers = new IManager[_managersPrefab.Length];

        for(int i =0; i < _managersPrefab.Length; ++i)
        {
            GameObject manager = Instantiate(_managersPrefab[i].Prefab);
            _managers[i] = manager.GetComponentInChildren<IManager>(true);
        }

        Call_PreStartManager();
        Call_RegisterCallbacks();
    }

    private void Start()
    {
        Call_StartManager();
        Call_PostStartManager();
        Call_StartGame();
    }

    private void Update()
    {
        Call_UpdateManager(Time.deltaTime);
    }

    private void LateUpdate()
    {
        Call_LateUpdateManager(Time.deltaTime);
    }

    private void EndGame()
    {
        Call_StopGame();
        Call_UnregisterCallbacks();
        Call_StopManager();
    }
    #endregion

    #region Manager Calls

    private void Call_PreStartManager()
    {
        for(int i = 0; i <  _managers.Length; ++i)
        {
            _managers[i].OnPreStartManager();
        }
    }

    private void Call_StartManager()
    {
        for (int i = 0; i < _managers.Length; ++i)
        {
            _managers[i].OnStartManager();
        }
    }

    private void Call_PostStartManager()
    {
        for (int i = 0; i < _managers.Length; ++i)
        {
            _managers[i].OnPostStartManager();
        }
    }

    private void Call_RegisterCallbacks()
    {
        for (int i = 0; i < _managers.Length; ++i)
        {
            _managers[i].OnRegisterCallbacks();
        }
    }
    private void Call_UnregisterCallbacks()
    {
        for (int i = 0; i < _managers.Length; ++i)
        {
            _managers[i].OnUnregisterCallbacks();
        }
    }

    private void Call_StartGame()
    {
        for (int i = 0; i < _managers.Length; ++i)
        {
            _managers[i].OnStartGame();
        }
    }
    private void Call_StopGame()
    {
        for (int i = 0; i < _managers.Length; ++i)
        {
            _managers[i].OnStopGame();
        }
    }
    private void Call_StopManager()
    {
        for (int i = 0; i < _managers.Length; ++i)
        {
            _managers[i].OnStopManager();
        }
    }

    private void Call_UpdateManager(float deltaTime)
    {
        for (int i = 0; i < _managers.Length; ++i)
        {
            _managers[i].OnUpdateManager(deltaTime);
        }
    }

    private void Call_LateUpdateManager(float deltaTime)
    {
        for (int i = 0; i < _managers.Length; ++i)
        {
            _managers[i].OnLateUpdateManager(deltaTime);
        }
    }

    #endregion

    #region API
    public static void AddFreeze(GameFreezeMask.FreezeContext context)
    {
        _freezeMask.Add(context);
        _timescaleFrozen = true;
        Time.timeScale = 0f;
        UpdateIsPaused();
    }
    public static void RemoveFreeze(GameFreezeMask.FreezeContext context)
    {
        _freezeMask.Remove(context);
        UpdateIsPaused();

        if (_freezeMask.Flags == 0)
        {
            _timescaleFrozen = false;
            Time.timeScale = TimeScale;
        }
    }

    private static void UpdateIsPaused()
    {
        _isPaused = ((int)_freezeMask.Flags & (1 << (int)GameFreezeMask.FreezeContext.Pause)) != 0;
    }
    public static bool IsPaused()
    {
        return _isPaused;;
    }

    #endregion
}
