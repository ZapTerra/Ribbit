using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class smoothCamera : MonoBehaviour
{
    // camera will follow this object
    public Transform target1;
    public Transform target2;
    // offset between camera and target
    public Vector3 Offset;
    // change this value to get desired smoothness
    public float SmoothTime = 0.3f;

    // This value will change at the runtime depending on target movement. Initialize with zero vector.
    public Transform targetAverage;
    private Vector3 velocity = Vector3.zero;

    private void LateUpdate()
    {
        targetAverage.position = (target1.position + target2.position) / 2;
        transform.position = Vector3.SmoothDamp(transform.position, new Vector3(targetAverage.position.x, targetAverage.position.y, transform.position.z), ref velocity, SmoothTime);
    }
}
