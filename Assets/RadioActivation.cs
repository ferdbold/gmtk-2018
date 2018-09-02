using System;
using System.Collections.Generic;
using UnityEngine;

public class RadioActivation : MonoBehaviour {

    public static event Action<bool> OnRadioActivated;


    public LayerMask layer;
    public AudioSource source;

    private bool enabled = false;

    private bool CanBeActive = false;


    private void OnEnable() {
        ObjectiveManager.OnGameStarted += OnGameStarted;
        ObjectiveManager.OnGameEnded += OnGameEnded;
    }
    private void OnDisable() {
        ObjectiveManager.OnGameStarted -= OnGameStarted;
        ObjectiveManager.OnGameEnded -= OnGameEnded;
    }

    private void OnGameStarted() {
        CanBeActive = true;
    }
    private void OnGameEnded() {
        enabled = true;
        ToggleRadio();
        CanBeActive = false;

    }

    // Update is called once per frame
    void Update () {
        if (Input.GetMouseButtonDown(0)) {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100.0f, layer)) {
                if (hit.collider.gameObject == gameObject) {
                    ToggleRadio();
                }
            }
        }
    }

    void ToggleRadio() {
        enabled = !enabled;

        if(enabled && CanBeActive) {
            source.Play();
        } else {
            source.Stop();
        }

        if (OnRadioActivated != null) OnRadioActivated(enabled);
    }
}
