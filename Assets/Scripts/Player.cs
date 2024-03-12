using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{

    public float speedSitTransition = 1f;
    public Chair currentChair = null;
    public Arm arm;

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
    public event EventHandler OnStartDiaryPuzzle;
    public event EventHandler OnLibrarianNotePickup;
    public event EventHandler OnPassPickup;
    public event EventHandler OnStateChange;

    public static Player Instance;

    public enum State {
        Sitting,
        Moving,
        Talking,
        Puzzling,
        Thinking,
        Typing,
    }

    private State state;
    private State stateBeforeThought = State.Moving;
    private State stateBeforeTalking = State.Moving;

    private void Awake() {
        Instance = this;
        if (currentChair != null) {
            state = State.Sitting;
        } else {
            state = State.Moving;
        }
    }

    public void ReceiveText() {
        arm.Activate();
    }

    private void Start() {
        GameLogic.Instance.OnStepChange += GameLogic_OnStepChange;
    }

    private void GameLogic_OnStepChange(object sender, GameLogic.OnStepChangeEventArgs e) {
        if (e.step == GameLogic.GameStep.GotLockerCombination) {
            StartThinking("Looks like this Jay might have found something interesting...");
        }
    }

    public State GetState() {
        return state;
    }

    public bool CanMove() {
        if (MenuManager.Instance.InMenu) return false;
        return state == State.Moving;
    }

    public bool CanLookAround() {
        if (MenuManager.Instance.InMenu) return false;
        return state == State.Moving || state == State.Sitting || state == State.Typing;
    }

    public void StartTalking(Character target) {
        stateBeforeTalking = state;
        SetState(State.Talking);
        OnStartDialog?.Invoke(this, new OnStartDialogEventArgs() {
            targetDialog = target
        });
    }

    public void StopTalking() {
        SetState(stateBeforeTalking);
        OnStopDialog?.Invoke(this, EventArgs.Empty);
    }

    public void Sit(Chair chair) {
        transform.position = chair.targetAnchor.position;
        transform.rotation = chair.targetAnchor.rotation;
        Camera.main.transform.localRotation = Quaternion.Euler(0, 0, 0);
        SetState(State.Sitting);
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
        SetState(State.Moving);
    }

    public void PickUp(Pickup.Type type) {
        if (type == Pickup.Type.Pass) {
            StartThinking("Time to find out what Dr Miller has been doing and undo it!");
            OnPassPickup?.Invoke(this, EventArgs.Empty);
            return;
        }

        if (type == Pickup.Type.Briefcase) {
            OnStartSmilePuzzle?.Invoke(this, EventArgs.Empty);
        } else if (type == Pickup.Type.PlayroomCodePuzzle) {
            OnStartPlayroomPuzzle?.Invoke(this, EventArgs.Empty);
        } else if (type == Pickup.Type.Diary) {
            OnStartDiaryPuzzle?.Invoke(this, EventArgs.Empty);
        } else if (type == Pickup.Type.ScrambledLibraryNote) {
            OnLibrarianNotePickup?.Invoke(this, EventArgs.Empty);
        }
        SetState(State.Puzzling);
    }

    public void SetState(State newState) {
        state = newState;
        OnStateChange?.Invoke(this, EventArgs.Empty);
    }
    
    public void CompleteSmilePuzzle() {
        GameLogic.Instance.SetStep(GameLogic.GameStep.SolvedBriefcaseClue);
        StartThinking("Looks like my target has something to do with that \"smile\".");
    }

    public void CompleteDiaryPuzzle() {
        GameLogic.Instance.SetStep(GameLogic.GameStep.BrokeDiaryCode);
        
    }

    public void StartThinking(string thought) {
        if (state == State.Thinking) return; // TODO: fix this ugly state management
        stateBeforeThought = state;
        SetState(State.Thinking);
        OnStartThinking?.Invoke(this, new OnStartThinkingEventArgs() {
            thought = thought
        });
    }

    public void StopThinking() {
        if (stateBeforeThought == State.Puzzling) {
            SetState(State.Moving);
        } else {
            SetState(stateBeforeThought);
        }
    }

    public void StartTyping() {
        SetState(State.Typing);
    }

}
