using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : InteractiveObject {

    public bool isLocked = false;
    public Keycode lockPad;
    public GameLogic.GameStep unlockedStep;

    public enum State { Closed, Open}

    private State state = State.Closed;
    private Vector3 startRotation;

    public State GetState() {
        return state;
    }

    private new void Start() {
        base.Start();
        startRotation = transform.eulerAngles;
        if (lockPad != null ) {
            isLocked = true;
            lockPad.OnSuccessCodeEntered += LockPad_OnSuccessCodeEntered;
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
    }

    public override bool CanInteract() {
        return !isLocked;
    }

    public override string ActionName() {
        return state == State.Open ? "close" : "open";
    }
}
