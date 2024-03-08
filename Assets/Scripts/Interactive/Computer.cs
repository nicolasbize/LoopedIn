using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static ComputerAccountSO;

public class Computer : InteractiveObject {


    public Transform canvas;
    public Transform consoleText;
    public ComputerAccountSO[] accounts;

    public bool isFree;
    private bool isTurnedOn;
    private bool isBeingUsed;
    private bool isPendingInput = false;
    private bool isAuthenticated = false;
    private ComputerAccountSO authenticatedAccount = null;

    private int maxRows = 13;
    private int maxCols = 40;
    private string currentInput = "";
    private string username = "";
    private string password = "";
    private string prompt = ">> ";

    private List<string> consoleLines = new List<string>();

    private Dictionary<KeyCode, string> validKeys = new Dictionary<KeyCode, string>();

    public new void Start() {
        base.Start();
        canvas.gameObject.SetActive(false);
        for (int i = (int)KeyCode.A; i <= (int)KeyCode.Z; i++) {
            int index = i - (int)KeyCode.A;
            string alphabet = "abcdefghijklmnopqrstuvwxyz";
            validKeys.Add((KeyCode)i, alphabet[index].ToString());
        }
        for (int i = (int)KeyCode.Alpha0; i <= (int)KeyCode.Alpha9; i++) {
            int index = i - (int)KeyCode.Alpha0;
            string numbers = "0123456789";
            validKeys.Add((KeyCode)i, numbers[index].ToString());
        }
        validKeys.Add(KeyCode.Space, " ");
        Player.Instance.OnStateChange += Player_OnStateChange;
    }

    private void Player_OnStateChange(object sender, System.EventArgs e) {
        if (isBeingUsed && Player.Instance.GetState() == Player.State.Moving) {
            isBeingUsed = false;
        }
    }

    public override string ActionName() {
        return "use";
    }

    public override bool CanInteract() {
        return isFree && Player.Instance.GetState() == Player.State.Sitting;
    }

    public override void Interact() {
        if (!isTurnedOn) {
            isTurnedOn = true;
            canvas.gameObject.SetActive(true);
            StartCoroutine(StartBootingSequence());
        }
        isBeingUsed = true;
        Player.Instance.StartTyping();
        StopHighlight();
    }

    private IEnumerator StartBootingSequence() {
        AddConsoleText("BOOTING TTA NETWORK OPERATING SYSTEM...");
        yield return new WaitForSeconds(2.0f);
        Clear();
        AddConsoleText("TTA OS 2.1 IS READY.");
        yield return new WaitForSeconds(0.5f);
        AddConsoleText("");
        AddConsoleText("STARTING SECURE CONNECTION...");
        yield return new WaitForSeconds(1.5f);
        AddConsoleText("CONNECTED.");
        yield return new WaitForSeconds(0.5f);
        PromptAuthentication();
    }

    private void PromptAuthentication() {
        username = "";
        password = "";
        AddConsoleText("");
        AddConsoleText("USERNAME:");
        StartInput(false);
    }

    private void StartInput(bool startNewLine = true) {
        if (startNewLine) {
            AddConsoleText("");
        }
        AddConsoleText(prompt + "_");
        currentInput = "";
        isPendingInput = true;
    }

    public void Update() {
        if (isPendingInput && isBeingUsed) {
            bool pressedKey = false;
            if (Input.GetKeyDown(KeyCode.Backspace) && currentInput.Length > 0) {
                currentInput = currentInput.Substring(0, currentInput.Length - 1);
                pressedKey = true;
            }
            // cannot believe there's no better way to do this...
            foreach (KeyCode keyCode in validKeys.Keys) {
                if (Input.GetKeyDown(keyCode)) {
                    currentInput += validKeys[keyCode].ToUpper();
                    pressedKey = true;
                }
            }
            if (currentInput.Length > maxCols) {
                currentInput = currentInput.Substring(0, maxCols - 1);
            }

            bool pressedEnter = Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return);

            if (pressedEnter || pressedKey) {
                string finalInput = prompt + currentInput + (pressedEnter ? "" : "_");
                consoleLines[consoleLines.Count - 1] = finalInput;
                PrintFinalText();
            }

            if (pressedEnter && currentInput.Length > 0) {
                ValidateEntry();
            }

        }
    }

    private void ValidateEntry() {
        isPendingInput = false;
        if (!isAuthenticated) {
            if (username == "") {
                username = currentInput.Trim().ToUpper();
                AddConsoleText("PASSWORD:");
                StartInput(false);
            } else {
                password = currentInput.Trim().ToUpper();
                isAuthenticated = TryAuthenticate(username, password);
                if (isAuthenticated) {
                    AddConsoleText("");
                    AddConsoleText("WELCOME " + username.Split(' ')[0]);
                    StartInput();
                } else {
                    AddConsoleText("Invalid credentials. Please try again.");
                    PromptAuthentication();
                }
            }
        } else {
            bool isErrorCommand = false;
            if (currentInput.Trim().ToUpper() == "HELP") {
                StartCoroutine(SystemResponse(new List<string>(){
            "TTA 2.1 Command List",
            "--------------------",
            "inbox        - Lists recent emails",
            "mail #       - Reads email specified",
            "logout       - Log out of account",
            }));
            } else if (currentInput.Trim().ToUpper() == "INBOX") {
                List<string> inbox = new List<string>() {
                    "INBOX: " + authenticatedAccount.emails.Length.ToString()  + " emails:",
                    "---------------",
                };
                for (int i = 0; i < authenticatedAccount.emails.Length; i++) {
                    inbox.Add((i + 1).ToString() + ". " + authenticatedAccount.emails[i].topic);
                }
                StartCoroutine(SystemResponse(inbox));
            } else if (currentInput.Trim().ToUpper() == "LOGOUT") {
                Clear();
                AddConsoleText("YOU ARE NOW LOGGED OUT.");
                AddConsoleText("");
                isAuthenticated = false;
                PromptAuthentication();
            } else if (currentInput.Trim().ToUpper().StartsWith("MAIL ")) {
                string[] parts = currentInput.Trim().Split(" ");
                if (parts.Length == 2 && int.TryParse(parts[1], out int emailIndex)) {
                    if (emailIndex - 1 < authenticatedAccount.emails.Length) {
                        Email emailSO = authenticatedAccount.emails[emailIndex - 1];
                        List<string> email = new List<string>() {
                            "DATE: " + emailSO.date,
                            "FROM: " + emailSO.from,
                            "  TO: " + emailSO.to,
                            "SUBJ: " + emailSO.topic,
                            ""
                        };
                        foreach(string s in emailSO.body.Split("\n")) {
                            email.Add(s);
                        }
                        StartCoroutine(SystemResponse(email, emailSO.executeStep));
                    }  else {
                        StartCoroutine(SystemResponse(new List<string>(){
                            "Email index is out of bounds. Try again"
                        }));
                    }
                } else {
                    isErrorCommand = true;
                }
            } else {
                isErrorCommand = true;
            }

            if (isErrorCommand) {
                StartCoroutine(SystemResponse(new List<string>(){
                "Invalid Syntax.",
                "Type HELP for a list of commands.",
                }));
            }
        }


    }

    private IEnumerator SystemResponse(List<string> listing, GameLogic.GameStep executeStep = GameLogic.GameStep.None) {
        foreach (string line in listing) {
            AddConsoleText(line);
            yield return new WaitForSeconds(0.3f);
        }
        if (executeStep != GameLogic.GameStep.None) {
            GameLogic.Instance.SetStep(executeStep);
        }
        StartInput();
    }

    private bool TryAuthenticate(string username, string password) {
        foreach(ComputerAccountSO account in accounts) {
            if (username == account.username && password == account.password) {
                authenticatedAccount  = account;
                return true;
            }
        }
        return false;
    }

    private void AddConsoleText(string text) {
        consoleLines.Add(text.ToUpper());
        if (consoleLines.Count > maxRows) {
            List<string> newLines = new List<string>();
            for (int i = 1; i < consoleLines.Count; i++) {
                newLines.Add(consoleLines[i]);
            }
            consoleLines = newLines;
        }
        PrintFinalText();
    }

    private void PrintFinalText() {
        string finalText = "";
        foreach (string line in consoleLines) {
            finalText += line + "\n";
        }
        consoleText.GetComponent<TextMeshProUGUI>().text = finalText;
    }

    private void Clear() {
        consoleLines.Clear();
        consoleText.GetComponent<TextMeshProUGUI>().text = "";
    }

}
