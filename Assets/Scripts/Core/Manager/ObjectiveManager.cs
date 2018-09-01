using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveManager : BaseManager<ObjectiveManager> {

    #region ATTRIBUTES

    public int _Score;

    public float _RecipeTickInterval;
    public float _RecipeTickRemaining;

    #endregion // ATTRIBUTES

    public override void OnStartGame()
    {
        _RecipeTickRemaining = _RecipeTickInterval;
    }

    public override void OnUpdateManager(float deltaTime)
    {
        _RecipeTickRemaining -= deltaTime;

        if (_RecipeTickRemaining < 0)
        {
            _RecipeTickRemaining = _RecipeTickInterval;
        }
    }
}
