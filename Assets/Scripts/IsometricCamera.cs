using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsometricCamera : MonoBehaviour
{
    public Transform lookTarget;
    [Range(4, 16)]
    public float distanceFromTarget = 8;

    private void Start()
    {
        transform.localPosition = new Vector3(1, 1, -1) * distanceFromTarget;
        transform.LookAt(lookTarget);
    }
}
