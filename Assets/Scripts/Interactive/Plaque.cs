using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plaque : InteractiveObject
{
    [TextArea] public string text;
    [TextArea] public string storyText;
    public GameLogic.GameStep triggerStep;
    public GameLogic.GameStep executesStep;

    public override string ActionName() {
        return "inspect";
    }

    public override bool CanInteract() {
        return true;
    }

    public override void Interact() {
        if (triggerStep != GameLogic.GameStep.None && GameLogic.Instance.Step == triggerStep) {
            Player.Instance.StartThinking(storyText);
            GameLogic.Instance.SetStep(executesStep);
        } else {
            Player.Instance.StartThinking(text);
        }
    }

}
