using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public float speedSitTransition = 1f;


    public event EventHandler OnStopDialog;
    public event EventHandler<OnStartDialogEventArgs> OnStartDialog;
    public class OnStartDialogEventArgs : EventArgs {
        public Character targetDialog;
    }


    public static Player Instance;

    public enum State {
        Sitting,
        Moving,
        Talking,
    }

    private State state;
    private Chair currentChair = null;

    private void Awake() {
        Instance = this;
        state = State.Moving;
    }

    public State GetState() {
        return state;
    }

    public bool CanMove() {
        return state == State.Moving;
    }

    public bool CanLookAround() {
        return state != State.Talking;
    }

    public void StartTalking(Character target) {
        state = State.Talking;
        OnStartDialog?.Invoke(this, new OnStartDialogEventArgs() {
            targetDialog = target
        });
    }

    public void StopTalking() {
        state = State.Moving;
        OnStopDialog?.Invoke(this, EventArgs.Empty);
    }

    public void Sit(Chair chair) {
        transform.position = chair.targetAnchor.position;
        transform.rotation = chair.targetAnchor.rotation;
        Camera.main.transform.localRotation = Quaternion.Euler(0, 0, 0);
        state = State.Sitting;
        currentChair = chair;
    }

    public void Stand(Transform targetDestination) {
        transform.position = targetDestination.position;
        transform.rotation = targetDestination.rotation;
        Camera.main.transform.localRotation = Quaternion.Euler(0, 0, 0);
        if (currentChair != null) {
            currentChair.FreeUp();
            currentChair = null;
        }
        state = State.Moving;
    }

    internal void Pickup(Pickup.Type type) {
        Debug.Log("here");
    }
}
