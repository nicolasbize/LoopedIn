using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabsDoor : MonoBehaviour
{

    public Button doorButton;
    public float timeAutoClose = 3f;
    private float timeStartClose = float.NegativeInfinity;
    private bool opened = false;

    void Start()
    {
        doorButton.OnButtonPress += DoorButton_OnButtonPress;
        Player.Instance.OnPassPickup += Player_OnPassPickup;
    }

    private void Player_OnPassPickup(object sender, System.EventArgs e) {
        doorButton.Enable();
    }

    private void Update() {
        if (opened && (Time.timeSinceLevelLoad - timeStartClose) > timeAutoClose) {
            opened = false;
            GetComponent<Animator>().SetTrigger("Close");
        }
    }

    private void DoorButton_OnButtonPress(object sender, Button.OnButtonPressEventArgs e) {
        GetComponent<Animator>().SetTrigger("Open");
        timeStartClose = Time.timeSinceLevelLoad;
        opened = true;
    }


}
