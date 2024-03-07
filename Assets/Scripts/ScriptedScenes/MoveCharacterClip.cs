using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCharacterClip : CharacterClip
{

    private Vector3 origin;
    public Vector3 destination;

    protected override void InitClip() {
        origin = character.transform.position;
        character.humanoid.GetComponent<Animator>().SetBool("IsWalking", true);
        duration = (origin - destination).magnitude / 5f;
    }

    protected override void UpdateProgress(float progress) {
        character.transform.position = Vector3.Lerp(origin, destination, progress);
    }

    public override void Stop() {
        base.Stop();
        character.humanoid.GetComponent<Animator>().SetBool("IsWalking", false);
    }
}
