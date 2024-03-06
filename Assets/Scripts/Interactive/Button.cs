using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : InteractiveObject {

    public event EventHandler<OnButtonPressEventArgs> OnButtonPress;
    public class OnButtonPressEventArgs : EventArgs {
        public string value;
    }

    public string actionName;
    public bool isEnabled = true;
    public string buttonValue = "";
    public Transform keyMesh;
    public Material pressedMaterial;

    private float timePressed = float.NegativeInfinity;
    private float pressedDuration = 0.4f;
    private Material unpressedMaterial;

    private new void Start() {
        base.Start();
        unpressedMaterial = keyMesh.GetComponent<MeshRenderer>().materials[0];
    }

    public override string ActionName() {
        return actionName;
    }

    public override bool CanInteract() {
        return isEnabled;
    }

    public override void Interact() {
        OnButtonPress?.Invoke(this, new OnButtonPressEventArgs() {
            value = buttonValue
        });
        isEnabled = false;
        StopHighlight();
        keyMesh.GetComponent<MeshRenderer>().material = pressedMaterial;
        timePressed = Time.timeSinceLevelLoad;
    }

    private void Update() {
        if (!isEnabled && (Time.timeSinceLevelLoad - timePressed > pressedDuration)) {
            isEnabled = true;
            keyMesh.GetComponent<MeshRenderer>().material = unpressedMaterial;
        }
    }
}
