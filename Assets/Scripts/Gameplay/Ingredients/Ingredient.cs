using UnityEngine;

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

    private Transform _visuals;
    private MeshRenderer _meshRenderer;

    private float _currentLenghtChange = 0f;
    private float _lenghtChangeMax = 0.25f;

    private float _currentRugosityChange = 0f;
    private float _rugosityChangeMax = 0.4f;

    private float _currentTemperatureChange = 0f;
    private float _temperatureChangeMax = 0.5f;

    private Color _currentColorOverride = Color.white;
    private bool _colorChanged = false;

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
        _meshRenderer = GetComponentInChildren<MeshRenderer>();

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

        _visuals.localScale = new Vector3(  1f + Mathf.Lerp(0, Mathf.Sign(_currentLenghtChange) * 0.40f, Mathf.Abs(_currentLenghtChange) / _lenghtChangeMax),
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

        _meshRenderer.material.color = _currentColorOverride;
    }
    public void ResetColor() {
        _colorChanged = false;

        _meshRenderer.material.color = _color;
    }

    public void AddHeat(float change) {
        _currentTemperatureChange += change;
        _currentTemperatureChange = Mathf.Clamp(_currentTemperatureChange, -_temperatureChangeMax, _temperatureChangeMax);

        //TODO VISUAL
    }
    public void ResetHeat() {
        _currentTemperatureChange = 0f;

    }

    public void AddGloss(float change) {
        _currentRugosityChange -= change;
        _currentRugosityChange = Mathf.Clamp(_currentRugosityChange, -_rugosityChangeMax, _rugosityChangeMax);

        //TODO VISUAL
        _meshRenderer.material.SetFloat("_Metallic", 1f-Rugosity);
        _meshRenderer.material.SetFloat("_Glossiness", 1f-Rugosity);
    }
    public void ResetGloss() {
        _currentRugosityChange = 0f;

        _meshRenderer.material.SetFloat("_Metallic", 1f - Rugosity);
        _meshRenderer.material.SetFloat("_Glossiness", 1f - Rugosity);
    }

    #endregion
}
