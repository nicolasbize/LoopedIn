using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISmilePuzzle : MonoBehaviour
{
    public Transform smileHintCanvas;

    void Start()
    {
        smileHintCanvas.gameObject.SetActive(false);
        Player.Instance.OnStartSmilePuzzle += Player_OnStartSmilePuzzle;
        smileHintCanvas.GetComponent<SmileLetter>().OnSolutionFound += UISmilePuzzle_OnSolutionFound;
    }

    private void UISmilePuzzle_OnSolutionFound(object sender, System.EventArgs e) {
        smileHintCanvas.gameObject.SetActive(false);
        Player.Instance.CompleteSmilePuzzle();
    }

    private void Player_OnStartSmilePuzzle(object sender, System.EventArgs e) {
        smileHintCanvas.gameObject.SetActive(true);
    }

}
