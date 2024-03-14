using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UIElements;

public class UI : MonoBehaviour
{
    public Transform dialogPanel;
    public Transform dialogText;
    public Transform dialogOptionPrefab;
    public Transform responsePanel;
    public Transform thoughtPanel;
    public Transform thoughtText;
    public Canvas inGameCanvas;

    private bool isNpcTalking;
    private bool isPlayerResponding;
    private bool isPlayerThinking;

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
        thoughtPanel.gameObject.SetActive(false);
        dialogPanel.gameObject.SetActive(false);
        responsePanel.gameObject.SetActive(false);

        Player.Instance.OnStartDialog += Player_OnStartDialog;
        Player.Instance.OnStopDialog += Player_OnStopDialog;
        Player.Instance.OnStartThinking += Player_OnStartThinking;

    }

    private void Player_OnStartThinking(object sender, Player.OnStartThinkingEventArgs e) {
        thoughtText.GetComponent<TextMeshProUGUI>().text = e.thought;
        thoughtPanel.gameObject.SetActive(true);
    }

    private void Player_OnStopDialog(object sender, System.EventArgs e) {
        dialogPanel.gameObject.SetActive(false);
    }

    private void Player_OnStartDialog(object sender, Player.OnStartDialogEventArgs e) {
        InitiateConversation(e.targetDialog.dialog);
    }

    private void InitiateConversation(DialogSO dialog) {
        dialogPanel.gameObject.SetActive(true);
        currentDialog = dialog;
        currentBranch = null;
        dialogText.GetComponent<TextMeshProUGUI>().text = currentDialog.greeting + " (click to continue)";
    }


    private DialogSO.Branch[] GetConversations() {
        DialogSO.Branch[] branches = (currentBranch == null ? currentDialog.branches : currentBranch.branches);
        DialogSO.Branch[] filteredBranches = branches.ToList().FindAll(b => {
            return (b.currentStepRequirement == GameLogic.GameStep.None) ||
            (b.currentStepRequirement == GameLogic.Instance.Step);
        }).ToArray();
        return filteredBranches;
    }

    private void ShowResponseOptions() {
        DialogSO.Branch[] branches = GetConversations();
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
            DialogSO.Branch[] branches = GetConversations();
            if (userOption < branches.Length) {
                currentBranch = branches[userOption];
                RefreshCharResponse();
            }
        }
    }

    private void RefreshCharResponse() {
        responsePanel.gameObject.SetActive(false);
        dialogPanel.gameObject.SetActive(true);
        dialogText.GetComponent<TextMeshProUGUI>().text = currentBranch.response + " (click to continue)";
        if (currentBranch.stepUnlocked != GameLogic.GameStep.None) {
            GameLogic.Instance.SetStep(currentBranch.stepUnlocked);
        }
    }

    private void Update() {
        isPlayerResponding = responsePanel.gameObject.activeSelf;
        isNpcTalking = dialogPanel.gameObject.activeSelf;
        isPlayerThinking = thoughtPanel.gameObject.activeSelf;
        if (isNpcTalking && Input.GetMouseButtonDown(0)) {
            ShowResponseOptions();
        } else if (isPlayerResponding) {
            CheckForUserInput();
        } else if (isPlayerThinking && Input.GetMouseButtonDown(0)) {
            thoughtPanel.gameObject.SetActive(false);
            Player.Instance.StopThinking();
        }
    }


}
