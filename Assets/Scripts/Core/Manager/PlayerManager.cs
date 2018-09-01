using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : BaseManager<PlayerManager> {

    [SerializeField] private Transform _player;
    [SerializeField] private AnimationCurve _movementCurve;

    #region LIFECYCLE
    public override void OnStartManager() 
    {
        _player.position = Vector3.zero;
        _player.rotation = Quaternion.identity;
    }

    public override void OnRegisterCallbacks() 
    { 

    }

    public override void OnUnregisterCallbacks() 
    {

    }


    public override void OnUpdateManager(float deltaTime)
    {
        float xAxis = Input.GetAxis("Horizontal");
        OnPlayerMove(Mathf.Sign(xAxis) * _movementCurve.Evaluate(Mathf.Abs(xAxis)));
    }
    #endregion


    #region 

    private void OnPlayerMove(float direction)
    {
        _player.Rotate(Vector3.up, direction);
    }

    #endregion

}
