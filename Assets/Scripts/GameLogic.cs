using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    
    public event EventHandler<OnStepChangeEventArgs> OnStepChange;
    public class OnStepChangeEventArgs : EventArgs {
        public GameStep step;
    }

    public Transform roofToEnable;

    public static GameLogic Instance;

    [System.Serializable]
    public enum GameStep {       // event that triggers this step:
        None,                    // start of the game
        ReceivedWakeUpCall,      // player has just received message on watch
        SolvedBriefcaseClue,      // player has just found clue SMILE
        FoundMillerPortrait,     // player has clicked on Dr Miller face, can start inquerying
        OpenedPlayroom,          // player opens up launge
        CompletedEmmaConversation,
        HeardWeakPassword,       // IT guy complained about password123
        MappedStonedPortrait,    // player clicked on Emma Stoned face, can log in
        AccessedWeakAccount,     // Saw locker combination
        OpenedLocker,
        BrokeDiaryCode,
        OpenedSecretLibraryRoom,
        AccessedSecretComputer,
        RetrievedIDCard,

    }

    public GameStep Step { get; private set; }

    private void Awake() {
        Instance = this;
    }

    public void SetStep(GameStep step) {
        this.Step = step;
        OnStepChange?.Invoke(this, new OnStepChangeEventArgs() {
            step = this.Step
        });
    }

    // Start is called before the first frame update
    void Start()
    {
        roofToEnable.gameObject.SetActive(true);
    }

}
