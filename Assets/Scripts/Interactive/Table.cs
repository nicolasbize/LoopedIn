using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Table : InteractiveObject {
    
    public Transform targetAnchor;

    public override string ActionName() {
        return "stand";
    }

    public override bool CanInteract() {
        return ((Player.Instance.GetState() == Player.State.Sitting) ||
                (Player.Instance.GetState() == Player.State.Typing)) &&
            (Player.Instance.transform.position - transform.position).magnitude < 2.5f;
    }

    public override void Interact() {
        if (CanInteract()) {
            Player.Instance.Stand(targetAnchor);            
        }
    }
}
