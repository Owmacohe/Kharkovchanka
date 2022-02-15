using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockSpawner : MonoBehaviour
{
    public bool collidable = true;
    [Range(0, 500)]
    public int density = 100;
    [Range(0.1f, 0.2f)]
    public float rockSizeMax = 0.15f;

    [HideInInspector]
    public List<Vector3> pointsOfInterest;
    private GameController controller;

    public void spawnRocks()
    {
        float radius = transform.localScale.x / 2;

        GameObject[] rocks = Resources.LoadAll<GameObject>("Rocks");

        for (int i = 0; i < density; i++)
        {
            GameObject temp = Instantiate(rocks[Random.Range(0, rocks.Length)], transform);

            Vector3 randomRockPosition = new Vector3(randomVector3().x, 0, randomVector3().y) * radius;
            temp.transform.localPosition = randomRockPosition;
            temp.transform.localScale = Vector3.one * Random.Range(0.05f, rockSizeMax);
            temp.transform.localRotation = Quaternion.Euler(randomVector3());

            temp.GetComponent<Collider>().enabled = collidable;
        }

        pointsOfInterest = new List<Vector3>();

        int POICount = Random.Range(1, 3);

        for (int j = 0; j < POICount; j++)
        {
            while (true)
            {
                Vector3 randomPOIPosition = transform.TransformPoint(new Vector3(randomVector3().x, 0, randomVector3().y) * radius);
                bool isValid = true;

                for (int k = 0; k < rocks.Length; k++)
                {
                    if (Vector3.Distance(randomPOIPosition, rocks[k].transform.position) <= (3 * rockSizeMax))
                    {
                        isValid = false;
                    }
                }

                if (isValid)
                {
                    pointsOfInterest.Add(randomPOIPosition);
                    break;
                }
            }
        }

        if (controller == null)
        {
            controller = FindObjectOfType<GameController>();
        }

        controller.addNewPOIs(pointsOfInterest);
    }

    private Vector3 randomVector3()
    {
        return new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));
    }
}
