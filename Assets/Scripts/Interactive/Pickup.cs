using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : InteractiveObject {

    public bool destroyAfterPickup;
    public bool pickedUp;
    public string actionName;

    public enum Type {
        Briefcase,
        PlayroomCodePuzzle,
        KeyCode,
        Diary,
        ScrambledLibraryNote,
        Pass,
    }

    public Type type;

    private new void Start() {
        base.Start();
        GameLogic.Instance.OnStepChange += GameLogic_OnStepChange;
    }

    private void GameLogic_OnStepChange(object sender, GameLogic.OnStepChangeEventArgs e) {

        if (e.step == GameLogic.GameStep.TalkedToBoyfriend) {
            pickedUp = false;
        }
    }

    public override string ActionName() {
        return actionName;
    }

    public override bool CanInteract() {
        return !pickedUp;
    }

    public override void Interact() {
        Player.Instance.PickUp(type);
        GetComponent<AudioSource>().Play();
        if (destroyAfterPickup) {
            pickedUp = true;
            StartCoroutine(DestroyAfterSound());
        }
    }

    private IEnumerator DestroyAfterSound() {
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
    
}
