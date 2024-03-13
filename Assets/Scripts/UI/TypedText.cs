using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class TypedText : MonoBehaviour
{
    public event EventHandler OnType;
    public event EventHandler OnComplete;

    private string fullText;
    private int indexText;
    private float timeSinceLastChar = float.NegativeInfinity;
    private float timeSinceStart = float.NegativeInfinity;
    private float timeBetweenChars = 0.1f;
    private bool isTyping = false;
    private bool clickedToSpeed = false;
    private bool isDone = false;

    private void Start()
    {
        fullText = GetComponent<TextMeshProUGUI>().text;
        GetComponent<TextMeshProUGUI>().text = "";
        StartTyping();
    }

    public void StartTyping() {
        isTyping = true;
        timeSinceLastChar = Time.timeSinceLevelLoad;
        timeSinceStart = Time.timeSinceLevelLoad;
    }

    private void Update() {
        if (isTyping) {
            if (Time.timeSinceLevelLoad - timeSinceLastChar > timeBetweenChars) {
                indexText += 1;
                if (indexText < fullText.Length) {
                    GetComponent<TextMeshProUGUI>().text = fullText.Truncate(indexText);
                    timeSinceLastChar = Time.timeSinceLevelLoad;
                    timeBetweenChars = clickedToSpeed ? 0.01f : UnityEngine.Random.value / 10f;
                    OnType?.Invoke(this, EventArgs.Empty);
                } else {
                    isTyping = false;
                    isDone = true;
                }

            }
        }
        if (Time.timeSinceLevelLoad - timeSinceStart > 1f && Input.GetMouseButton(0)) {
            clickedToSpeed = true;
        }
        if (isDone && Input.GetMouseButton(0)) {
            OnComplete?.Invoke(this, EventArgs.Empty);
        }
    }


}
