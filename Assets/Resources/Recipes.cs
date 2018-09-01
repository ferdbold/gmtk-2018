using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Recipes", menuName = "Data/Recipes", order = 1)]
public class Recipes : ScriptableObject {

    [Serializable]
    public struct SRecipe
    {
        public string _Name;
        public List<Ingredient> _Ingredients;
    }

    [SerializeField]
    public List<SRecipe> _Recipes;
}
