using System;
using Unity.VisualScripting;
using UnityEngine;

public class LiquidDispenser : MonoBehaviour
{
    public Transform mesh;
    public Material enabledMaterial;
    public Button vialButton;

    private Material disabledMaterial;

    private void Start() {
        disabledMaterial = mesh.GetComponent<MeshRenderer>().material;
        vialButton.OnButtonPress += VialButton_OnButtonPress;
    }

    private void VialButton_OnButtonPress(object sender, Button.OnButtonPressEventArgs e) {
        GetComponent<AudioSource>().Play();
    }

    public void Enable() {
        mesh.GetComponent<MeshRenderer>().material = enabledMaterial;
    }

    public void Disable() {
        mesh.GetComponent<MeshRenderer>().material = disabledMaterial;
    }


}
