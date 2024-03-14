using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabsDoor : MonoBehaviour
{

    public Button doorButton;
    public float timeAutoClose = 3f;
    public AudioClip soundOpen;
    public AudioClip soundClose;
    
    private float timeStartClose = float.NegativeInfinity;
    private bool opened = false;

    public event EventHandler OnClose;

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
            OnClose?.Invoke(this, EventArgs.Empty);
            GetComponent<Animator>().SetTrigger("Close");
            GetComponent<AudioSource>().clip = soundClose;
            GetComponent<AudioSource>().Play();
        }
    }

    private void DoorButton_OnButtonPress(object sender, Button.OnButtonPressEventArgs e) {
        GetComponent<Animator>().SetTrigger("Open");
        GetComponent<AudioSource>().clip = soundOpen;
        GetComponent<AudioSource>().Play();
        GameLogic.Instance.SetStep(GameLogic.GameStep.AccessedLab);
        timeStartClose = Time.timeSinceLevelLoad;
        opened = true;
    }


}
