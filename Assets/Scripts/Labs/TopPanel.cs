using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopPanel : MonoBehaviour
{

    public Computer[] computersForOverride;
    public MeshRenderer[] lightMeshes;
    public Button[] buttonsToEnable;
    public Material disabledMaterial;
    public Material enabledMaterial;
    private HashSet<string> overridesEnabled = new HashSet<string>();
    private int nbOverrides = 0;

    private void Start() {
        foreach (Computer computer in computersForOverride) {
            computer.OnInjectionOverride += Computer_OnInjectionOverride;
        }
    }

    private void Computer_OnInjectionOverride(object sender, Computer.OnInjectionOverrideEventArgs e) {
        if (!overridesEnabled.Contains(e.name)) {
            overridesEnabled.Add(e.name);
            nbOverrides += 1;
            RefreshIndicators();
        }
    }

    private void RefreshIndicators() {
        for (int i = 0; i < nbOverrides; i++) {
            lightMeshes[i].material = enabledMaterial;
        }
        if (nbOverrides == computersForOverride.Length) {
            foreach (Button button in buttonsToEnable) {
                button.Enable();
            }
        }
    }
}
