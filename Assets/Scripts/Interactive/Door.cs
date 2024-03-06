using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : InteractiveObject {

    public bool isLocked = false;
    public Keycode lockPad;

    private bool isOpened = false;
    private Vector3 startRotation;

    private new void Start() {
        base.Start();
        startRotation = transform.eulerAngles;
        if (lockPad != null ) {
            lockPad.OnSuccessCodeEntered += LockPad_OnSuccessCodeEntered;
        }
    }

    private void LockPad_OnSuccessCodeEntered(object sender, EventArgs e) {
        Unlock();
    }

    public void Unlock() {
        isLocked = false;
        Interact();
    }

    public override void Interact() {
        isOpened = !isOpened;
        transform.eulerAngles = startRotation + (isOpened ? Vector3.up * 90 : Vector3.zero);
    }

    public override bool CanInteract() {
        return !isLocked;
    }

    public override string ActionName() {
        return isOpened ? "close" : "open";
    }
}
