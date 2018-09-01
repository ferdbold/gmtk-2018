using UnityEngine;

public class Ingredient : MonoBehaviour {

    #region ATTRIBUTES

    #region TRAITS

    [Header("Color")]

    public Color _color;

    [Range(0, 1)]
    public float _colorWeight = 0.5f;
    
    [Header("Solidity")]

    [Range(0, 1)]
    public float _solidity;

    [Range(0, 1)]
    public float _solidityWeight = 0.5f;

    [Header("Length")]

    [Range(0, 1)]
    public float _length;

    [Range(0, 1)]
    public float _lengthWeight = 0.5f;

    [Header("Temperature")]

    [Range(0, 1)]
    public float _temperature;

    [Range(0, 1)]
    public float _temperatureWeight = 0.5f;

    #endregion // TRAITS

    public float _ConveyorSpeed;
    public bool _OnConveyorBelt = true;

    #endregion // ATTRIBUTES

    public struct SComparisonScore
    {
        public float _colorScore;
        public float _colorWeight;

        public float _solidityScore;
        public float _solidityWeight;

        public float _lengthScore;
        public float _lengthWeight;

        public float _temperatureScore;
        public float _temperatureWeight;

        public float _globalScore;
    }

    public void Awake() {
        gameObject.layer = 9;
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
        float totalWeight = _colorWeight + _solidityWeight + _lengthWeight + _temperatureWeight;

        float h = 0f, s = 0f, v = 0f;
        Color.RGBToHSV(_color, out h, out s, out v);
        float otherH = 0f, otherS = 0f, otherV = 0f;
        Color.RGBToHSV(other._color, out otherH, out otherS, out otherV);

        SComparisonScore score;
        score._colorScore = GetScore(h, otherH);
        score._colorWeight = _colorWeight / totalWeight;

        score._solidityScore = GetScore(_solidity, other._solidity);
        score._solidityWeight = _solidityWeight / totalWeight;

        score._lengthScore = GetScore(_length, other._length);
        score._lengthWeight = _lengthWeight / totalWeight;

        score._temperatureScore = GetScore(_temperature, other._temperature);
        score._temperatureWeight = _temperatureWeight / totalWeight;

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
}
