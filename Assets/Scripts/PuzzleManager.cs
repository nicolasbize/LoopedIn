using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{

    enum PuzzleType {
        SmileLetter,
        BreakroomCode,
        JayDiary,
    }

    public static PuzzleManager Instance;

    private void Awake() {
        Instance = this;
    }
}
