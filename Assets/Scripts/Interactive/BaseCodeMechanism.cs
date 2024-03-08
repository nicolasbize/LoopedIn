using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCodeMechanism : MonoBehaviour
{
    public event EventHandler OnSuccessCodeEntered;
    public event EventHandler OnErrorCodeEntered;
    public string passcode;
    public GameLogic.GameStep stepAfterUnlock;

    protected void IssueSuccessEvent() {
        OnSuccessCodeEntered?.Invoke(this, EventArgs.Empty);
    }

    protected void IssueErrorEvent() {
        OnErrorCodeEntered?.Invoke(this, EventArgs.Empty);
    }
}
