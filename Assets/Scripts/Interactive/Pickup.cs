using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : InteractiveObject {

    public bool destroyAfterPickup;
    public bool pickedUp;

    public enum Type {
        Briefcase,
        PlayroomCodePuzzle,
    }

    public Type type;

    public override string ActionName() {
        return "pick up";
    }

    public override bool CanInteract() {
        return !pickedUp;
    }

    public override void Interact() {
        Player.Instance.Pickup(type);
        if (destroyAfterPickup) {
            Destroy(gameObject);
        } else {
            pickedUp = true;
        }
    }
    
}
