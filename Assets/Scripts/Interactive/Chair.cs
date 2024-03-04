using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chair : InteractiveObject
{
    public State state;
    public Transform targetAnchor;

    public override void Interact() {
        if (CanInteract()) {
            state = State.Occupied;
            Player.Instance.Sit(this);
        }
    }

    public override bool CanInteract() {
        return state == State.Free;
    }

    public override string ActionName() {
        return "sit";
    }

    public void FreeUp() {
        state = State.Free;
    }

    public enum State {
        Free,
        Occupied,
    }


}
