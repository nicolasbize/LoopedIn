using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Table : InteractiveObject {
    
    public Transform targetAnchor;

    public override string ActionName() {
        return "stand up";
    }

    public override bool CanInteract() {
        return Player.Instance.GetState() == Player.State.Sitting &&
            (Player.Instance.transform.position - transform.position).magnitude < 2f;
    }

    public override void Interact() {
        if (CanInteract()) {
            Player.Instance.Stand(targetAnchor);
        }
    }
}
