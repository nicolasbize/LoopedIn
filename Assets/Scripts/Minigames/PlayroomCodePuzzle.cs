using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayroomCodePuzzle : MonoBehaviour
{
    public Transform hintCanvas;

    void Start() {
        hintCanvas.gameObject.SetActive(false);
        Player.Instance.OnStartPlayroomPuzzle += Player_OnStartPlayroomPuzzle;
    }

    private void Player_OnStartPlayroomPuzzle(object sender, System.EventArgs e) {
        hintCanvas.gameObject.SetActive(true);
    }

    private void Update() {
        bool isActive = hintCanvas.gameObject.activeSelf;
        if (isActive  && Input.GetMouseButtonDown(0)) {
            hintCanvas.gameObject.SetActive(false);
            Player.Instance.StopThinking();
        }
    }
}
