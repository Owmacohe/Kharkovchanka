using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class IceCoreDrill : MonoBehaviour
{
    public bool isProgressing = true;
    [Range(0.1f, 0.5f)]
    public float progressSpeed = 0.5f;

    private TMP_Text progressBarText;
    private GameObject progressBar;
    private Transform progressParent;
    private float progressAmount;

    private GameController controller;
    private Transform iceCore;

    private void Start()
    {
        progressBarText = GetComponentInChildren<TMP_Text>();
        progressBar = progressBarText.gameObject;
        progressParent = progressBar.transform.parent;

        controller = FindObjectOfType<GameController>();
        iceCore = transform.GetChild(0);
    }

    private void Update()
    {
        progressParent.LookAt(Camera.main.transform);
        Vector3 tempRotation = progressParent.rotation.eulerAngles;
        progressParent.rotation = Quaternion.Euler(tempRotation);
        //progressParent.rotation = Quaternion.Euler(new Vector3(0, tempRotation.y, tempRotation.z));
    }

    private void FixedUpdate()
    {
        if (isProgressing)
        {
            if (progressAmount < 100)
            {
                progressAmount += progressSpeed / 3f;
                progressBarText.text = "Drilling core: " + Mathf.Round(progressAmount) + "%";
            }
            else
            {
                controller.isDrillingCore = false;
                Destroy(gameObject);
            }
        }

        if (iceCore.localPosition.y > -1.75f)
        {
            iceCore.localPosition -= Vector3.up * progressSpeed / 180f;
        }
    }
}
