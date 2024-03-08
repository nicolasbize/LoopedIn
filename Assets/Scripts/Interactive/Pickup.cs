using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : InteractiveObject {

    public bool destroyAfterPickup;
    public bool pickedUp;
    public string actionName;

    public enum Type {
        Briefcase,
        PlayroomCodePuzzle,
        KeyCode,
        Diary,
    }

    public Type type;

    public override string ActionName() {
        return actionName;
    }

    public override bool CanInteract() {
        return !pickedUp;
    }

    public override void Interact() {
        Player.Instance.PickUp(type);
        if (destroyAfterPickup) {
            Destroy(gameObject);
        } else {
            pickedUp = true;
        }
    }
    
}
