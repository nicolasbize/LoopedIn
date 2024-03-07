using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkCharacterClip : CharacterClip {

    public string dialog;

    protected override void InitClip() {
        character.ThinkOutLoud(dialog);
        duration = 0.5f + dialog.Length / 10f;
    }

    protected override void UpdateProgress(float progress) {
    }

    public override void Stop() {
        base.Stop();
        character.StopThinkingOutLoud();
    }

}
