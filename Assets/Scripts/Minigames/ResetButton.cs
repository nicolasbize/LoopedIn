using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ResetButton : MonoBehaviour, IPointerClickHandler {

    public event EventHandler OnResetClick;

    public void OnPointerClick(PointerEventData eventData) {
        OnResetClick?.Invoke(this, EventArgs.Empty);
    }
}
