using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseClip : MonoBehaviour
{
    public event EventHandler OnClipFinished;

    protected bool isPlaying;
    protected float clipStartTime = float.NegativeInfinity;
    protected float duration = 0.5f;


    public virtual void Stop() {
        isPlaying = false;
    }

    public void Play() {
        if (!isPlaying) {
            isPlaying = true;
            clipStartTime = Time.timeSinceLevelLoad;
            InitClip();
        }
    }

    protected abstract void InitClip();
    protected abstract void UpdateProgress(float progress);

    private void Update() {
        if (isPlaying) {
            float progress = (Time.timeSinceLevelLoad - clipStartTime) / duration;
            if (progress >= 1) {
                Stop();
                OnClipFinished?.Invoke(this, EventArgs.Empty);
            } else {
                UpdateProgress(progress);
            }
        }
    }

}
