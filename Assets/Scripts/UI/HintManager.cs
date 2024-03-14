using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HintManager : MonoBehaviour
{
    public Transform hintText;
    public Transform hintContainer;

    public static HintManager Instance;

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        hintContainer.gameObject.SetActive(false);
    }

    public void Show() {
        if (!hintContainer.gameObject.activeSelf) {
            hintContainer.gameObject.SetActive(true);
        }
    }

    public void Hide() {
        if (hintContainer.gameObject.activeSelf) {
            hintContainer.gameObject.SetActive(false);
        }
    }


    public void UpdateHint(string hint) {
        hintText.GetComponent<TextMeshProUGUI>().text = hint;
    }

}
