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
            OnPassPickup?.Invoke(this, EventArgs.Empty);
            return;
        }

        if (type == Pickup.Type.Briefcase) {
            OnStartSmilePuzzle?.Invoke(this, EventArgs.Empty);
        } else if (type == Pickup.Type.PlayroomCodePuzzle) {
            OnStartPlayroomPuzzle?.Invoke(this, EventArgs.Empty);
            GameLogic.Instance.SetStep(GameLogic.GameStep.FoundTrashNote);
        } else if (type == Pickup.Type.Diary) {
            OnStartDiaryPuzzle?.Invoke(this, EventArgs.Empty);
        } else if (type == Pickup.Type.ScrambledLibraryNote) {
            OnLibrarianNotePickup?.Invoke(this, EventArgs.Empty);
            GameLogic.Instance.SetStep(GameLogic.GameStep.ReadJerryLeeCrumbledPaper);
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
        if(CanReceiveHint() && Input.GetKeyDown(KeyCode.F1)) {
            ShowTip();
        }
    }

    private bool CanReceiveHint() {
        return state == State.Moving || state == State.Puzzling;
    }

    private void ShowTip() {
        switch(GameLogic.Instance.Step) {
            case GameLogic.GameStep.ReceivedWakeUpCall:
                if (state == State.Puzzling) {
                    StartThinking("Looks like the letter S is in sock but not in moon...");
                } else {
                    StartThinking("The text said I should check the bathroom stall to identify my target...");
                }
                break;
            case GameLogic.GameStep.SolvedBriefcaseClue:
                StartThinking("I should inspect the paintings on the wall and find the person who smiles.");
                break;
            case GameLogic.GameStep.FoundMillerPortrait:
                StartThinking("I should try to find someone in the hallway that knows Dr Miller.");
                break;
            case GameLogic.GameStep.TalkedToBoyfriend:
                StartThinking("I should check the trash to find Dr Miller's note to access the break room.");
                break;
            case GameLogic.GameStep.FoundTrashNote:
                if (state == State.Puzzling) {
                    StartThinking("Each orange weight seems to be 10lbs, so the green ones must weight 25lbs each. The code should map to green-orange-green.");
                } else {
                    StartThinking("I should check Dr Miller's note again to find the code to the break room.");
                }
                break;
            case GameLogic.GameStep.OpenedPlayroom:
                StartThinking("I should try to find one of Dr Miller's friends in the break room.");
                break;
            case GameLogic.GameStep.CompletedEmmaConversation:
            case GameLogic.GameStep.HeardWeakPassword:
                StartThinking("Once I have that person's full name, I can access her computer account using the default PASSWORD123.");
                break;
            case GameLogic.GameStep.MappedStonedPortrait:
                StartThinking("Emma Stoned, PASSWORD123. Let's access her account.");
                break;
            case GameLogic.GameStep.GotLockerCombination:
                StartThinking("I should check Jay's locker. 5th one, code is 4827.");
                break;
            case GameLogic.GameStep.OpenedLocker:
                StartThinking("I need to understand Jay's journal. Looks like it starts with a date...");
                break;
            case GameLogic.GameStep.BrokeDiaryCode:
                StartThinking("Jay's diary said to count the blue, red and green to find the code in the library.");
                break;
            case GameLogic.GameStep.OpenedSecretLibraryRoom:
                StartThinking("I've got to find the user name and password.");
                break;
            case GameLogic.GameStep.ReadJerryLeeCrumbledPaper:
                StartThinking("They've been here for 2 years, and change the default password PASSWORD123 every month.");
                break;
            case GameLogic.GameStep.AccessedSecretComputer:
                StartThinking("Looks like a lab pass is hidden in the girls middle locker. Code is 8453.");
                break;
            case GameLogic.GameStep.RetrievedIDCard:
                StartThinking("Now that I have the pass, I should head over to the lab.");
                break;
            case GameLogic.GameStep.AccessedLab:
                StartThinking("I should check the desks for clues on names and passwords.");
                break;
            case GameLogic.GameStep.FoundMillerPasswordHint:
                StartThinking("Mike mentioned Dr Miller's 50th birthday in an email dating from Feb 1st 1993. I should try Miller's birthday as password.");
                break;
            case GameLogic.GameStep.FoundInjectionProcedure:
                StartThinking("Looks like the 4 users need to enter the command OVERRIDE INJECT");
                break;
            case GameLogic.GameStep.StartedVialPuzzle:
                StartThinking("The formula on the board should break whatever is going on. I need to match the ratio. ");
                break;
            default:
                Debug.Log("could not find step to help with");
                Debug.Log(GameLogic.Instance.Step);
                break;
        }
    }

}
