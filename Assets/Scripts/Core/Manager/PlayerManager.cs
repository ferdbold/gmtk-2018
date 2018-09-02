using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : BaseManager<PlayerManager> {

    [SerializeField] private Transform _player;
    [SerializeField] private Transform _inventorySocket;

    [Header("Movement")]
    [SerializeField] private AnimationCurve _movementCurve;
    [SerializeField] private float multiplier = 1f;
    [SerializeField] private float _speed = 1f;
    [SerializeField] private float _gravity = 1f;
    [SerializeField] private float maxSpeed = 3f;
    private float _currentVelocity = 0;

    #region LIFECYCLE
    public override void OnStartManager() 
    {
        _player.position = Vector3.zero;
        _player.rotation = Quaternion.identity;
        _currentVelocity = 0f;
    }

    public override void OnRegisterCallbacks() 
    { 

    }

    public override void OnUnregisterCallbacks() 
    {

    }


    public override void OnUpdateManager(float deltaTime) {
        if(ObjectiveManager.GameStarted) {
            HandleHorizontalMovement();
        }
    }
    #endregion

    #region Gameplay

    private void HandleHorizontalMovement() {
        float xAxis = 0f;
        if (Input.GetAxis("Horizontal") == 1) xAxis = 1;
        else if (Input.GetAxis("Horizontal") == -1) xAxis = -1;


        if (Mathf.Approximately(xAxis, 0f)) {
            if (Mathf.Abs(_currentVelocity) > 0) {
                _currentVelocity -= _gravity * Mathf.Sign(_currentVelocity);
                if (Mathf.Abs(_currentVelocity) < _gravity) _currentVelocity = 0f;
            }
        } else {
            _currentVelocity += Mathf.Sign(xAxis) * _speed * _movementCurve.Evaluate(Mathf.Abs(xAxis));
        }
        _currentVelocity = Mathf.Clamp(_currentVelocity, -maxSpeed, maxSpeed);

        OnPlayerMove();
    }

    private void OnPlayerMove()
    {       
        _player.Rotate(Vector3.up, _currentVelocity * Time.deltaTime * multiplier);
    }

    #endregion

    #region Accessors

    public static Vector3 Forward 
    {
        get 
        { 
            return Instance._player.forward;
        }
    }
    public static Vector3 Position
    {
        get
        {
            return Instance._player.position;
        }
    }
    public static Transform InventoryTransform 
    {
        get
        {
            return Instance._inventorySocket;
        }
    }

    #endregion

}
