using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Core : MonoBehaviour
{

    public Explosion explosion;
    public AudioClip warningAudio;
    public AudioClip triggerAudio;
    public LabsDoor labsDoor;
    public VialPuzzle vialPuzzle;

    private void Start() {
        labsDoor.OnClose += LabsDoor_OnClose;
        vialPuzzle.OnLaunch += VialPuzzle_OnLaunch;
    }

    private void VialPuzzle_OnLaunch(object sender, System.EventArgs e) {
        GetComponent<AudioSource>().clip = triggerAudio;
        GetComponent<AudioSource>().loop = false;
        GetComponent<AudioSource>().Play();
        StartCoroutine(CompleteDeath());

    }

    IEnumerator CompleteDeath() {
        yield return new WaitForSeconds(9f);
        explosion.Explode();
    }

    private void LabsDoor_OnClose(object sender, System.EventArgs e) {
        if (!GetComponent<AudioSource>().isPlaying) {
            GetComponent<AudioSource>().loop = true;
            GetComponent<AudioSource>().clip = warningAudio;
            GetComponent<AudioSource>().Play();
        }
    }


}
