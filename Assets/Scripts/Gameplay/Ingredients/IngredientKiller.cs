using UnityEngine;

public class IngredientKiller : MonoBehaviour {

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("CALISSE");

        Ingredient ingredient = other.GetComponent<Ingredient>();
        if (ingredient != null && ingredient._OnConveyorBelt)
        {
            GameObject.Destroy(other.gameObject);
        }
    }
}
