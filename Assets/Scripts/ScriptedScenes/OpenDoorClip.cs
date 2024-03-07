using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoorClip : BaseClip
{
    public Door door;

    protected override void InitClip() {
        duration = 0.1f;
    }

    protected override void UpdateProgress(float progress) {
        if (door.GetState() == Door.State.Closed) {
            door.Interact();
        }
    }

}
