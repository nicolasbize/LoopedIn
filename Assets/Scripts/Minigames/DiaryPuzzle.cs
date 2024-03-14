using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiaryPuzzle : MonoBehaviour
{

    public Transform diaryBackground;
    public Transform letterPrefab;
    public Transform newLetterInput;

    [TextArea] public string leftPage;
    [TextArea] public string rightPage;

    private List<HighlightableLetter> letters = new List<HighlightableLetter>();

    private Dictionary<KeyCode, char> validKeys = new Dictionary<KeyCode, char>();
    private Dictionary<char, char> encoding = new Dictionary<char, char>();
    private Dictionary<char, bool> lettersFound = new Dictionary<char, bool>();

    public event EventHandler OnSolutionFound;

    private void Start() {
        string alphabet = "abcdefghijklmnopqrstuvwxyz";
        for (int i = (int)KeyCode.A; i <= (int)KeyCode.Z; i++) {
            int index = i - (int)KeyCode.A;
            validKeys.Add((KeyCode)i, alphabet[index]);
        }

        string encodedAlphabet = "jgqcoifzytukwarvpsxmbnheld";
        for (int i = 0; i < alphabet.Length; i++) {
            encoding.Add(alphabet[i], encodedAlphabet[i]);
        }
        
        PlaceLetters(leftPage, 180, -80);
        PlaceLetters(rightPage, Screen.width / 2 + 80, -80);
    }

    private void PlaceLetters(string page, int leftOffset, int topOffset) {
        string[] hintPhrases = page.Split('\n');
        for (int i = 0; i < hintPhrases.Length; i++) {
            string phrase = hintPhrases[i];
            for (int j = 0; j < phrase.Length; j++) {
                Transform letterObj = Instantiate(letterPrefab, diaryBackground);
                HighlightableLetter letter = letterObj.GetComponent<HighlightableLetter>();
                letter.isFullCasing = true;
                letter.isSelectable = false;
                letter.SetOriginalLetter(phrase[j]); // this will set the original letter
                if (encoding.ContainsKey(char.ToLower(phrase[j]))) { // only cover what can be decoded
                    bool isUpper = char.ToUpper(phrase[j]) == phrase[j];
                    char encoded = encoding[char.ToLower(phrase[j])];
                    lettersFound.TryAdd(char.ToLower(phrase[j]), false);
                    letter.SetLetter(isUpper ? char.ToUpper(encoded) : encoded);
                    letter.OnLetterHighlight += OnLetterHighlight;
                    letter.OnLetterRemoveHighlight += OnLetterRemoveHighlight;
                } else {
                    letter.SetLetter(phrase[j]);
                }
                letterObj.GetComponent<RectTransform>().localPosition = new Vector3(leftOffset + 22 * j, topOffset - 42 * i, 0f);
                letterObj.GetComponent<RectTransform>().anchorMin = new Vector2(0, 1);
                letterObj.GetComponent<RectTransform>().anchorMax = new Vector2(0, 1);
                letterObj.GetComponent<RectTransform>().pivot = new Vector2(0, 1);
                letters.Add(letter);
            }
        }
    }

    private void Update() {
        char newChar = char.MinValue;
        // cannot believe there's no better way to do this...
        foreach (KeyCode keyCode in validKeys.Keys) {
            if (newChar == char.MinValue && Input.GetKeyDown(keyCode)) {
                newChar = validKeys[keyCode];
            }
        }
        if (newChar != char.MinValue) {
            ReplaceHighlightedLetters(newChar);
            CheckForSolution();
            newChar = char.MinValue;
        }
    }

    private void CheckForSolution() {
        bool completed = true;
        foreach(char c in lettersFound.Keys) {
            if (!lettersFound[c]) {
                completed = false; break;
            }
        }
        if (completed) {
            OnSolutionFound?.Invoke(this, EventArgs.Empty);
        }
    }

    private void ReplaceHighlightedLetters(char newChar) {
        foreach (HighlightableLetter letter in letters) {
            if (letter.IsHighlighted()) {
                bool isUpper = char.ToUpper(letter.GetLetter()).Equals(letter.GetLetter());
                letter.SetLetter(isUpper ? char.ToUpper(newChar) : newChar);
                letter.RemoveHighlight();
                if (char.ToUpper(letter.GetLetter()) == char.ToUpper(letter.GetOriginalLetter())) {
                    letter.PreventHighlighting();
                    letter.MarkAsGreen();
                    lettersFound[char.ToLower(letter.GetLetter())] = true;
                }
            }
        }
    }

    private void OnLetterRemoveHighlight(object sender, HighlightableLetter.OnLetterEventArgs e) {
        foreach (HighlightableLetter letter in letters) {
            if (letter.GetOriginalLetter() == e.originalValue) {
                letter.RemoveHighlight();
            }
        }
    }

    private void OnLetterHighlight(object sender, HighlightableLetter.OnLetterEventArgs e) {
        // highlight other letters
        foreach (HighlightableLetter letter in letters) {
            if (letter.GetOriginalLetter() == e.originalValue) {
                letter.AddHighlight();
            } else {
                letter.RemoveHighlight();
            }
        }
    }

}
