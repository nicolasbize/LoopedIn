using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionModifier : MonoBehaviour
{
    public float intensityPosition;
    public float intensityRotation;
    public bool lockPositionX;
    public bool lockPositionY;
    public bool lockPositionZ;
    public bool lockRotationX;
    public bool lockRotationY;
    public bool lockRotationZ;

    // Start is called before the first frame update
    void Start()
    {
        Vector3 newPosition = transform.position;
        Vector3 newRotation = transform.rotation.eulerAngles;
        if (!lockPositionX) {
            newPosition.x = Random.Range(newPosition.x - intensityPosition / 2, newPosition.x + intensityPosition / 2);
        }
        if (!lockPositionY) {
            newPosition.y = Random.Range(newPosition.y - intensityPosition / 2, newPosition.y + intensityPosition / 2);
        }
        if (!lockPositionZ) {
            newPosition.z = Random.Range(newPosition.z - intensityPosition / 2, newPosition.z + intensityPosition / 2);
        }
        if (!lockRotationX) {
            newRotation.x = Random.Range(newRotation.x - intensityRotation / 2, newRotation.x + intensityRotation / 2);
        }
        if (!lockRotationY) {
            newRotation.y = Random.Range(newRotation.y - intensityRotation / 2, newRotation.y + intensityRotation / 2);
        }
        if (!lockRotationZ) {
            newRotation.z = Random.Range(newRotation.z - intensityRotation / 2, newRotation.z + intensityRotation / 2);
        }
        transform.position = newPosition;
        transform.rotation = Quaternion.Euler(newRotation);
    }

}
