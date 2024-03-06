using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keycode : MonoBehaviour
{

    public event EventHandler OnSuccessCodeEntered;
    public event EventHandler OnErrorCodeEntered;

    public string passcode;
    public Transform keys;
    public Transform keycodeMesh;
    public Material successColorMaterial;
    public Material failureColorMaterial;
    public GameLogic.GameStep stepAfterUnlock;

    private string currentSequence;
    private Material originalMaterial;

    private int nbPulsesLeft;
    private float timeSincePulse = float.NegativeInfinity;
    private float pulseDuration = 1.0f;
    private Material pulseMaterial;

    private void Start() {
        foreach (Transform child in keys) {
            Button button = child.GetComponent<Button>();
            button.OnButtonPress += OnButtonPress;
        }
        originalMaterial = keycodeMesh.GetComponent<MeshRenderer>().material;
    }

    private void Update() {
        if (nbPulsesLeft > 0 && (Time.timeSinceLevelLoad - timeSincePulse > pulseDuration)) {
            nbPulsesLeft -= 1;
            timeSincePulse = Time.timeSinceLevelLoad;
            if (keycodeMesh.GetComponent<MeshRenderer>().material == originalMaterial) {
                keycodeMesh.GetComponent<MeshRenderer>().material = pulseMaterial;
            } else {
                keycodeMesh.GetComponent<MeshRenderer>().material = originalMaterial;
            }
        }
    }

    private void OnButtonPress(object sender, Button.OnButtonPressEventArgs e) {
        if (e.value == "reset") {
            currentSequence = "";
        } else if (e.value == "enter" && currentSequence.Length > 0) {
            if (currentSequence == passcode) {
                OnSuccessCodeEntered?.Invoke(this, EventArgs.Empty);
                nbPulsesLeft = 2;
                pulseDuration = 1f;
                pulseMaterial = successColorMaterial;
                if (stepAfterUnlock != GameLogic.GameStep.None) {
                    GameLogic.Instance.Step = stepAfterUnlock;
                }
            } else {
                OnErrorCodeEntered?.Invoke(this, EventArgs.Empty);
                nbPulsesLeft = 6;
                pulseDuration = 0.2f;
                pulseMaterial = failureColorMaterial;
            }
            currentSequence = "";
        } else {
            currentSequence += e.value;
        }
    }
}
