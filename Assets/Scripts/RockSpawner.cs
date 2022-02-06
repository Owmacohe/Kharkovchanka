using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockSpawner : MonoBehaviour
{
    [Range(0, 500)]
    public int density = 100;
    [Range(0.1f, 0.2f)]
    public float rockSizeMax = 0.15f;

    private void Start()
    {
        float radius = transform.localScale.x / 2;

        GameObject[] rocks = Resources.LoadAll<GameObject>("Rocks");

        for (int i = 0; i < density; i++)
        {
            GameObject temp = Instantiate(rocks[Random.Range(0, rocks.Length)], transform);

            Vector3 randomPosition = new Vector3(randomVector3().x, 0, randomVector3().y) * radius;
            temp.transform.localPosition = randomPosition;
            temp.transform.localScale = Vector3.one * Random.Range(0.05f, rockSizeMax);
            temp.transform.localRotation = Quaternion.Euler(randomVector3());
        }
    }

    private Vector3 randomVector3()
    {
        return new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));
    }
}
