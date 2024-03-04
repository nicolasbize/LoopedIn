using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class DialogSO : ScriptableObject
{
    public string npcText;
    public PartialDialogSO[] playerOptions;
}
