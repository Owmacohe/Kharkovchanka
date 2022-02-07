using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainSpawner : MonoBehaviour
{
    private GameObject terrainObject;
    public enum Directions { None, N, NE, E, SE, S, SW, W, NW }
    private float offset;

    public class TerrainChunk
    {
        public GameObject obj;
        public int x, y;

        public TerrainChunk(GameObject obj, int x, int y)
        {
            this.obj = obj;
            this.x = x;
            this.y = y;
        }
    }
    private List<TerrainChunk> terrains;

    private void Start()
    {
        terrainObject = Resources.Load<GameObject>("Terrain");
        offset = terrainObject.transform.localScale.x * 10;
        terrains = new List<TerrainChunk>();

        spawnAround(Vector2.zero);
    }

    public void spawnAround(Vector2 origin)
    {
        foreach (Directions i in Enum.GetValues(typeof(Directions)))
        {
            Vector2 temp = spawnTerrain(origin, i);

            foreach (Directions j in Enum.GetValues(typeof(Directions)))
            {
                spawnTerrain(temp, j);
            }
        }
    }

    public Vector2 spawnTerrain(Vector2 origin, Directions dir)
    {
        Vector2 temp = Vector2.zero;

        switch (dir)
        {
            case Directions.N:
                temp = new Vector2(0, 1);
                break;
            case Directions.NE:
                temp = new Vector2(1, 1);
                break;
            case Directions.E:
                temp = new Vector2(1, 0);
                break;
            case Directions.SE:
                temp = new Vector2(1, -1);
                break;
            case Directions.S:
                temp = -new Vector2(0, 1);
                break;
            case Directions.SW:
                temp = new Vector2(-1, -1);
                break;
            case Directions.W:
                temp = -new Vector2(1, 0);
                break;
            case Directions.NW:
                temp = new Vector2(-1, 1);
                break;
        }

        Vector2 tempPosition = origin + temp;
        Vector3 tempPositionOffset = new Vector3(tempPosition.x, 0, tempPosition.y) * offset;
        bool isValid = true;

        foreach (TerrainChunk i in terrains)
        {
            if (i.x == tempPosition.x && i.y == tempPosition.y)
            {
                isValid = false;
            }
        }

        if (isValid)
        {
            GameObject newTerrainObj = Instantiate(terrainObject, transform);
            newTerrainObj.transform.localPosition = tempPositionOffset;
            terrains.Add(new TerrainChunk(newTerrainObj, (int)tempPosition.x, (int)tempPosition.y));
        }

        return tempPosition;
    }

    /*
    public void clearTerrains(Vector3 origin)
    {
        foreach (TerrainChunk i in terrains)
        {
            if (Vector3.Distance(origin, i.obj.transform.position) > offset * 2)
            {
                print("cleared: " + i.x + " " + i.y);
                terrains.Remove(i);
                Destroy(i.obj);
            }
        }
    }
    */
}
