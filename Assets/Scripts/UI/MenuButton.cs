using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler {
    public event EventHandler OnClick;

    public Sprite defaultImage;
    public Sprite hoverImage;
    public AudioClip hoverAudioClip;
    public AudioClip pressedAudioClip;

    private void Start() {
        GetComponent<Image>().sprite = defaultImage;
    }

    public void OnPointerClick(PointerEventData eventData) {
        GetComponent<AudioSource>().clip = pressedAudioClip;
        GetComponent<AudioSource>().Play();
        StartCoroutine(CallListeners());
    }

    IEnumerator CallListeners() {
        // wait for the click sound before making this gameobject inactive
        yield return new WaitForSeconds(0.3f);
        OnClick?.Invoke(this, EventArgs.Empty);
    }

    public void OnPointerEnter(PointerEventData eventData) {
        GetComponent<Image>().sprite = hoverImage;
        GetComponent<AudioSource>().clip = hoverAudioClip;
        GetComponent<AudioSource>().Play();
    }

    public void OnPointerExit(PointerEventData eventData) {
        GetComponent<Image>().sprite = defaultImage;
    }
}
