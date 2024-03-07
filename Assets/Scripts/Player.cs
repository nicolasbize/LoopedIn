using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{

    public float speedSitTransition = 1f;


    public event EventHandler OnStopDialog;
    public event EventHandler<OnStartDialogEventArgs> OnStartDialog;
    public class OnStartDialogEventArgs : EventArgs {
        public Character targetDialog;
    }

    public event EventHandler<OnStartThinkingEventArgs> OnStartThinking;
    public class OnStartThinkingEventArgs : EventArgs {
        public string thought;
    }

    public event EventHandler OnStartSmilePuzzle;
    public event EventHandler OnStartPlayroomPuzzle;
    public event EventHandler OnStateChange;

    public static Player Instance;

    public enum State {
        Sitting,
        Moving,
        Talking,
        Puzzling,
        Thinking,
    }

    private State state;
    private Chair currentChair = null;

    private void Awake() {
        Instance = this;
        state = State.Moving;
    }

    public State GetState() {
        return state;
    }

    public bool CanMove() {
        return state == State.Moving;
    }

    public bool CanLookAround() {
        return state == State.Moving || state == State.Sitting;
    }

    public void StartTalking(Character target) {
        state = State.Talking;
        OnStateChange?.Invoke(this, EventArgs.Empty);
        OnStartDialog?.Invoke(this, new OnStartDialogEventArgs() {
            targetDialog = target
        });
    }

    public void StopTalking() {
        state = State.Moving;
        Debug.Log("stop talking");
        OnStateChange?.Invoke(this, EventArgs.Empty);
        OnStopDialog?.Invoke(this, EventArgs.Empty);
    }

    public void Sit(Chair chair) {
        transform.position = chair.targetAnchor.position;
        transform.rotation = chair.targetAnchor.rotation;
        Camera.main.transform.localRotation = Quaternion.Euler(0, 0, 0);
        state = State.Sitting;
        currentChair = chair;
    }

    public void Stand(Transform targetDestination) {
        transform.position = targetDestination.position;
        transform.rotation = targetDestination.rotation;
        Camera.main.transform.localRotation = Quaternion.Euler(0, 0, 0);
        if (currentChair != null) {
            currentChair.FreeUp();
            currentChair = null;
        }
        state = State.Moving;
        OnStateChange?.Invoke(this, EventArgs.Empty);
    }

    public void Pickup(Pickup.Type type) {
        if (type == global::Pickup.Type.Briefcase) {
            OnStartSmilePuzzle?.Invoke(this, EventArgs.Empty);
            state = State.Puzzling;
            OnStateChange?.Invoke(this, EventArgs.Empty);
        } else if(type == global::Pickup.Type.PlayroomCodePuzzle) {
            OnStartPlayroomPuzzle?.Invoke(this, EventArgs.Empty);
            state = State.Puzzling;
            OnStateChange?.Invoke(this, EventArgs.Empty);
        }
    }

    public void CompleteSmilePuzzle() {
        GameLogic.Instance.SetStep(GameLogic.GameStep.SolvedBriefcaseClue);
        StartThinking("OK, my target has something to do with a smile");
    }

    public void StartThinking(string thought) {
        state = State.Thinking;
        OnStateChange?.Invoke(this, EventArgs.Empty);
        OnStartThinking?.Invoke(this, new OnStartThinkingEventArgs() {
            thought = thought
        });
    }

    public void StopThinking() {
        state = State.Moving;
        OnStateChange?.Invoke(this, EventArgs.Empty);
        Debug.Log("stopped thinking");
    }

}
