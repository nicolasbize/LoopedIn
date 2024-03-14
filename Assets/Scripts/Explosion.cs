using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Explosion : MonoBehaviour
{
    private float timeToExplode = 5f;
    private float timeStart = float.NegativeInfinity;
    private bool isExploding = false;
    private Vector3 originalPosition;
    private Vector3 targetDestination;

    public void Explode() {
        originalPosition = transform.position;
        targetDestination = Player.Instance.transform.position;
        targetDestination = new Vector3(targetDestination.x, originalPosition.y, targetDestination.z);
        targetDestination = originalPosition + (targetDestination - originalPosition) * 0.9f;
        GetComponent<VisualEffect>().gameObject.SetActive(true);
        timeStart = Time.timeSinceLevelLoad;
        isExploding = true;
        GetComponent<AudioSource>().Play();
        HintManager.Instance.Hide();
    }

    private void Update() {
        if (isExploding) {
            float progress = (Time.timeSinceLevelLoad - timeStart) / timeToExplode;
            if (progress < 1f) {
                transform.position = Vector3.Lerp(originalPosition, targetDestination, progress);
                GetComponent<VisualEffect>().SetFloat("Size", progress * 10f);
            } else {
                isExploding = false;
                RestartLoop();
            }
        }
    }

    private void RestartLoop() {
        MenuManager.Instance.TransitionToRestart();
    }

}
