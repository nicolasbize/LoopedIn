using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractiveObject : MonoBehaviour
{
    public enum InteractionType {
        EKey,
        MouseClick,
    }

    public InteractionType interactionType;
    public Transform highlightObject;

    public void Start() {
        highlightObject.gameObject.SetActive(false);
    }

    public void Highlight() {
        highlightObject.gameObject.SetActive(true);
    }

    public void StopHighlight() {
        highlightObject.gameObject.SetActive(false);
    }

    public abstract bool CanInteract();

    public abstract void Interact();

    public abstract string ActionName();

}
