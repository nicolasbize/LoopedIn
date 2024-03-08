using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIDiaryPuzzle : MonoBehaviour
{
    public Transform diaryCanvas;
    private bool completed = false;

    void Start() {
        Player.Instance.OnStateChange += Player_OnStateChange;
        diaryCanvas.gameObject.SetActive(false);
        Player.Instance.OnStartDiaryPuzzle += Player_OnStartDiaryPuzzle;
        diaryCanvas.GetComponent<DiaryPuzzle>().OnSolutionFound += UIDiaryPuzzle_OnSolutionFound;
    }

    private void UIDiaryPuzzle_OnSolutionFound(object sender, System.EventArgs e) {
        completed = true;
        Player.Instance.StartThinking("This looks bad... I've got to find a way into that library secret room.");
    }

    private void Player_OnStateChange(object sender, System.EventArgs e) {
        // we get here after thinking out loud, but without having closed the clue
        if (Player.Instance.GetState() == Player.State.Moving && completed) {
            diaryCanvas.gameObject.SetActive(false);
            Player.Instance.CompleteDiaryPuzzle();
        }
    }

    private void Player_OnStartDiaryPuzzle(object sender, System.EventArgs e) {
        diaryCanvas.gameObject.SetActive(true);
    }
}
