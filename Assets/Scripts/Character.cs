using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : ConversationalObject {
    public bool isSitting;
    public Transform humanoid;
    public DialogSO dialog;
    public Transform renderCamera;

    private float timeStartTalk = float.NegativeInfinity;
    private bool isTalking = false;

    void Start() {
        StartCoroutine(InitializeAnimation());
    }

    IEnumerator InitializeAnimation() {
        yield return new WaitForSeconds(Random.Range(0.0f, 1.0f));
        humanoid.GetComponent<Animator>().SetBool("IsSitting", isSitting);
    }

    public override void Talk() {
        renderCamera.GetComponent<Camera>().enabled = true;
        timeStartTalk = Time.timeSinceLevelLoad;
        isTalking = true;
        Player.Instance.StartTalking(this);
    }

    private void Update() {
        // this just takes a picture for the conversation dialog
        if (isTalking && (Time.timeSinceLevelLoad - timeStartTalk > 0.1f)) {
            renderCamera.GetComponent<Camera>().enabled = false;
        }
    }

}
