using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Credits : MonoBehaviour
{
    private bool isFinished = false;

    public void CreditsFinishedRolling() {
        isFinished = true;
    }

    private void Update() {
        if (isFinished && Input.GetMouseButton(0)) {
            Application.Quit();
        }
    }
}
