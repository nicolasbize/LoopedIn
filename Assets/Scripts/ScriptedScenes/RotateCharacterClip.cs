using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCharacterClip : CharacterClip {

    public enum RotationType { ClockWise, CounterClockWise }
    public RotationType rotationType;
    
    private Quaternion startRotation;
    private Quaternion endRotation;

    protected override void InitClip() {
        startRotation = character.transform.rotation;
        Transform endTransform = character.transform;
        float rotationAmount = rotationType == RotationType.ClockWise ? 90f : -90f;
        endTransform.Rotate(new Vector3(0f, 90f, 0f));
        endRotation = endTransform.rotation;
    }

    protected override void UpdateProgress(float progress) {
        character.transform.rotation = Quaternion.Lerp(
            startRotation, endRotation, progress);
    }
}
