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
    private Vector3 endPosition = new Vector3(0, 2.71f, 0.8f);
    private bool hasReadText = false;

    private void Start() {
        transform.localPosition = startPosition;

        Player.Instance.OnStateChange += Player_OnStateChange;
    }


    public void Activate() {
        StartCoroutine(LiftUpArm());
    }

    IEnumerator LiftUpArm() {
        yield return new WaitForSeconds(1f);
        GetComponent<AudioSource>().Play();
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
                StartGame();
            }
        }
    }

    IEnumerator ReadAndThink() {
        yield return new WaitForSeconds(4f);
        Player.Instance.StartThinking("Oh no, I gotta get going quick!");
        hasReadText = true;
    }

    private void Player_OnStateChange(object sender, System.EventArgs e) {
        if (hasReadText) {
            isDropping = true;
            timeStart = Time.timeSinceLevelLoad;
        }
    }

    private void StartGame() {
        if (gameObject.activeSelf) {
            MenuManager.Instance.StartGame();
            GameLogic.Instance.StartMusic();
            gameObject.SetActive(false);
        }
    }

}
