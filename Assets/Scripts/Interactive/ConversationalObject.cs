using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ConversationalObject : MonoBehaviour
{
    private bool isLocked = false;

    public abstract void Talk();

    public virtual bool CanInteract() {
        return !isLocked;
    }

    public void Lock() {
        isLocked = true;
    }

    public void Unlock() {
        isLocked = false;
    }

}
