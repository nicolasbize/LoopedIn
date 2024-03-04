using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI : MonoBehaviour
{
    public Transform dialogPanel;
    public Transform dialogText;

    private bool isTalking;

    void Start()
    {
        Player.Instance.OnStartDialog += Instance_OnStartDialog;
        Player.Instance.OnStopDialog += Instance_OnStopDialog;
    }

    private void Instance_OnStopDialog(object sender, System.EventArgs e) {
        dialogPanel.gameObject.SetActive(false);
        isTalking = false;
    }

    private void Instance_OnStartDialog(object sender, Player.OnStartDialogEventArgs e) {
        dialogPanel.gameObject.SetActive(true);
        dialogText.GetComponent< TextMeshProUGUI>().text = e.targetDialog.dialog.npcText + " (click to continue)";
        isTalking = true;
    }

    private void Update() {
        if (isTalking && Input.GetMouseButtonDown(0)) {
            Player.Instance.StopTalking();
        }
    }
}
