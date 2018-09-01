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
            Ingredient ingredient = _IngredientPrefabs[Random.Range(0, _IngredientPrefabs.Count)];
            GameObject.Instantiate(ingredient, transform.position, transform.rotation);

            _TimeToSpawn = _SpawnInterval;
        }
    }
}
