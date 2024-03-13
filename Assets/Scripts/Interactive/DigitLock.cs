using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DigitLock : InteractiveObject {

    public event EventHandler<OnButtonPressEventArgs> OnButtonPress;
    public class OnButtonPressEventArgs : EventArgs {
        public string value;
        public int index;
    }

    public Transform textmesh;

    private int value = 0;
    private int digitIndex = 0;

    public override string ActionName() {
        return "Turn knob";
    }

    public override bool CanInteract() {
        return true;
    }

    public override void Interact() {
        value = (value + 1) % 10;
        textmesh.GetComponent<TextMeshProUGUI>().text = value.ToString();
        int.TryParse(gameObject.name.Split("-")[1], out digitIndex);
        GetComponent<AudioSource>().Play();
        OnButtonPress?.Invoke(this, new OnButtonPressEventArgs() {
            value = value.ToString(),
            index = digitIndex,
        });
    }
}
