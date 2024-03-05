using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : InteractiveObject {

    public enum Type {
        Briefcase,
    }

    public Type type;

    public override string ActionName() {
        return "pick up";
    }

    public override bool CanInteract() {
        return true;
    }

    public override void Interact() {
        Player.Instance.Pickup(type);
        Destroy(gameObject);
    }
    
}
