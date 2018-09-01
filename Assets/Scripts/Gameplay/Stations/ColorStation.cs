using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorStation : WorkStation {

    public static event Action OnColorStationUsed;

    [Serializable]
    private class SliderData {
        public Animator anim;
        [HideInInspector] public Vector3 start;
        [HideInInspector] public Vector3 end;
        [HideInInspector] public float value = 0;
    }

    [SerializeField] private Renderer _rendererToColor;

    [SerializeField] private SliderData _sliderRed;
    [SerializeField] private SliderData _sliderGreen;
    [SerializeField] private SliderData _sliderBlue;


    private bool _stationLocked = false;
    private bool _firstUpdate = true;
    private int _animationBlendParam;
    private Vector3 _animationTranslation = new Vector3(0,0.225f,0);
    private SliderData _grabbedSlider = null;
    private Color _dipColor;

    public override void Setup() {
        base.Setup();

        _sliderRed.start = _sliderRed.anim.transform.position;
        _sliderGreen.start = _sliderGreen.anim.transform.position;
        _sliderBlue.start = _sliderBlue.anim.transform.position;
        _sliderRed.end = _sliderRed.start + (_sliderRed.anim.transform.rotation * _animationTranslation);
        _sliderGreen.end = _sliderGreen.start + (_sliderRed.anim.transform.rotation * _animationTranslation);
        _sliderBlue.end = _sliderBlue.start + (_sliderRed.anim.transform.rotation * _animationTranslation);

        _animationBlendParam = Animator.StringToHash("Blend");

        SetBlend(_sliderRed, UnityEngine.Random.Range(0f,1f));
        SetBlend(_sliderGreen, UnityEngine.Random.Range(0f, 1f));
        SetBlend(_sliderBlue, UnityEngine.Random.Range(0f, 1f));

        Debug.Log("start and pos red: " + _sliderRed.start + "  " + _sliderRed.end);
    }

    public override void UpdateStation(float deltaTime) {
        base.UpdateStation(deltaTime);

        //UpdateColor();


        if (_grabbedSlider == null) {
            if(Input.GetMouseButtonDown(0)) {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit, 100.0f, WorkStationManager.WorkstationLayermask)) {
                    Animator animator = hit.collider.gameObject.GetComponent<Animator>();
                    if(animator != null) {
                        if(animator == _sliderRed.anim)
                            _grabbedSlider = _sliderRed;
                        else if (animator == _sliderGreen.anim)
                            _grabbedSlider = _sliderGreen;
                        else if (animator == _sliderBlue.anim)
                            _grabbedSlider = _sliderBlue;
                    }
                }
            }
        } else {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Plane sliderPlane = new Plane(_grabbedSlider.anim.transform.forward, _grabbedSlider.start);
            float distance = 0;
            if(sliderPlane.Raycast(ray, out distance)) {
                Vector3 collisionPoint = ray.GetPoint(distance);
                Vector3 fromTo = _grabbedSlider.end - _grabbedSlider.start;

                Vector3 proj = Vector3.Project(collisionPoint, _grabbedSlider.anim.transform.up);
                Vector3 fromProj = Vector3.Project(_grabbedSlider.start, _grabbedSlider.anim.transform.up);
                Vector3 fromToProj = (proj - fromProj);

                float lenght = fromToProj.magnitude / fromTo.magnitude;
                if (Vector3.Dot(fromToProj, fromTo) < 0)
                    lenght = 0;


                SetBlend(_grabbedSlider,lenght);
            }

            if (Input.GetMouseButtonUp(0)) {
                _grabbedSlider = null;
            }        
        }
    }

    public override void UseStation() {
        base.UseStation();

        if (OnColorStationUsed != null) OnColorStationUsed();
    }

    private void SetBlend(SliderData slider, float blend) {
        slider.value = blend;
        slider.anim.SetFloat(_animationBlendParam, blend);

        _dipColor = new Color(_sliderRed.value, _sliderGreen.value, _sliderBlue.value);
        _rendererToColor.material.color = _dipColor;
    }

    public void UpdateColor() {
        _dipColor = new Color(_sliderRed.value, _sliderGreen.value, _sliderBlue.value);
        _rendererToColor.material.color = _dipColor;
    }
}