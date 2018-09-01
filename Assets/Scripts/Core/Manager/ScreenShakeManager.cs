using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;



public class ScreenShakeManager : BaseManager<ScreenShakeManager>
{

    public class eScreenShakeIntensity
    {

        public static readonly float eVeryLow = 0.014f;
        public static readonly float eLow = 0.022f;
        public static readonly float eMedium = 0.038f;
        public static readonly float eHigh = 0.05f;
        public static readonly float eVeryHigh = 0.1f;
    }

    public class eScreenShakeDuration
    {

        public static readonly float eSmall = 0.04f;
        public static readonly float eMedium = 0.1f;
        public static readonly float eBig = 0.25f;

    }

    public enum eFreezeDuration
    {
        eSmall,
        eMedium,
        eBig,
        eVeryBig,
    }

    private Camera _camera;
    private Sequence _sequence;
    private float _overflowMultiplier = 0.40f;

    private float _currentIntensity = 0f;
    private float _currentDuration = 0f;
    private Vector3 _initialCameraPosition;
    private int _freezeStack = 0;

    public override void OnStartManager()
    {
        _camera = Camera.main;
        _initialCameraPosition = _camera.transform.position;
        _sequence = DOTween.Sequence();
    }

    public override void OnStartGame()
    {
        _freezeStack = 0;
    }

    public override void OnStopGame()
    {
        StopAllCoroutines();
        GameManager.RemoveFreeze(GameFreezeMask.FreezeContext.Pause);
    }

    public override void OnRegisterCallbacks() {
        ShapeStation.OnShapeStationUsed += OnShapeStationUsed;
        CleanStation.OnCleanStationUsed += OnCleanStationUsed;
        ColorStation.OnColorStationUsed += OnColorStationUsed;
        
    }

    public override void OnUnregisterCallbacks() {
        ShapeStation.OnShapeStationUsed -= OnShapeStationUsed;
        CleanStation.OnCleanStationUsed -= OnCleanStationUsed;
        ColorStation.OnColorStationUsed += OnColorStationUsed;

    }

    #region CALLBACKS

    private void OnShapeStationUsed() {
        AddShake(eScreenShakeIntensity.eLow, eScreenShakeDuration.eSmall, false);
    }
    private void OnCleanStationUsed() {
        AddShake(eScreenShakeIntensity.eVeryLow, eScreenShakeDuration.eSmall, true);
    }
    private void OnColorStationUsed() {
        AddShake(eScreenShakeIntensity.eVeryLow, eScreenShakeDuration.eSmall, true);
    }

    #endregion

    #region SHAKE
    public static void AddShakeRequest(float intensity, float duration, bool fadeOut = true)
    {
        Instance.AddShake(intensity, duration, fadeOut);
    }


    void AddShake(float intensity)
    {
        AddShake(intensity, eScreenShakeDuration.eSmall);
    }

    void AddShake(float intensity, float duration, bool fadeOut = true)
    {
        if (intensity < _currentIntensity && duration < _currentDuration)
        {
            //CustomLogger.Log("Ignoring Shake");
            return;
        }

        _sequence.Kill(false);
        _sequence = DOTween.Sequence();

        float diffIntensity = Mathf.Abs(intensity - _currentIntensity);
        float diffDuration = Mathf.Abs(duration - _currentDuration);

        _currentIntensity = Mathf.Max(_currentIntensity, intensity);
        _currentIntensity += diffIntensity * _overflowMultiplier;
        Tweener intensityTween = DOTween.To(() => _currentIntensity, x => _currentIntensity = x, 0f, _currentDuration);

        _currentDuration = Mathf.Max(_currentDuration, duration);
        _currentDuration += diffDuration * _overflowMultiplier;
        /*Tweener durationTween = */DOTween.To(() => _currentDuration, x => _currentDuration = x, 0f, _currentDuration);

        Tweener shakeTween = null;
        shakeTween = _camera.DOShakePosition(_currentDuration, _currentIntensity, 10, 90, fadeOut);


        _sequence.Append(shakeTween);
        _sequence.Join(intensityTween);
        _sequence.Join(shakeTween);
        _sequence.AppendCallback(ResetCameraPosition);
    }

    void ResetCameraPosition()
    {
        _camera.transform.position = _initialCameraPosition;
    }
    #endregion

    #region FREEZE

    IEnumerator AddFreeze(eFreezeDuration duration)
    {
        ++_freezeStack;
        if(_freezeStack == 1) 
            GameManager.AddFreeze(GameFreezeMask.FreezeContext.Pause);

        yield return new WaitForSecondsRealtime(GetFreezeCoroutine(duration));

        _freezeStack = Mathf.Max(0, _freezeStack - 1);
        if(_freezeStack == 0)
            GameManager.RemoveFreeze(GameFreezeMask.FreezeContext.Pause);
    }

    float GetFreezeCoroutine(eFreezeDuration duration)
    {
        switch (duration)
        {
            case eFreezeDuration.eSmall:
                return 0.035f;
            case eFreezeDuration.eMedium:
                return 0.055f;
            case eFreezeDuration.eBig:
                return 0.08f;
            case eFreezeDuration.eVeryBig:
                return 0.15f;
        }
        return 0f;
    }

    #endregion

}