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
    public bool isSelectable;
    public bool isFullCasing;

    private char letter = Char.MinValue;
    private char originalLetter = Char.MinValue;
    private bool isHighlightable = false;
    public bool isCorrect = false;

    public event EventHandler OnLetterSelect;


    public class OnLetterEventArgs : EventArgs {
        public char originalValue;
        public char value;
    }
    public event EventHandler<OnLetterEventArgs> OnLetterHighlight;
    public event EventHandler<OnLetterEventArgs> OnLetterRemoveHighlight;

    public void OnPointerClick(PointerEventData eventData) {
        if (isHighlightable) {
            bool wasActive = selectedImage.gameObject.activeSelf;
            if (isSelectable) {
                selectedImage.gameObject.SetActive(!wasActive);
            }
            OnLetterSelect?.Invoke(this, EventArgs.Empty);
        }
    }

    public void SetInactive() {
        selectedImage.gameObject.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData) {
        if (isHighlightable) {
            highlightImage.gameObject.SetActive(true);
            OnLetterHighlight?.Invoke(this, new OnLetterEventArgs() {
                value = letter,
                originalValue = originalLetter,
            });
        }
    }

    public void OnPointerExit(PointerEventData eventData) {
        if (isHighlightable) {
            highlightImage.gameObject.SetActive(false);
            OnLetterRemoveHighlight?.Invoke(this, new OnLetterEventArgs() {
                value = letter,
                originalValue = originalLetter,
            });
        }
    }

    private void Start() {
        highlightImage.gameObject.SetActive(false);
        selectedImage.gameObject.SetActive(false);
    }

    public bool IsHighlighted() {
        return highlightImage.gameObject.activeSelf;
    }

    public void RemoveHighlight() {
        highlightImage.gameObject.SetActive(false);
    }

    public void AddHighlight() {
        highlightImage.gameObject.SetActive(true);
    }

    public void PreventHighlighting() {
        isHighlightable = false;
    }

    public void MarkAsGreen() {
        ColorUtility.TryParseHtmlString("#078007", out Color color);
        text.GetComponent<TextMeshProUGUI>().color = color;
    }

    public char GetLetter() {
        return letter;
    }

    public char GetOriginalLetter() {
        return originalLetter;
    }

    public void SetOriginalLetter(char letter) {
        originalLetter = char.ToLower(letter);
    }

    public void SetLetter(char letter) {
        isHighlightable = Char.IsLetter(letter);
        // this is a special case for the smile puzzle:
        isCorrect = isHighlightable && (char.ToUpper(letter) == letter);
        this.letter = letter;
        text.GetComponent<TextMeshProUGUI>().text = (isFullCasing ? letter : char.ToUpper(letter)).ToString();
    }

    public bool IsSelected() {
        return selectedImage.gameObject.activeSelf;
    }


}
