using System;
using Unity.VisualScripting;
using UnityEngine;

public class LiquidDispenser : MonoBehaviour
{
    public Transform mesh;
    public Material enabledMaterial;

    private Material disabledMaterial;

    private void Start() {
        disabledMaterial = mesh.GetComponent<MeshRenderer>().material;
    }

    public void Enable() {
        mesh.GetComponent<MeshRenderer>().material = enabledMaterial;
    }

    public void Disable() {
        mesh.GetComponent<MeshRenderer>().material = disabledMaterial;
    }


}
