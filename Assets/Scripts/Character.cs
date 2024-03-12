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
    public bool randomizeColors;

    public Transform[] skinMeshes;
    public Transform[] shirtMeshes;
    public Transform[] eyeMeshes;
    public Transform[] pantsMeshes;
    public Transform hairMesh;
    public Transform[] shoeMeshes;

    public Material[] skinColors;
    public Material[] hairColors;
    public Material[] eyeColors;
    public Material[] shoeColors;
    public Material[] clothesColors;

    private float timeStartTalk = float.NegativeInfinity;
    private bool isTalking = false;
   

    void Start() {
        StartCoroutine(InitializeAnimation());
        if (randomizeColors) {
            Material shirtColor = clothesColors[Random.Range(0, clothesColors.Length)];
            Material skinColor = skinColors[Random.Range(0, skinColors.Length)];
            Material eyeColor = eyeColors[Random.Range(0, eyeColors.Length)];
            Material pantsColor = clothesColors[Random.Range(0, clothesColors.Length)];
            Material shoesColor = shoeColors[Random.Range(0, shoeColors.Length)];
            Material hairColor = hairColors[Random.Range(0, hairColors.Length)];

            foreach (Transform t in shirtMeshes) {
                t.GetComponent<MeshRenderer>().material = shirtColor;
            }
            foreach (Transform t in skinMeshes) {
                t.GetComponent<MeshRenderer>().material = skinColor;
            }
            foreach (Transform t in eyeMeshes) {
                t.GetComponent<MeshRenderer>().material = eyeColor;
            }
            foreach (Transform t in pantsMeshes) {
                t.GetComponent<MeshRenderer>().material = pantsColor;
            }
            foreach (Transform t in shoeMeshes) {
                t.GetComponent<MeshRenderer>().material = shoesColor;
            }
            hairMesh.GetComponent<MeshRenderer>().material = hairColor;
        }
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
