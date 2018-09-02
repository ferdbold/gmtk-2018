using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PreparationStation : WorkStation {

    #region ATTRIBUTES

    [SerializeField] private LayerMask _shipBellLayer;
    [SerializeField] private GameObject _platePrefab;

    public List<Transform> _preppedAnchors;
    public Transform _shippedAnchor;
    public Transform _shipBell;

    private static int _index = 1;
    private GameObject _plateInstance = null;

    #endregion // ATTRIBUTES

    public static event Action OnPreparationStationUsed;

    public static List<Ingredient> _LaborOfLove = new List<Ingredient>();

    public void OnEnable() {
        ObjectiveManager.OnRecipeShipped += OnRecipeShipped;
    }
    public void OnDisable() {
        ObjectiveManager.OnRecipeShipped -= OnRecipeShipped;
    }

    public override void Setup() {
        base.Setup();

        _plateInstance = (GameObject)Instantiate(_platePrefab, _preppedAnchors[0]);
    }

    public override void UpdateStation(float deltaTime)
    {
        base.UpdateStation(deltaTime);

        // Ship bell input
        if (Input.GetMouseButtonDown(0))
        {
            if(WorkStationManager.IsStationSelected(this) && _index > 1)
            {
                // Prevent it if we're already in an interlude
                if (!ObjectiveManager.Instance.IsInterlude())
                {
                    RaycastHit hit;
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out hit, 100.0f, _shipBellLayer))
                    {
                        if (hit.collider.transform.Equals(_shipBell))
                        {
                            ObjectiveManager.Instance.ShipRecipe();
                            _shipBell.DOShakeRotation(1f, 35, 10);
                        }
                    }
                }
            }
        }
    }

    public override void UseStation(Ingredient ingredient) {
        base.UseStation(ingredient);

        ingredient._CanBeGrabbed = false;
        _LaborOfLove.Add(ingredient);

        ingredient.transform.parent = _preppedAnchors[_index];
        ingredient.transform.DOLocalMove(Vector3.zero, InventoryManager.Instance._objectAnimationTime).SetEase(InventoryManager.Instance._objectAnimationCurve);
        ingredient.transform.DOLocalRotate(Quaternion.identity.eulerAngles, InventoryManager.Instance._objectAnimationTime).SetEase(InventoryManager.Instance._objectAnimationCurve);

        ++_index;
        _index = Mathf.Clamp(_index, 1, _preppedAnchors.Count - 1);

        if (OnPreparationStationUsed != null) OnPreparationStationUsed();
    }

    public void OnRecipeShipped(ObjectiveManager.SRecipeScore recipeScore)
    {
        _index = 1;
        // Ship animation
        foreach (Ingredient ingredient in _LaborOfLove)
        {
            MoveIngredientToShipping(ingredient);
        }
        MoveIngredientToShipping(_plateInstance.GetComponent<Ingredient>());

        _LaborOfLove.Clear();

        _plateInstance = (GameObject)Instantiate(_platePrefab, _preppedAnchors[0]);
    }

    private void MoveIngredientToShipping(Ingredient ingredient) {
        Vector3 locPos = ingredient.transform.localPosition + ingredient.transform.parent.localPosition;
        Vector3 locRot = ingredient.transform.localRotation.eulerAngles + ingredient.transform.parent.localRotation.eulerAngles;

        ingredient.transform.parent = _shippedAnchor;
        ingredient.transform.DOLocalMove(locPos, InventoryManager.Instance._objectAnimationTime).SetEase(InventoryManager.Instance._objectAnimationCurve);
        ingredient.transform.DOLocalRotate(locRot, InventoryManager.Instance._objectAnimationTime).SetEase(InventoryManager.Instance._objectAnimationCurve);
        ingredient._OnConveyorBelt = true;
    }

    public static int AmtIngredientsPlaced() {
        return _index - 1;
    }
}
