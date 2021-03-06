﻿using UnityEngine;

public class Ingredient : MonoBehaviour {

    #region ATTRIBUTES

    #region TRAITS

    [Header("Color")]
    [SerializeField] private Color _color;

    [Range(0, 1)]
    public float _colorWeight = 0.5f;
    
    [Header("Solidity")]
    [Range(0, 1)]
    [SerializeField]  private float _rugosity;

    [Range(0, 1)]
    public float _rugosityWeight = 0.5f;

    [Header("Length")]
    [Range(0, 1)]
    [SerializeField] private float _length;

    [Range(0, 1)]
    public float _lengthWeight = 0.5f;

    [Header("Temperature")]
    [Range(0, 1)]
    [SerializeField] private float _temperature;

    [Range(0, 1)]
    public float _temperatureWeight = 0.5f;

    #endregion // TRAITS

    public float _ConveyorSpeed;
    [HideInInspector] public bool _OnConveyorBelt = false;
    public bool _CanBeGrabbed = true;

    private Transform _visuals;
    private MeshRenderer[] _meshRenderer;

    private float _currentLenghtChange = 0f;
    private float _lenghtChangeMax = 0.30f;

    private float _currentRugosityChange = 0f;
    private float _rugosityChangeMax = 0.4f;

    private float _currentTemperatureChange = 0f;
    private float _temperatureChangeMax = 0.5f;

    private Color _currentColorOverride = Color.white;
    private bool _colorChanged = false;

    private float _maxMetallic = 0.5f;
    private float _maxGloss = 0.75f;


    #endregion // ATTRIBUTES

    #region Getters 

    public float Temperature {
        get { return Mathf.Clamp(_temperature + _currentTemperatureChange, 0f, 1f); }
    }
    public float Lenght
    {
        get { return Mathf.Clamp(_length + _currentLenghtChange, 0f, 1f); }
    }
    public float Rugosity
    {
        get { return Mathf.Clamp(_rugosity + _currentRugosityChange, 0f, 1f); }
    }
    public Color Color
    {
        get { return _colorChanged ? _currentColorOverride : _color; }
    }

    private Texture[] _albedo;
    private Texture[] _metallics;

    #endregion

    public class SComparisonScore
    {
        public float _colorScore = 0f;
        public float _colorWeight = 0f;

        public float _solidityScore = 0f;
        public float _solidityWeight = 0f;

        public float _lengthScore = 0f;
        public float _lengthWeight = 0f;

        public float _temperatureScore = 0f;
        public float _temperatureWeight = 0f;

        public float _globalScore = 0f;
    }

    public void Awake() {
        foreach(Transform t in gameObject.GetComponentsInChildren<Transform>())
            t.gameObject.layer = 9; //Ingredient layer

        _visuals = transform.Find("Visuals");
        _meshRenderer = GetComponentsInChildren<MeshRenderer>();
        _albedo = new Texture[_meshRenderer.Length];
        _metallics = new Texture[_meshRenderer.Length];
        for (int i =0; i < _meshRenderer.Length; ++i) {
            _albedo[i] = _meshRenderer[i].material.GetTexture("_MainTex");
            _metallics[i] = _meshRenderer[i].material.GetTexture("_MetallicGlossMap");
        }

        ResetColor();
        ResetHeat();
        ResetGloss();
        ResetLenght();
    }

    public void Update()
    {
        if (_OnConveyorBelt)
        {
            transform.Translate(transform.forward * _ConveyorSpeed * Time.deltaTime);
        }
    }

    public SComparisonScore Compare(Ingredient other)
    {
        float totalWeight = _colorWeight + _rugosityWeight + _lengthWeight + _temperatureWeight;

        float h = 0f, s = 0f, v = 0f;
        Color.RGBToHSV(_color, out h, out s, out v);
        float otherH = 0f, otherS = 0f, otherV = 0f;
        Color.RGBToHSV(other._color, out otherH, out otherS, out otherV);

        SComparisonScore score = new SComparisonScore
        {
            _colorScore = GetScore(h, otherH),
            _colorWeight = _colorWeight / totalWeight,

            _solidityScore = GetScore(_rugosity, other._rugosity),
            _solidityWeight = _rugosityWeight / totalWeight,

            _lengthScore = GetScore(_length, other._length),
            _lengthWeight = _lengthWeight / totalWeight,

            _temperatureScore = GetScore(_temperature, other._temperature),
            _temperatureWeight = _temperatureWeight / totalWeight
        };

        score._globalScore = 
              score._colorScore * score._colorWeight
            + score._solidityScore * score._solidityWeight
            + score._lengthScore * score._lengthWeight
            + score._temperatureScore * score._temperatureWeight;

        return score;
    }

    private float GetScore(float myStat, float otherStat)
    {
        return 1 - Mathf.Abs(myStat - otherStat);
    }

    #region GAMEPLAY
    public void ChangeLenght(float change) {
        _currentLenghtChange += change;
        _currentLenghtChange = Mathf.Clamp(_currentLenghtChange, -_lenghtChangeMax, _lenghtChangeMax);

        _visuals.localScale = new Vector3(  1f + Mathf.Lerp(0, Mathf.Sign(_currentLenghtChange) * 0.5f, Mathf.Abs(_currentLenghtChange) / _lenghtChangeMax),
                                            1f - Mathf.Lerp(0, Mathf.Sign(_currentLenghtChange) * 0.25f, Mathf.Abs(_currentLenghtChange) / _lenghtChangeMax),
                                            1f - Mathf.Lerp(0, Mathf.Sign(_currentLenghtChange) * 0.25f, Mathf.Abs(_currentLenghtChange) / _lenghtChangeMax));
    }
    public void ResetLenght() {
        _currentLenghtChange = 0f;
        _visuals.localScale = new Vector3(1f,1f,1f);
    }

    public void ChangeColor(Color color) {
        _colorChanged = true;
        _currentColorOverride = color;

        for (int i = 0; i < _meshRenderer.Length; ++i) {
            _meshRenderer[i].material.color = _currentColorOverride;
            _meshRenderer[i].material.SetTexture("_MainTex", null);
            _meshRenderer[i].material.SetTexture("_MetallicGlossMap", null);
            _meshRenderer[i].material.DisableKeyword("_METALLICGLOSSMAP");
        }

    }
    public void ResetColor() {
        _colorChanged = false;

        for (int i = 0; i < _meshRenderer.Length; ++i) {
            _meshRenderer[i].material.color = _color;
            _meshRenderer[i].material.SetTexture("_MainTex", _albedo[i]);
            _meshRenderer[i].material.SetTexture("_MetallicGlossMap", _metallics[i]);
            _meshRenderer[i].material.EnableKeyword("_METALLICGLOSSMAP");

        }
    }

    public void AddHeat(float change) {
        _currentTemperatureChange += change;
        _currentTemperatureChange = Mathf.Clamp(_currentTemperatureChange, -_temperatureChangeMax, _temperatureChangeMax);

        _currentRugosityChange += change;
        _currentRugosityChange = Mathf.Clamp(_currentRugosityChange, -_rugosityChangeMax, _rugosityChangeMax);

        //TODO VISUAL BURN
        for (int i = 0; i < _meshRenderer.Length; ++i) {
            _meshRenderer[i].material.SetFloat("_Metallic", _maxMetallic - (Rugosity * _maxMetallic));
            _meshRenderer[i].material.SetFloat("_Glossiness", _maxGloss - (Rugosity * _maxGloss));        
        }

        Debug.Log("Rugosity " + Rugosity);

    }
    public void ResetHeat() {
        _currentTemperatureChange = 0f;

    }

    public void AddGloss(float change) {
        _currentRugosityChange -= change;
        _currentRugosityChange = Mathf.Clamp(_currentRugosityChange, -_rugosityChangeMax, _rugosityChangeMax);

        for (int i = 0; i < _meshRenderer.Length; ++i) {
            _meshRenderer[i].material.SetFloat("_Metallic", _maxMetallic - (Rugosity * _maxMetallic));
            _meshRenderer[i].material.SetFloat("_Glossiness", _maxGloss - (Rugosity * _maxGloss));
        }
        Debug.Log("Rugosity " + Rugosity);
    }
    public void ResetGloss() {
        _currentRugosityChange = 0f;

        for (int i = 0; i < _meshRenderer.Length; ++i) {
            _meshRenderer[i].material.SetFloat("_Metallic", _maxMetallic - (Rugosity * _maxMetallic));
            _meshRenderer[i].material.SetFloat("_Glossiness", _maxGloss - (Rugosity * _maxGloss));
        }
    }

    #endregion
}
