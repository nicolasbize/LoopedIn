using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    public Transform roofToEnable;

    // Start is called before the first frame update
    void Start()
    {
        roofToEnable.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
