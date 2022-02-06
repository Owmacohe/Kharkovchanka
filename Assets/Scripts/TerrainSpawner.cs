using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainSpawner : MonoBehaviour
{
    private GameObject terrainObject;
    private List<GameObject> terrains;
    public enum Directions { N, NE, E, SE, S, SW, W, NW }
    private float offset;

    private void Start()
    {
        terrainObject = Resources.Load<GameObject>("Terrain");
        offset = terrainObject.transform.localScale.x * 10;
        terrains = new List<GameObject>();

        for (int i = 0; i < transform.childCount; i++)
        {
            terrains.Add(transform.GetChild(i).gameObject);
        }

        foreach (Directions j in Enum.GetValues(typeof(Directions)))
        {
            spawnTerrain(Vector3.zero, j);
        }
    }

    public void spawnTerrain(Vector3 origin, Directions dir)
    {
        GameObject newTerrain = Instantiate(terrainObject, transform);

        Vector3 temp = Vector3.zero;

        switch (dir)
        {
            case Directions.N:
                temp = Vector3.forward * offset;
                break;
            case Directions.NE:
                temp = new Vector3(1, 0, 1) * offset;
                break;
            case Directions.E:
                temp = Vector3.right * offset;
                break;
            case Directions.SE:
                temp = new Vector3(1, 0, -1) * offset;
                break;
            case Directions.S:
                temp = Vector3.forward * -offset;
                break;
            case Directions.SW:
                temp = new Vector3(-1, 0, -1) * offset;
                break;
            case Directions.W:
                temp = Vector3.right * -offset;
                break;
            case Directions.NW:
                temp = new Vector3(-1, 0, 1) * offset;
                break;
        }

        Vector3 tempPosition = new Vector3(origin.x, 0, origin.z) + temp;
        bool isValid = true;

        foreach (GameObject i in terrains)
        {
            if (Vector3.Distance(i.transform.position, tempPosition) <= offset / 2.0)
            {
                isValid = false;
            }
        }

        if (isValid)
        {
            newTerrain.transform.localPosition = tempPosition;
        }
    }

    public void clearTerrains(Vector3 origin)
    {
        foreach (GameObject i in terrains)
        {
            if (Vector3.Distance(origin, i.transform.position) > offset * 2)
            {
                Destroy(i);
                print("cleared");
            }
        }
    }
}
