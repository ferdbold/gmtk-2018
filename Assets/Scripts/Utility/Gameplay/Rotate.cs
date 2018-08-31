using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class Rotate : MonoBehaviour
{

    public enum eRotationType
    {
        constant = 0
        , oscillation = 1
    }

    public eRotationType rotationType = eRotationType.constant;
    [Header("Rotation Type : ALL")]
    public Vector2 RangeSpeed;
    public Vector2 RangeRotationX;
    public Vector2 RangeRotationY;
    public Vector2 RangeRotationZ;
    [Header("Rotation Type : Constant")]
    public bool applyRandomDirection = true;
    [Header("Rotation Type : Oscillation")]
    public float oscillationTime;
    public AnimationCurve oscillationCurve;

    private WaitForSeconds OscillationWaitCoroutine;
    private Coroutine RotateObjectConstantCoroutine = null;
    private Coroutine RotateObjectOscillationCoroutine = null;

    [HideInInspector]
    public float speed;
    public bool IsRunning { get; private set; }
    private Transform _transform;
    private Vector3 _startLocalRotation;
    private bool _isInit = false;

    private void Awake()
    {
        if (!_isInit)
            Init();
    }

    private void Init()
    {
        _isInit = true;

        _transform = transform;
        _startLocalRotation = _transform.localRotation.eulerAngles;

        OscillationWaitCoroutine = new WaitForSeconds(oscillationTime);
    }

    void OnEnable()
    {
        switch (rotationType)
        {
            case eRotationType.constant:
                RotateObjectConstantCoroutine = StartCoroutine(RotateObjectConstant());
                break;
            case eRotationType.oscillation:
                RotateObjectOscillationCoroutine = StartCoroutine(RotateObjectOscillation());
                break;
            default:
                break;
        }
    }
    void OnDisable()
    {
        switch (rotationType)
        {
            case eRotationType.constant:
                if (RotateObjectConstantCoroutine != null) StopCoroutine(RotateObjectConstantCoroutine);
                break;
            case eRotationType.oscillation:
                if (RotateObjectOscillationCoroutine != null) StopCoroutine(RotateObjectOscillationCoroutine);
                break;
            default:
                break;
        }
    }

    public void ResetRotation()
    {
        if (!_isInit)
            Init();

        StopAllCoroutines();
        switch (rotationType)
        {
            case eRotationType.constant:
                RotateObjectConstantCoroutine = StartCoroutine(RotateObjectConstant());
                break;
            case eRotationType.oscillation:
                RotateObjectOscillationCoroutine = StartCoroutine(RotateObjectOscillation());
                break;
            default:
                break;
        }
    }

    public void StopRotation()
    {
        StopAllCoroutines();
        IsRunning = false;
    }

    IEnumerator RotateObjectConstant()
    {
        IsRunning = true;
        speed = Random.Range(RangeSpeed.x, RangeSpeed.y);
        if (applyRandomDirection && Random.Range(0f, 1f) > 0.5f) speed *= -1f;
        Vector3 rotation = new Vector3(Random.Range(RangeRotationX.x, RangeRotationX.y),
            Random.Range(RangeRotationY.x, RangeRotationY.y),
            Random.Range(RangeRotationZ.x, RangeRotationZ.y));
        while (true)
        {
            transform.Rotate(rotation * speed * Time.deltaTime);
            yield return null;
        }
    }

    IEnumerator RotateObjectOscillation()
    {
        IsRunning = true;
        Vector3 startRotation = _startLocalRotation + new Vector3(RangeRotationX.x, RangeRotationY.x, RangeRotationZ.x);
        Vector3 endRotation = _startLocalRotation + new Vector3(RangeRotationX.y, RangeRotationY.y, RangeRotationZ.y);
        _transform.localEulerAngles = startRotation;

        while (true)
        {
            Tween t = _transform.DOLocalRotate(endRotation, oscillationTime);
            t.SetEase(oscillationCurve);
            yield return OscillationWaitCoroutine;

            t = _transform.DOLocalRotate(startRotation, oscillationTime);
            t.SetEase(oscillationCurve);
            yield return OscillationWaitCoroutine;
        }
    }
}