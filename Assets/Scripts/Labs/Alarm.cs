using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alarm : MonoBehaviour
{
    public Transform spotlight;
    public float lightRotationSpeed;

    void Update()
    {
        spotlight.Rotate(new Vector3(0, Time.deltaTime * lightRotationSpeed, 0));
    }
}
