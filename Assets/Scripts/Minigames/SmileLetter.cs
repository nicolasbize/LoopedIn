using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmileLetter : MonoBehaviour
{

    public Transform letterBackground;
    public Transform letterPrefab;
    [TextArea] public string hint;
    public Transform resetButton;

    private List<HighlightableLetter> letters = new List<HighlightableLetter>();

    public event EventHandler OnSolutionFound;

    private void Start() {
        string[] hintPhrases = hint.Split('\n');
        for (int i = 0; i < hintPhrases.Length; i++) {
            string phrase = hintPhrases[i];
            for(int j = 0; j < phrase.Length; j++) {
                Transform letterObj = Instantiate(letterPrefab, letterBackground);
                HighlightableLetter letter = letterObj.GetComponent<HighlightableLetter>();
                letter.SetLetter(phrase[j]);
                letterObj.GetComponent<RectTransform>().localPosition = new Vector3(20 + 40 * j, -20 - 60 * i, 0f);
                letterObj.GetComponent<RectTransform>().anchorMin = new Vector2(0, 1);
                letterObj.GetComponent<RectTransform>().anchorMax = new Vector2(0, 1);
                letterObj.GetComponent<RectTransform>().pivot = new Vector2(0, 1);
                letters.Add(letter);
                letter.OnLetterSelect += Letter_OnLetterSelect;
            }
        }
        resetButton.GetComponent<ResetButton>().OnResetClick += SmileLetter_OnResetClick;
    }

    private void Letter_OnLetterSelect(object sender, System.EventArgs e) {
        if (CheckForSolution()) {
            OnSolutionFound?.Invoke(this, EventArgs.Empty);
        }
    }

    private bool CheckForSolution() {
        bool foundSolution = true;
        foreach (var letter in letters) {
            if (!letter.isCorrect && letter.IsSelected()) {
                foundSolution = false;
            }
            if (letter.isCorrect && !letter.IsSelected()) {
                foundSolution = false;
            }
        }
        return foundSolution;
    }

    private void SmileLetter_OnResetClick(object sender, System.EventArgs e) {
        foreach(HighlightableLetter letter in letters) {
            letter.SetInactive();
        }
    }
}
