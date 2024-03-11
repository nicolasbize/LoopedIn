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
    public float pressedDuration = 0.4f;
    public string buttonValue = "";
    public Transform keyMesh;
    public Material pressedMaterial;
    public Material enabledMaterial;

    private bool isPressed = false;
    private float timePressed = float.NegativeInfinity;
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
        if (enabledMaterial != null) {
            keyMesh.GetComponent<MeshRenderer>().material = enabledMaterial;
        }
    }

    public void Disable() {
        isEnabled = false;
        keyMesh.GetComponent<MeshRenderer>().material = unpressedMaterial;
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
            Material material = enabledMaterial == null ? unpressedMaterial : enabledMaterial;
            keyMesh.GetComponent<MeshRenderer>().material = material;
        }
    }
}
