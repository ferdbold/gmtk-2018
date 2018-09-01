using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PreparationStation : WorkStation {

    #region ATTRIBUTES

    [SerializeField] private LayerMask _shipBellLayer;

    public Transform _preppedAnchor;
    public Transform _shippedAnchor;
    public Transform _shipBell;

    #endregion // ATTRIBUTES

    public static event Action OnPreparationStationUsed;

    public static List<Ingredient> _LaborOfLove = new List<Ingredient>();

    public void Start()
    {
        ObjectiveManager.OnRecipeShipped += OnRecipeShipped;
    }

    public override void UpdateStation(float deltaTime)
    {
        base.UpdateStation(deltaTime);

        // Ship bell input
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100.0f, _shipBellLayer))
            {
                if (hit.collider.transform.Equals(_shipBell))
                {
                    ObjectiveManager.Instance.ShipRecipe();
                }
            }
        }
    }

    public override void UseStation(Ingredient ingredient) {
        base.UseStation(ingredient);

        _LaborOfLove.Add(ingredient);

        if (OnPreparationStationUsed != null) OnPreparationStationUsed();

        ingredient.transform.parent = _preppedAnchor;
        ingredient.transform.DOLocalMove(Vector3.zero, InventoryManager.Instance._objectAnimationTime).SetEase(InventoryManager.Instance._objectAnimationCurve);
        ingredient.transform.DOLocalRotate(Quaternion.identity.eulerAngles, InventoryManager.Instance._objectAnimationTime).SetEase(InventoryManager.Instance._objectAnimationCurve);
    }

    public void OnRecipeShipped(ObjectiveManager.SRecipeScore recipeScore)
    {
        // Ship animation
        foreach(Ingredient ingredient in _LaborOfLove)
        {
            ingredient.transform.parent = _shippedAnchor;
            ingredient.transform.DOLocalMove(Vector3.zero, InventoryManager.Instance._objectAnimationTime).SetEase(InventoryManager.Instance._objectAnimationCurve);
            ingredient.transform.DOLocalRotate(Quaternion.identity.eulerAngles, InventoryManager.Instance._objectAnimationTime).SetEase(InventoryManager.Instance._objectAnimationCurve);
            ingredient._OnConveyorBelt = true;
        }

        _LaborOfLove.Clear();
    }
}
