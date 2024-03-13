using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : InteractiveObject {

    public bool isLocked = false;
    public BaseCodeMechanism lockPad;
    public GameLogic.GameStep unlockedStep;
    public AudioClip soundOpen;
    public AudioClip soundClose;
    public bool autoClose = false;
    public float autoCloseDuration = 2f;

    public enum State { Closed, Open}

    private State state = State.Closed;
    private Vector3 startRotation;
    private float timeSinceOpened = float.NegativeInfinity;

    public State GetState() {
        return state;
    }

    private new void Start() {
        base.Start();
        startRotation = transform.eulerAngles;
        if (lockPad != null ) {
            isLocked = true;
            lockPad.OnSuccessCodeEntered += LockPad_OnSuccessCodeEntered;
            lockPad.OnErrorCodeEntered += LockPad_OnErrorCodeEntered;
        }
    }

    private void LockPad_OnErrorCodeEntered(object sender, EventArgs e) {
        if (state == State.Closed) {
            isLocked = true;
        }
    }

    private void LockPad_OnSuccessCodeEntered(object sender, EventArgs e) {
        Unlock();
    }

    public void Unlock() {
        isLocked = false;
        Interact();
        if (unlockedStep != GameLogic.GameStep.None ) {
            GameLogic.Instance.SetStep(unlockedStep);
        }
    }

    public override void Interact() {
        state = (state == State.Open) ? State.Closed : State.Open;
        transform.eulerAngles = startRotation + (state == State.Open ? Vector3.up * 90 : Vector3.zero);
        GetComponent<AudioSource>().clip = state == State.Closed ? soundClose : soundOpen;
        GetComponent<AudioSource>().Play();
        if (state == State.Open) {
            timeSinceOpened = Time.timeSinceLevelLoad;
        }
    }

    private void Update() {
        if (state == State.Open && autoClose && (Time.timeSinceLevelLoad - timeSinceOpened > autoCloseDuration)) {
            Interact();
        }
    }

    public override bool CanInteract() {
        return !isLocked;
    }

    public override string ActionName() {
        return state == State.Open ? "close" : "open";
    }
}
