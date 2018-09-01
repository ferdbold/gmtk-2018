using UnityEngine;

public class IngredientComparer : MonoBehaviour {

    public Ingredient _ingredientA;
    public Ingredient _ingredientB;

    public void Start()
    {
        Ingredient.SComparisonScore score = _ingredientA.Compare(_ingredientB); 

        Debug.Log("Comparing " + _ingredientA.name + " with " + _ingredientB.name + "\n"
            + "==========================\n"
            + "COLOR            : " + score._colorScore + " (Weight: " + score._colorWeight + ")\n"
            + "SOLIDITY        : " + score._solidityScore + " (Weight: " + score._solidityWeight + ")\n"
            + "LENGTH          : " + score._lengthScore + " (Weight: " + score._lengthWeight + ")\n"
            + "TEMPERATURE : " + score._temperatureScore + " (Weight: " + score._temperatureWeight + ")\n"
            + "==========================\n"
            + "GLOBAL          : " + score._globalScore
        );
    }
}
