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
    public AudioClip wakeUpClip;

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
        None,
        Sitting,
        Moving,
        Talking,
        Puzzling,
        Thinking,
        Typing,
        Dying,
    }

    private State state;
    private State stateBeforeThought = State.Moving;
    private State stateBeforeTalking = State.Moving;
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private int hintHelp = 0;

    private void Awake() {
        Instance = this;
        if (currentChair != null) {
            state = State.Sitting;
        } else {
            state = State.Moving;
        }
        originalPosition = transform.position;
        originalRotation = transform.rotation;
    }

    public void WakeUp() {
        GetComponent<AudioSource>().clip = wakeUpClip;
        GetComponent<AudioSource>().Play();
    }

    public void ReceiveText() {
        arm.gameObject.SetActive(true);
        arm.Activate();
    }

    public void ReceiveTextAndQuit() {
        MenuManager.Instance.FadeOutAndCredits();
    }

    private void Start() {
        GameLogic.Instance.OnStepChange += GameLogic_OnStepChange;
    }

    private void GameLogic_OnStepChange(object sender, GameLogic.OnStepChangeEventArgs e) {

        if (e.step == GameLogic.GameStep.GotLockerCombination) {
            StartThinking("Looks like this Jay might have found something interesting...");
        }

        // update hints
        switch (GameLogic.Instance.Step) {
            case GameLogic.GameStep.ReceivedWakeUpCall:
                HintManager.Instance.UpdateHint("The text I received mentioned a way to identify my target in the bathroom stall. I should head there.");
                break;
            case GameLogic.GameStep.SolvedBriefcaseClue:
                HintManager.Instance.UpdateHint("I need to find a smiling face around here...");
                break;
            case GameLogic.GameStep.FoundMillerPortrait:
                HintManager.Instance.UpdateHint("I should ask around in the hallway if anyone knows my target Dr Miller");
                break;
            case GameLogic.GameStep.TalkedToBoyfriend:
                HintManager.Instance.UpdateHint("I should check in the trash to find the code to the break room.");
                break;
            case GameLogic.GameStep.OpenedPlayroom:
                HintManager.Instance.UpdateHint("I should talk with people inside to get information about Dr Miller.");
                break;
            case GameLogic.GameStep.CompletedEmmaConversation:
                HintManager.Instance.UpdateHint("I should pay attention to the current conversation between Emma and the IT guy...");
                break;
            case GameLogic.GameStep.HeardWeakPassword:
                HintManager.Instance.UpdateHint("Looks like Emma Stoned is using the default password PASSWORD123.");
                break;
            case GameLogic.GameStep.GotLockerCombination:
                HintManager.Instance.UpdateHint("I should check Jay's locker in the men's bathroom. 5th one, code is 4827.");
                break;
            case GameLogic.GameStep.BrokeDiaryCode:
                HintManager.Instance.UpdateHint("I should go into the library to find that hidden room. Jay mentioned to tally the blue, red and green.");
                break;
            case GameLogic.GameStep.OpenedSecretLibraryRoom:
                HintManager.Instance.UpdateHint("I should try to find the librarian's full name and password to access their account.");
                break;
            case GameLogic.GameStep.AccessedSecretComputer:
                HintManager.Instance.UpdateHint("I should go find the lab facility pass. Girls' middle locker, code 8453.");
                break;
            case GameLogic.GameStep.AccessedLab:
                HintManager.Instance.UpdateHint("I should check the facility for clues.");
                break;
            case GameLogic.GameStep.FoundMillerPasswordHint:
                HintManager.Instance.UpdateHint("Dr Miller turned 50 on Feb 01 1993, that should help me find the password.");
                break;
            case GameLogic.GameStep.FoundInjectionProcedure:
                HintManager.Instance.UpdateHint("I should type the command OVERRIDE INJECT on each computer.");
                break;
            case GameLogic.GameStep.StartedVialPuzzle:
                HintManager.Instance.UpdateHint("Looks like the vial already has some content. I need to match the ratios.");
                break;
            default:
                break;
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
        return state == State.Moving || state == State.Sitting || state == State.Typing || state == State.Dying;
    }

    public void StartTalking(Character target) {
        if (state != State.Talking) {
            stateBeforeTalking = state;
        }
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

    public void Die() {
        SetState(State.Dying);
        GameLogic.Instance.FinishGame();
    }

    public void ResetToOriginalTransform() {
        transform.position = originalPosition;
        transform.rotation = originalRotation;
        
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
            HintManager.Instance.UpdateHint("I should be able to access the lab facility now.");
            OnPassPickup?.Invoke(this, EventArgs.Empty);
            return;
        }

        if (type == Pickup.Type.Briefcase) {
            OnStartSmilePuzzle?.Invoke(this, EventArgs.Empty);
            HintManager.Instance.UpdateHint("Looks like a puzzle around letters... Press H for a hint!");
        } else if (type == Pickup.Type.PlayroomCodePuzzle) {
            OnStartPlayroomPuzzle?.Invoke(this, EventArgs.Empty);
            GameLogic.Instance.SetStep(GameLogic.GameStep.FoundTrashNote);
            HintManager.Instance.UpdateHint("Looks like I need to find a 6 digit number... Press H for a hint!");
        } else if (type == Pickup.Type.Diary) {
            OnStartDiaryPuzzle?.Invoke(this, EventArgs.Empty);
            HintManager.Instance.UpdateHint("Looks like I need to swap letters to decode Jay's journal... Press H for a hint!");
        } else if (type == Pickup.Type.ScrambledLibraryNote) {
            OnLibrarianNotePickup?.Invoke(this, EventArgs.Empty);
            GameLogic.Instance.SetStep(GameLogic.GameStep.ReadJerryLeeCrumbledPaper);
            HintManager.Instance.UpdateHint("Looks like I can guess the librarian's password... Press H for a hint!");
        }
        SetState(State.Puzzling);
    }

    public void SetState(State newState) {
        state = newState;
        OnStateChange?.Invoke(this, EventArgs.Empty);
    }
    
    public void CompleteSmilePuzzle() {
        GameLogic.Instance.SetStep(GameLogic.GameStep.SolvedBriefcaseClue);
        StartThinking("Looks like my target has something to do with that \"smile\".", State.Moving);
    }

    public void CompleteDiaryPuzzle() {
        GameLogic.Instance.SetStep(GameLogic.GameStep.BrokeDiaryCode);
    }

    public void StartThinking(string thought, State overrideReturnState = State.None) {
        if (state == State.Thinking) return; // TODO: fix this ugly state management
        stateBeforeThought = overrideReturnState == State.None ? state : overrideReturnState;
        SetState(State.Thinking);
        OnStartThinking?.Invoke(this, new OnStartThinkingEventArgs() {
            thought = thought
        });
    }

    public void StopThinking(State overrideReturnState = State.None) {
        SetState(overrideReturnState == State.None ? stateBeforeThought : overrideReturnState);
    }

    public void StartTyping() {
        SetState(State.Typing);
    }

    private void Update() {
        if(state == State.Puzzling && Input.GetKeyDown(KeyCode.H)) {
            ShowTip();
        }
    }

    private void ShowTip() {
        switch(GameLogic.Instance.Step) {
            case GameLogic.GameStep.ReceivedWakeUpCall:
                StartThinking("Looks like the letter S is in sock but not in clock... I should select all the letters that are in the first word but not in the second.");
                break;
            case GameLogic.GameStep.FoundTrashNote:
                StartThinking("I'm looking for a 6 digit number. 3 orange weights equal 30lbs, so if I find the green weight the combination is green-orange-green.");
                break;
            case GameLogic.GameStep.OpenedLocker:
                StartThinking("Looks like the pages start with a date, I should try to find a day and a month.");
                break;
            case GameLogic.GameStep.ReadJerryLeeCrumbledPaper:
                StartThinking("They've been here for 2 years, and change the default password PASSWORD123 every month.");
                break;
            default:
                break;
        }
    }

}
