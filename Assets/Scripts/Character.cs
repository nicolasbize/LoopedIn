using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Character : ConversationalObject {
    public bool isSitting;
    public Transform humanoid;
    public DialogSO dialog;
    public Transform renderCamera;
    public Transform dialogCanvas;
    public Transform thoughtText;

    private float timeStartTalk = float.NegativeInfinity;
    private bool isTalking = false;
   

    void Start() {
        StartCoroutine(InitializeAnimation());
    }

    IEnumerator InitializeAnimation() {
        yield return new WaitForSeconds(Random.Range(0.0f, 1.0f));
        if (isSitting) {
            humanoid.GetComponent<Animator>().SetBool("IsSitting", isSitting);
        }
    }

    // back and forth exchange with player
    public override void Talk() {
        renderCamera.GetComponent<Camera>().enabled = true;
        timeStartTalk = Time.timeSinceLevelLoad;
        isTalking = true;
        Player.Instance.StartTalking(this);
    }

    public void ThinkOutLoud(string thought) {
        thoughtText.GetComponent<TextMeshProUGUI>().text = thought;
        dialogCanvas.gameObject.SetActive(true);
    }

    public void StopThinkingOutLoud() {
        dialogCanvas.gameObject.SetActive(false);
    }

    private void Update() {
        // this just takes a picture for the conversation dialog
        if (isTalking && (Time.timeSinceLevelLoad - timeStartTalk > 0.1f)) {
            renderCamera.GetComponent<Camera>().enabled = false;
        }
    }

}
