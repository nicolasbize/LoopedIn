using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public event EventHandler OnStopDialog;
    public event EventHandler<OnStartDialogEventArgs> OnStartDialog;
    public class OnStartDialogEventArgs : EventArgs {
        public Character targetDialog;
    }


    public static Player Instance;

    private enum State {
        Moving,
        Talking,
    }

    private State state;

    private void Awake() {
        Instance = this;
        state = State.Moving;
    }

    public bool CanMove() {
        return state == State.Moving;
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

    
}
