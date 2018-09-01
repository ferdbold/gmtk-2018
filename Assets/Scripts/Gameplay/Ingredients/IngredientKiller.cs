using UnityEngine;

public class IngredientKiller : MonoBehaviour {

    private BoxCollider _BoxCollider;

    void Start()
    {
        _BoxCollider = GetComponent<BoxCollider>();
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collided");

        if (other.gameObject.GetComponent<Ingredient>() != null)
        {
            GameObject.Destroy(other.gameObject);
        }
    }
}
