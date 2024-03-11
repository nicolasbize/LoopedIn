using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ClipManager : MonoBehaviour
{
    public GameLogic.GameStep triggerStep;
    public GameLogic.GameStep executesStep;
    public Character[] charactersInvolved;

    private int currentStep;
    private float clipStartTime = float.NegativeInfinity;
    private BaseClip[] clips;

    private void Start() {
        GameLogic.Instance.OnStepChange += GameLogic_OnStepChange;
        clips = GetComponents<BaseClip>();
    }

    private void GameLogic_OnStepChange(object sender, GameLogic.OnStepChangeEventArgs e) {
        if (triggerStep != GameLogic.GameStep.None &&
            triggerStep == e.step) {
            StartMovie();
        }
    }

    private void StartMovie() {
        currentStep = -1;
        foreach (Character character in charactersInvolved) {
            character.Lock();
        }
        PlayNextStep();
    }

    private void StopMovie() {
        foreach (Character character in charactersInvolved) {
            character.Unlock();
        }
    }

    private void PlayNextStep() {
        currentStep += 1;
        if (currentStep < clips.Length) {
            clips[currentStep].OnClipFinished += ClipManager_OnClipFinished;
            clips[currentStep].Play();
        } else {
            StopMovie();
        }
    }

    private void ClipManager_OnClipFinished(object sender, System.EventArgs e) {
        PlayNextStep();
    }
}
