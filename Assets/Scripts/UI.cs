using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class UI : MonoBehaviour
{
    public Transform dialogPanel;
    public Transform dialogText;
    public Transform dialogOptionPrefab;
    public Transform responsePanel;

    private bool isNpcTalking;
    private bool isUserResponding;
    private DialogSO currentDialog;
    private DialogSO.Branch currentBranch = null;

    private Dictionary<KeyCode, int> numDict = new Dictionary<KeyCode, int>();

    private void Awake() {
        numDict.Add(KeyCode.Alpha1, 0);
        numDict.Add(KeyCode.Alpha2, 1);
        numDict.Add(KeyCode.Alpha3, 2);
        numDict.Add(KeyCode.Alpha4, 3);
    }

    void Start()
    {
        Player.Instance.OnStartDialog += Instance_OnStartDialog;
        Player.Instance.OnStopDialog += Instance_OnStopDialog;
    }

    private void Instance_OnStopDialog(object sender, System.EventArgs e) {
        dialogPanel.gameObject.SetActive(false);
    }

    private void Instance_OnStartDialog(object sender, Player.OnStartDialogEventArgs e) {
        InitiateConversation(e.targetDialog.dialog);
    }

    private void InitiateConversation(DialogSO dialog) {
        dialogPanel.gameObject.SetActive(true);
        currentDialog = dialog;
        currentBranch = null;
        dialogText.GetComponent<TextMeshProUGUI>().text = currentDialog.greeting + " (click to continue)";
    }


    private void ShowResponseOptions() {
        DialogSO.Branch[] branches = (currentBranch == null ? currentDialog.branches : currentBranch.branches);
        if (branches.Length > 0) {
            dialogPanel.gameObject.SetActive(false);
            foreach (Transform t in responsePanel.transform) {
                Destroy(t.gameObject);
            }
            for (int i = 0; i < branches.Length; i++) {
                DialogSO.Branch branch = branches[i];
                Transform option = Instantiate(dialogOptionPrefab, responsePanel.transform);
                option.GetComponent<TextMeshProUGUI>().text = (i + 1).ToString() + ". " + branch.question;
            }
            responsePanel.gameObject.SetActive(true);

        } else {
            Player.Instance.StopTalking();
        }
    }

    private void CheckForUserInput() {
        int userOption = -1;
        foreach (KeyCode code in numDict.Keys) {
            if (Input.GetKeyDown(code)) {
                userOption = numDict[code];
            }
        }
        if (userOption > -1) {
            if (currentBranch == null) {
                currentBranch = currentDialog.branches[userOption];
            } else {
                currentBranch = currentBranch.branches[userOption];
            }
            RefreshCharResponse();
        }
    }

    private void RefreshCharResponse() {
        responsePanel.gameObject.SetActive(false);
        dialogPanel.gameObject.SetActive(true);
        dialogText.GetComponent<TextMeshProUGUI>().text = currentBranch.response + " (click to continue)";
    }

    private void Update() {
        isUserResponding = responsePanel.gameObject.activeSelf;
        isNpcTalking = dialogPanel.gameObject.activeSelf;
        if (isNpcTalking && Input.GetMouseButtonDown(0)) {
            ShowResponseOptions();
        } else if (isUserResponding) {
            CheckForUserInput();
        }
    }


}
