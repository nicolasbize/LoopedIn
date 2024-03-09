using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class VialLiquid : MonoBehaviour
{
    public enum Type {
        Yellow, Green, Blue,
    }

    public Type type;
    public Material blueColor;
    public Material greenColor;
    public Material yellowColor;
    public MeshRenderer mesh;

    public static Dictionary<string, Type> LiquidTypeDictionary = new Dictionary<string, Type>() {
        { "y", Type.Yellow },
        { "g", Type.Green },
        { "b", Type.Blue },
    };
    private Dictionary<Type, Material> materialDictionary = new Dictionary<Type, Material>();

    public void SetType(Type type) {
        this.type = type;

    }

    private void Start() {
        materialDictionary.Add(Type.Green, greenColor);
        materialDictionary.Add(Type.Blue, blueColor);
        materialDictionary.Add(Type.Yellow, yellowColor);
        mesh.material = materialDictionary[type];
    }
}
