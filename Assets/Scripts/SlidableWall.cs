using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidableWall : MonoBehaviour
{
    public GameLogic.GameStep triggerStep;

    void Start()
    {
        GameLogic.Instance.OnStepChange += Gameplay_OnStepChange;
    }

    private void Gameplay_OnStepChange(object sender, GameLogic.OnStepChangeEventArgs e) {
        if (GameLogic.Instance.Step == triggerStep) {
            GetComponent<Animator>().SetTrigger("OpenDoor");
        }
    }
}
