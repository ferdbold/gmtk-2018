using UnityEngine;

public class IngredientKiller : MonoBehaviour {

    public void OnTriggerEnter(Collider other)
    {
        Ingredient ingredient = other.GetComponentInParent<Ingredient>();
        if (ingredient != null && ingredient._OnConveyorBelt)
        {
            GameObject.Destroy(other.gameObject);
        }
    }
}
