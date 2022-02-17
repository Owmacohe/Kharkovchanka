using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour
{
    public float speed;

    private void FixedUpdate()
    {
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + (Vector3.up * speed));
    }
}
