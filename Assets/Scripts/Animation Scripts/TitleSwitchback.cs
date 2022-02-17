using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleSwitchback : MonoBehaviour
{
    public float speed = 0.1f;
    public float distance = 30;
    public float padding = 10;

    private Rigidbody rb;
    private bool switchback;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        transform.position = new Vector3(-distance, transform.position.y, 0);
    }

    private void FixedUpdate()
    {
        if (switchback)
        {
            if (transform.position.x > -distance)
            {
                rb.transform.position -= Vector3.right * speed;
            }
            else
            {
                transform.position -= Vector3.forward * padding;
                transform.rotation = Quaternion.identity;
                switchback = false;
            }
        }
        else
        {
            if (transform.position.x < distance)
            {
                rb.transform.position += Vector3.right * speed;
            }
            else
            {
                transform.position += Vector3.forward * padding;
                transform.rotation = Quaternion.Euler(Vector3.up * 180);
                switchback = true;
            }
        }
    }
}
