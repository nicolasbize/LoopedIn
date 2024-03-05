using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class HighlightableLetter : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler {

    public Transform highlightImage;
    public Transform selectedImage;
    public Transform text;
    private string letter;
    private bool isHighlightable = false;
    public bool isCorrect = false;

    public event EventHandler OnLetterSelect;

    public void OnPointerClick(PointerEventData eventData) {
        if (isHighlightable) {
            bool wasActive = selectedImage.gameObject.activeSelf;
            selectedImage.gameObject.SetActive(!wasActive);
            OnLetterSelect?.Invoke(this, EventArgs.Empty);
        }
    }

    public void SetInactive() {
        selectedImage.gameObject.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData) {
        if (isHighlightable) {
            highlightImage.gameObject.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData) {
        if (isHighlightable) {
            highlightImage.gameObject.SetActive(false);
        }
    }

    private void Start() {
        highlightImage.gameObject.SetActive(false);
        selectedImage.gameObject.SetActive(false);
    }

    public string GetLetter() {
        return letter;
    }

    public void SetLetter(string letter) {
        isHighlightable = Char.IsLetter(letter[0]);
        isCorrect = isHighlightable && (letter.ToUpper() == letter);
        this.letter = letter;
        if (isCorrect) { 
            Debug.Log("setting letter as correct: " + letter);
        }
        text.GetComponent<TextMeshProUGUI>().text = letter.ToUpper();
    }

    public bool IsSelected() {
        return selectedImage.gameObject.activeSelf;
    }


}
