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


    private void Start() {
        GetComponent<Image>().sprite = defaultImage;
    }

    public void OnPointerClick(PointerEventData eventData) {
        OnClick?.Invoke(this, EventArgs.Empty);
    }

    public void OnPointerEnter(PointerEventData eventData) {
        GetComponent<Image>().sprite = hoverImage;

    }

    public void OnPointerExit(PointerEventData eventData) {
        GetComponent<Image>().sprite = defaultImage;
    }
}
