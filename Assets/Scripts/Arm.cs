using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arm : MonoBehaviour
{

    private bool isLifting = false;
    private bool isDropping = false;
    private float timeStart = float.NegativeInfinity;
    private float duration = 0.2f;
    private Vector3 startPosition = new Vector3(0, 1.7f, 0.8f);
    private Vector3 endPosition = new Vector3(0, 2.67f, 0.8f);

    private void Start() {
        transform.localPosition = startPosition;
    }

    public void Activate() {
        // ring
        // await 2 secs
        Debug.Log("activating arm");
        StartCoroutine(LiftUpArm());
    }

    IEnumerator LiftUpArm() {
        yield return new WaitForSeconds(2f);
        isLifting = true;
        timeStart = Time.timeSinceLevelLoad;
    }

    private void Update() {
        if (isLifting) {
            float progress = (Time.timeSinceLevelLoad - timeStart) / duration;
            if (progress < 1f) {
                transform.localPosition = Vector3.Lerp(startPosition, endPosition, progress);
            } else {
                isLifting = false;
                StartCoroutine(ReadAndThink());
            }
        }

        if (isDropping) {
            float progress = (Time.timeSinceLevelLoad - timeStart) / duration;
            if (progress < 1f) {
                transform.localPosition = Vector3.Lerp(endPosition, startPosition, progress);
            } else {
                isDropping = false;
                
            }
        }
    }

    IEnumerator ReadAndThink() {
        yield return new WaitForSeconds(4f);
        Player.Instance.StartThinking("Oh no, I gotta get going quick!");
        yield return new WaitForSeconds(2f);
        isDropping = true;
        timeStart = Time.timeSinceLevelLoad;
        MenuManager.Instance.StartGame();
        gameObject.SetActive(false);
    }
}
