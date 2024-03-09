using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VialPuzzle : MonoBehaviour
{
    public Transform rootLiquid;
    public Button[] AddLiquidButtons;
    public Transform liquidPrefab;

    private List<Transform> currentLiquids = new List<Transform>();
    

    private void Start() {
        foreach (Button button in AddLiquidButtons) {
            button.OnButtonPress += OnAddLiquidButtonPress;
        }

        ResetLiquids();
    }

    private void ResetLiquids() {
        foreach (Transform t in currentLiquids) {
            Destroy(t.gameObject);
        }
        currentLiquids = new List<Transform>();
        AddLiquid(VialLiquid.Type.Blue);
        AddLiquid(VialLiquid.Type.Blue);
    }

    private void OnAddLiquidButtonPress(object sender, Button.OnButtonPressEventArgs e) {
        AddLiquid(VialLiquid.LiquidTypeDictionary[e.value]);
    }

    private void AddLiquid(VialLiquid.Type type) {
        Transform liquid = Instantiate(liquidPrefab, rootLiquid);
        liquid.GetComponent<VialLiquid>().SetType(type);
        currentLiquids.Add(liquid);
        ReorderLiquids();
        liquid.localPosition = Vector3.up * currentLiquids.Count * 0.1f;
    }

    private void ReorderLiquids() {
        // TODO HERE

    }

}
