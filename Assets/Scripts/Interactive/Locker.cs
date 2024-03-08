using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class Locker : BaseCodeMechanism
{

    public DigitLock[] digits;
    private string current = "0000";

    private void Start() {
        foreach (DigitLock digit in digits) {
            digit.OnButtonPress += Digit_OnButtonPress;
        }
    }

    private void Digit_OnButtonPress(object sender, DigitLock.OnButtonPressEventArgs e) {
        StringBuilder sb = new StringBuilder(current);
        sb[e.index] = e.value.ToString()[0];
        current = sb.ToString();
        CheckCombination();
    }

    private void CheckCombination() {
        if (current == passcode) {
            IssueSuccessEvent();
        } else {
            IssueErrorEvent();
        }
    }

}
