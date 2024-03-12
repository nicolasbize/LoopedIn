using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogoScreen : MonoBehaviour
{

    public event EventHandler OnLogosComplete;

    public void CloseIntro() {
        OnLogosComplete?.Invoke(this, EventArgs.Empty);
    }
}
