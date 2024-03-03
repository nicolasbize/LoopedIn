using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public bool isSitting;
    public Transform humanoid;

    void Start() {
        humanoid.GetComponent<Animator>().SetBool("IsSitting", isSitting);

    }

}
