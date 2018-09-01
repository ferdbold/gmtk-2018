using System.Collections.Generic;
using UnityEngine;

public class IngredientSpawner : MonoBehaviour {

    #region ATTRIBUTES

    public List<Ingredient> _IngredientPrefabs;

    public float _SpawnInterval = 3f;

    #endregion

    private float _TimeToSpawn;

    public void Start()
    {
        _TimeToSpawn = _SpawnInterval;
    }

    public void Update()
    {
        _TimeToSpawn -= Time.deltaTime;
        if (_TimeToSpawn < 0)
        {
            Ingredient ingredientPrefab = _IngredientPrefabs[Random.Range(0, _IngredientPrefabs.Count)];
            Ingredient ingredient = GameObject.Instantiate(ingredientPrefab, transform.position, transform.rotation);
            ingredient._OnConveyorBelt = true;

            _TimeToSpawn = _SpawnInterval;
        }
    }
}
