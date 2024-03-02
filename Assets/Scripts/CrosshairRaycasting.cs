using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairRaycasting : MonoBehaviour {

    public float interactionDistance = 5f;
    public Transform playerCamera;
    public LayerMask ignoredLayer;

    private InteractiveObject currentInteractiveComponent;

    private void Update() {
        RaycastHit hit;
        Debug.DrawRay(playerCamera.transform.position, playerCamera.transform.forward * interactionDistance, Color.yellow);
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, interactionDistance, ~ignoredLayer)) {
            if (hit.transform.TryGetComponent(out InteractiveObject interactiveComponent)) {
                if (currentInteractiveComponent != interactiveComponent) {
                    if (currentInteractiveComponent != null) {
                        currentInteractiveComponent.StopHighlight();
                    }
                    currentInteractiveComponent = interactiveComponent;
                    interactiveComponent.Highlight();
                }
            } else if (currentInteractiveComponent != null) {
                currentInteractiveComponent.StopHighlight();
                currentInteractiveComponent = null;
            }
        } else if (currentInteractiveComponent != null) {
            currentInteractiveComponent.StopHighlight();
            currentInteractiveComponent = null;
        }

        if (Input.GetMouseButtonDown(0) && currentInteractiveComponent != null) {
            currentInteractiveComponent.Interact();
        }
    }


}