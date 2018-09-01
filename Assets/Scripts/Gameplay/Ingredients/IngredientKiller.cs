using UnityEngine;

public class IngredientKiller : MonoBehaviour {

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collided");

        if (other.gameObject.GetComponent<Ingredient>() != null)
        {
            GameObject.Destroy(other.gameObject);
        }
    }
}
