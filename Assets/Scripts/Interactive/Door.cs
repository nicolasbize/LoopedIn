using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : InteractiveObject {

    private bool isOpened = false;
    private Vector3 startRotation;

    private new void Start() {
        base.Start();
        startRotation = transform.eulerAngles;
    }

    public override void Interact() {
        isOpened = !isOpened;
        transform.eulerAngles = startRotation + (isOpened ? Vector3.up * 90 : Vector3.zero);
    }

}
