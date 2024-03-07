using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Apple;

public class TeleportCharacterClip : CharacterClip {

    public Vector3 destination;
    private bool hasTeleported = false;

    protected override void InitClip() {
        duration = 0.1f;
    }

    protected override void UpdateProgress(float progress) {
        if (!hasTeleported) {
            hasTeleported = true;
            character.transform.position = destination;
        }
    }
}
