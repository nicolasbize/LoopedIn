using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class CrosshairRaycasting : MonoBehaviour {

    public float interactionDistance = 5f;
    public Transform playerCamera;
    public LayerMask ignoredLayer;
    public Transform actionBackground;
    public Transform actionText;

    private InteractiveObject currentInteractiveComponent;
    private ConversationalObject currentConversationalObject;

    private void Update() {
        Debug.DrawRay(playerCamera.transform.position, playerCamera.transform.forward * interactionDistance, Color.yellow);
        CheckForInteractiveElement();
        CheckForConversationalObject();

        if (Input.GetKeyDown(KeyCode.E) && currentInteractiveComponent != null) {
            currentInteractiveComponent.Interact();
        } else if (Input.GetKeyDown(KeyCode.E) && currentConversationalObject != null) {
            currentConversationalObject.Talk();
            Debug.Log("Talk");
        }

        actionBackground.gameObject.SetActive(IsObjectInFocus());
    }

    private bool IsObjectInFocus() {
        if (currentInteractiveComponent != null) {
            return true;
        }
        if (currentConversationalObject != null) {
            return true;
        }
        return false;
    }

    private void CheckForConversationalObject() {
        RaycastHit hit;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, interactionDistance, ~ignoredLayer)) {
            if (hit.transform.TryGetComponent(out ConversationalObject conversationalComponent)) {
                if (currentConversationalObject != conversationalComponent) {
                    if (currentConversationalObject != null) {
                    }
                    currentConversationalObject = conversationalComponent;
                    actionText.GetComponent<TextMeshProUGUI>().text = "E to Talk";
                }
            } else if (currentConversationalObject != null) {
                currentConversationalObject = null;
            }
        } else if (currentConversationalObject != null) {
            currentConversationalObject = null;
        }

    }

    private void CheckForInteractiveElement() {
        RaycastHit hit;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, interactionDistance, ~ignoredLayer)) {
            if (hit.transform.TryGetComponent(out InteractiveObject interactiveComponent)) {
                if (currentInteractiveComponent != interactiveComponent) {
                    if (currentInteractiveComponent != null) {
                        currentInteractiveComponent.StopHighlight();
                    }
                    currentInteractiveComponent = interactiveComponent;
                    interactiveComponent.Highlight();
                    actionText.GetComponent<TextMeshProUGUI>().text = "E to Interact";
                }
            } else if (currentInteractiveComponent != null) {
                currentInteractiveComponent.StopHighlight();
                currentInteractiveComponent = null;
            }
        } else if (currentInteractiveComponent != null) {
            currentInteractiveComponent.StopHighlight();
            currentInteractiveComponent = null;
        }
    }
}