using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroScreen : MonoBehaviour
{
    public event EventHandler OnIntroComplete;

    public Transform[] introPages;
    public TypedText[] introTexts;

    private int currentPageIndex = -1;

    void Start()
    {
        foreach (Transform t in introPages) {
            t.gameObject.SetActive(false);
        }
        foreach (TypedText t in introTexts) {
            t.OnComplete += Text_OnComplete;
            t.OnType += Text_OnType;
        }

        currentPageIndex = introPages.Length;
        ShowNextPage();
    }

    private void Text_OnComplete(object sender, System.EventArgs e) {
        Destroy(introPages[currentPageIndex].gameObject);
        ShowNextPage();
    }

    private void Text_OnType(object sender, System.EventArgs e) {
        GetComponent<AudioSource>().pitch = UnityEngine.Random.Range(0.7f, 1.3f);
        GetComponent<AudioSource>().Play();
    }

    private void ShowNextPage() {
        currentPageIndex -= 1;
        if (currentPageIndex > -1) {
            introPages[currentPageIndex].gameObject.SetActive(true);
            introTexts[currentPageIndex].StartTyping();
        } else {
            OnIntroComplete?.Invoke(this, EventArgs.Empty);
        }
    }

}
