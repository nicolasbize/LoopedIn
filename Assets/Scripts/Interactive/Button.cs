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
    public bool isEnabled;

    public string buttonValue = "";
    public Transform keyMesh;
    public Material pressedMaterial;

    private bool isPressed = false;
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

    public void Enable() {
        isEnabled = true;
    }

    public override bool CanInteract() {
        return isEnabled && !isPressed;
    }

    public override void Interact() {
        OnButtonPress?.Invoke(this, new OnButtonPressEventArgs() {
            value = buttonValue
        });
        isPressed = true;
        StopHighlight();
        keyMesh.GetComponent<MeshRenderer>().material = pressedMaterial;
        timePressed = Time.timeSinceLevelLoad;
    }

    private void Update() {
        if (isPressed && (Time.timeSinceLevelLoad - timePressed > pressedDuration)) {
            isPressed = false;
            keyMesh.GetComponent<MeshRenderer>().material = unpressedMaterial;
        }
    }
}
