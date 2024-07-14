using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class smoothCamera : MonoBehaviour
{
    // camera will follow this object
    public Transform target1;
    public Transform target2;
    // offset between camera and target
    public Vector3 offset;
    // change this value to get desired smoothness
    public float SmoothTime = 0.3f;
    public bool smoothX = true;
    public bool smoothY = true;
    public bool smoothZ = true;

    // This value will change at the runtime depending on target movement. Initialize with zero vector.
    public Transform targetAverage;
    private Vector3 velocity = Vector3.zero;

    private void LateUpdate()
    {
        targetAverage.position = (target1.position + target2.position) / 2 + offset;
        transform.position = Vector3.SmoothDamp(transform.position, new Vector3(targetAverage.position.x, targetAverage.position.y, transform.position.z), ref velocity, SmoothTime);
        transform.position = new Vector3(smoothX ? transform.position.x : targetAverage.position.x, smoothY ? transform.position.y : targetAverage.position.y, smoothZ ? transform.position.z : targetAverage.position.z);
    }
}
