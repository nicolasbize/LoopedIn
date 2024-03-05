using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    public Transform roofToEnable;

    public static GameLogic Instance;

    [System.Serializable]
    public enum GameStep {
        None,
        LookingForBriefcase,
        LookingForSmilePicture,
        AskingForDrMiller,
        LookingForNickBoyfriend,
        LookingForPlayroomHint,
        OpeningPlayroom,

    }

    public GameStep Step;

    private void Awake() {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        roofToEnable.gameObject.SetActive(true);
    }

}
