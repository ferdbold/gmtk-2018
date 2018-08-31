using UnityEngine;

public class Ingredient : MonoBehaviour {

    public Color _color;

    [Range(0, 1)]
    public float _solidity;

    [Range(0, 1)]
    public float _length;

    [Range(0, 1)]
    public float _rugosity;
}
