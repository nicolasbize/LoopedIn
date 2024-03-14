using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WakeUpCanvas : MonoBehaviour
{
    private bool blinked = false;

    private void Start() {
        Player.Instance.OnStateChange += Player_OnStateChange;
    }

    // called from animation
    public void StartBlinking() {
        Player.Instance.WakeUp();
    }

    public void FinishBlinking() {
        Player.Instance.StartThinking("Wow, can't believe this class put me to sleep...");
        blinked = true;
    }

    private void Player_OnStateChange(object sender, System.EventArgs e) {
        if (gameObject.activeSelf && blinked) {
            gameObject.SetActive(false);
            if (GameLogic.Instance.HasFinishedGame) {
                Player.Instance.ReceiveTextAndQuit();
            } else if (Player.Instance.GetState() == Player.State.Sitting) {
                Player.Instance.ReceiveText();
            }
        }
    }
}
