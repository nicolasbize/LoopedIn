using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractiveObject : MonoBehaviour
{
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

    public abstract void Interact();

}
