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
    public bool isProgressing = true;
    [Range(0.1f, 0.5f)]
    public float progressSpeed = 0.5f;

    private bool isRotating;
    private float swivelAngle;

    private TMP_Text progressBarText;
    private GameObject progressBar;
    private Transform progressParent;
    private float progressAmount;

    private GameController controller;

    private void Start()
    {
        progressBarText = GetComponentInChildren<TMP_Text>();
        progressBar = progressBarText.gameObject;
        progressParent = progressBar.transform.parent;

        controller = FindObjectOfType<GameController>();
    }

    private void Update()
    {
        progressParent.LookAt(Camera.main.transform);
        Vector3 tempRotation = progressParent.rotation.eulerAngles;
        progressParent.rotation = Quaternion.Euler(tempRotation);
        //progressParent.rotation = Quaternion.Euler(new Vector3(0, tempRotation.y, tempRotation.z));

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
        if (isProgressing)
        {
            if (progressAmount < 100)
            {
                progressAmount += progressSpeed / 3f;
                progressBarText.text = "Tracking stars: " + Mathf.Round(progressAmount) + "%";
            }
            else
            {
                controller.isTrackingStars = false;
                Destroy(gameObject);
            }
        }

        if (!isRotating && Random.Range(0, swivelChance) <= 1)
        {
            swivelAngle = Random.Range(0, 360);
            isRotating = true;
        }
    }
}
