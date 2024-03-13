using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndLoopCanvas : MonoBehaviour
{
    public event EventHandler OnReadyForLoop;
    public event EventHandler OnReadyToPrepareLoop;
    
    public Transform black;
    public Transform white;

    private float durationTransition = 2f;
    private bool isPlayingWhite = false;
    private bool isPlayingBlack = false;
    private float startTransitionTime = float.NegativeInfinity;
    
    public void StartEndLoop() {
        isPlayingWhite = true;
        startTransitionTime = Time.timeSinceLevelLoad;
    }

    public void RestartGame() {
        isPlayingBlack = true;
        startTransitionTime = Time.timeSinceLevelLoad;
        GetComponent<AudioSource> ().Play();
    }

    private void Update() {
        if (isPlayingWhite) {
            float progress = (Time.timeSinceLevelLoad - startTransitionTime) / durationTransition;
            if (progress < 1f) {
                Color newColor = white.GetComponent<RawImage>().color;
                newColor.a = progress;
                white.GetComponent<RawImage>().color = newColor;
            } else {
                white.GetComponent<RawImage>().color = Color.white;
                isPlayingWhite = false;
                OnReadyToPrepareLoop?.Invoke(this, EventArgs.Empty);
            }
        } else if (isPlayingBlack) {
            float progress = (Time.timeSinceLevelLoad - startTransitionTime) / durationTransition;
            if (progress < 1f) {
                Color newColor = black.GetComponent<RawImage>().color;
                newColor.a = progress;
                black.GetComponent<RawImage>().color = newColor;
            } else {
                black.GetComponent<RawImage>().color = Color.black;
                isPlayingBlack = false;
                FinishGame();
            }
        }
    }

    private void FinishGame() {
        OnReadyForLoop?.Invoke(this, EventArgs.Empty);
    }

}
