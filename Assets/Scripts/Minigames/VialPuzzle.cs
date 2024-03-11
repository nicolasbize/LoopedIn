using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class VialPuzzle : MonoBehaviour
{
    public Transform rootLiquid;
    public LiquidDispenser[] liquidDispensers;
    public Button[] addLiquidButtons;
    public Button resetButton;
    public Button administerButton;
    public Transform liquidPrefab;

    private List<VialLiquid> currentLiquids = new List<VialLiquid>();
    
    private float vialStartY = -1.75f;
    private float vialEndY = 2f;
    private float timeToRise = 3f;
    private float timeStartRising = float.NegativeInfinity;
    private bool isRising = false;
    private bool isInjecting = false;

    private void Start() {
        transform.localPosition = new Vector3(transform.localPosition.x, vialStartY, transform.position.z);
        foreach (Button button in addLiquidButtons) {
            button.OnButtonPress += OnAddLiquidButtonPress;
        }
        resetButton.OnButtonPress += ResetButton_OnButtonPress;
        administerButton.OnButtonPress += AdministerButton_OnButtonPress;
        StartPuzzle();
    }

    private void AdministerButton_OnButtonPress(object sender, Button.OnButtonPressEventArgs e) {
        Launch();
    }

    private void ResetButton_OnButtonPress(object sender, Button.OnButtonPressEventArgs e) {
        ResetLiquids();
    }

    public void StartPuzzle() {
        isRising = true;
        timeStartRising = Time.timeSinceLevelLoad;
    }

    private void Update() {
        if (isRising) {
            float progress = (Time.timeSinceLevelLoad - timeStartRising) / timeToRise;
            if (progress < 1) {
                float y = Mathf.Lerp(vialStartY, vialEndY, progress);
                transform.localPosition = new Vector3(transform.localPosition.x, y, transform.position.z);
            } else {
                isRising = false;
                EnableControls();
            }
        }

        if (isInjecting) {
            float progress = (Time.timeSinceLevelLoad - timeStartRising) / timeToRise;
            if (progress < 1) {
                float y = Mathf.Lerp(vialEndY, vialStartY, progress);
                transform.localPosition = new Vector3(transform.localPosition.x, y, transform.position.z);
            } else {
                isInjecting = false;
                CompletePuzzle();
            }
        }
    }

    private void EnableControls() {
        isRising = false;
        foreach (Button button in addLiquidButtons) {
            button.Enable();
        }
        foreach (LiquidDispenser liquidDispenser in liquidDispensers) {
            liquidDispenser.Enable();
        }
        resetButton.Enable();
        ResetLiquids();
    }

    private void Launch() {
        isInjecting = true;
        timeStartRising = Time.timeSinceLevelLoad;
        foreach (Button button in addLiquidButtons) {
            button.Disable();
        }
        foreach (LiquidDispenser liquidDispenser in liquidDispensers) {
            liquidDispenser.Disable();
        }
        administerButton.Disable();
        resetButton.Disable();

    }


    private void ResetLiquids() {
        foreach (VialLiquid t in currentLiquids) {
            Destroy(t.gameObject);
        }
        currentLiquids = new List<VialLiquid>();
        AddLiquid(VialLiquid.Type.Yellow);
        AddLiquid(VialLiquid.Type.Yellow);
    }

    private void OnAddLiquidButtonPress(object sender, Button.OnButtonPressEventArgs e) {
        AddLiquid(VialLiquid.LiquidTypeDictionary[e.value]);
    }

    private void AddLiquid(VialLiquid.Type type) {
        Transform obj = Instantiate(liquidPrefab, rootLiquid);
        VialLiquid liquid = obj.GetComponent<VialLiquid>();
        liquid.GetComponent<VialLiquid>().SetType(type);
        currentLiquids.Add(liquid);
        ReorderLiquids();
    }

    private void ReorderLiquids() {
        List<VialLiquid> blues = currentLiquids.FindAll(l => { return l.type == VialLiquid.Type.Blue; });
        List<VialLiquid> greens = currentLiquids.FindAll(l => { return l.type == VialLiquid.Type.Green; });
        List<VialLiquid> yellows = currentLiquids.FindAll(l => { return l.type == VialLiquid.Type.Yellow; });
        for(int i = 0; i < blues.Count; i++) {
            blues[i].transform.localPosition = Vector3.up * i * 0.1f;
        }
        for (int i = 0; i < greens.Count; i++) {
            greens[i].transform.localPosition = Vector3.up * (blues.Count + i) * 0.1f;
        }
        for (int i = 0; i < yellows.Count; i++) {
            yellows[i].transform.localPosition = Vector3.up * (blues.Count + greens.Count + i) * 0.1f;
        }
        if ((greens.Count == 2 * yellows.Count) && (blues.Count == 2 * greens.Count)) {
            administerButton.Enable();
        } else {
            administerButton.Disable();
        }
    }

    private void CompletePuzzle() {
        Debug.Log("DONE!");
    }

}
