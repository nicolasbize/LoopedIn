using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plaque : InteractiveObject
{
    public string text;
    [TextArea]
    public string storyText;
    public bool triggerStory;

    public override string ActionName() {
        return "read";
    }

    public override bool CanInteract() {
        return true;
    }

    public override void Interact() {
        if (triggerStory && GameLogic.Instance.Step == GameLogic.GameStep.SolvedBriefcaseClue) {
            Player.Instance.StartThinking(storyText);
            GameLogic.Instance.SetStep(GameLogic.GameStep.FoundMillerPortrait);
        } else {
            Player.Instance.StartThinking(text);
        }
    }

}
