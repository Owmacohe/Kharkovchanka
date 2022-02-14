using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TelescopeSwivel : MonoBehaviour
{
    [Range(5, 40)]
    public float swivelChance = 30;
    [Range(1, 10)]
    public float speed = 3;
    [Range(0.1f, 0.5f)]
    public float progressSpeed = 0.2f;

    private bool isRotating;
    private float swivelAngle;

    private GameObject progressBar;
    private TMP_Text progressBarText;
    private float progressAmount;

    private void Start()
    {
        progressBarText = GetComponentInChildren<TMP_Text>();
        progressBar = progressBarText.gameObject;
    }

    private void Update()
    {
        progressBar.transform.parent.LookAt(Camera.main.transform);

        if (isRotating)
        {
            float temp = transform.localEulerAngles.y;
            int direction;

            if (temp > 270)
            {
                if (swivelAngle > temp || swivelAngle < 90)
                {
                    direction = 1;
                }
                else
                {
                    direction = -1;
                }
            }
            else if (temp < 90)
            {
                if (swivelAngle > 270 || swivelAngle < temp)
                {
                    direction = -1;
                }
                else
                {
                    direction = 1;
                }
            }
            else
            {
                if (swivelAngle > temp)
                {
                    direction = 1;
                }
                else
                {
                    direction = -1;
                }
            }

            float amount = temp + (speed * direction);
            transform.localRotation = Quaternion.Euler(new Vector3(0, amount, 90));

            if (temp > (swivelAngle - speed) && temp < (swivelAngle + speed))
            {
                isRotating = false;
            }
        }
    }

    private void FixedUpdate()
    {
        progressAmount += progressSpeed / 3f;
        progressBarText.text = "Tracking stars: " + Mathf.Round(progressAmount) + "%";

        if (!isRotating && Random.Range(0, swivelChance) <= 1)
        {
            swivelAngle = Random.Range(0, 360);
            isRotating = true;
        }
    }
}
