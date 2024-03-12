using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

public class Fader : MonoBehaviour
{
    public float timeToAppear = 2f;

    private Action callbackOnAppear = null;

    private bool isAppearing = false;
    private float timeTriggered = float.NegativeInfinity;

    public void Appear(Action callback) {
        callbackOnAppear = callback;
        isAppearing = true;
        timeTriggered = Time.timeSinceLevelLoad;
    }

    private void Update() {
        if (isAppearing) {
            float progress = (Time.timeSinceLevelLoad - timeTriggered) / timeToAppear;
            if (progress >= 1f) {
                isAppearing = false;
                if (callbackOnAppear != null) {
                    callbackOnAppear();
                }
            }
        }
    }


}
