using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MeteoriteSearch : MonoBehaviour
{
    public bool isProgressing = true;
    [Range(0.1f, 0.5f)]
    public float progressSpeed = 0.5f;

    private TMP_Text progressBarText;
    private GameObject progressBar;
    private Transform progressParent;
    private float progressAmount;

    private GameController controller;
    private Transform shovel;

    private void Start()
    {
        progressBarText = GetComponentInChildren<TMP_Text>();
        progressBar = progressBarText.gameObject;
        progressParent = progressBar.transform.parent;

        controller = FindObjectOfType<GameController>();
        shovel = transform.GetChild(0);

        transform.rotation = Quaternion.identity;
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
                progressBarText.text = "Searching for meteorites: " + Mathf.Round(progressAmount) + "%";
            }
            else
            {
                controller.meteoriteCount++;
                controller.shovelText.text = "" + controller.meteoriteCount;

                controller.isSearchingForMeteorites = false;
                Destroy(gameObject);
            }
        }

        shovel.transform.localPosition = Vector3.up * (0.3f * Mathf.Sin(4 * Time.time) + 1.2f);
    }
}
