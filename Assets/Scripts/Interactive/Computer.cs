using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Computer : InteractiveObject {


    public Transform canvas;
    public Transform consoleText;

    public bool isFree;
    private bool isTurnedOn;
    private bool isPendingInput = false;
    private bool isAuthenticated = false;

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
        Player.Instance.StartTyping();
        StopHighlight();
    }

    private IEnumerator StartBootingSequence() {
        AddConsoleText("Booting TTA Network Operating System...");
        yield return new WaitForSeconds(2.0f);
        Clear();
        AddConsoleText("TTA Network OS 2.1 Ready.");
        yield return new WaitForSeconds(0.5f);
        AddConsoleText("");
        AddConsoleText("Starting secure connection...");
        yield return new WaitForSeconds(1.5f);
        AddConsoleText("Connected.");
        yield return new WaitForSeconds(0.5f);
        PromptAuthentication();
    }

    private void PromptAuthentication() {
        username = "";
        password = "";
        AddConsoleText("");
        AddConsoleText("Username:");
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
        if (isPendingInput) {
            bool pressedKey = false;
            bool isShift = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
            if (Input.GetKeyDown(KeyCode.Backspace) && currentInput.Length > 0) {
                currentInput = currentInput.Substring(0, currentInput.Length - 1);
                pressedKey = true;
            }
            // cannot believe there's no better way to do this...
            foreach (KeyCode keyCode in validKeys.Keys) {
                if (Input.GetKeyDown(keyCode)) {
                    currentInput += (isShift ? validKeys[keyCode].ToUpper() : validKeys[keyCode].ToLower());
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
                AddConsoleText("Password:");
                StartInput(false);
            } else {
                password = currentInput.Trim().ToUpper();
                isAuthenticated = TryAuthenticate(username, password);
                if (isAuthenticated) {
                    AddConsoleText("Welcome " + username);
                    StartInput();
                } else {
                    AddConsoleText("Invalid credentials. Please try again.");
                    PromptAuthentication();
                }
            }
        } else {
            if (currentInput.Trim().ToUpper() == "HELP") {
                StartCoroutine(SystemResponse(new List<string>(){
            "TTA 2.1 Command List",
            "--------------------",
            "mail list    - Lists recent emails",
            "read #       - Reads email specified",
            "quit         - Exits TTA",
            }));
            } else {
                StartCoroutine(SystemResponse(new List<string>(){
            "Invalid Syntax.",
            "Type HELP for a list of commands.",
            }));
            }
        }


    }

    private IEnumerator SystemResponse(List<string> listing) {
        foreach (string line in listing) {
            AddConsoleText(line);
            yield return new WaitForSeconds(0.3f);
        }
        StartInput();
    }

    private bool TryAuthenticate(string username, string password) {
        return (username == "AAA" && password == "123");
    }

    private void AddConsoleText(string text) {
        consoleLines.Add(text);
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
