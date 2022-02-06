using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowTracks : MonoBehaviour
{
    public Transform raycasters;
    public Shader drawShader;
    [Range(1, 500)]
    public float size = 5;
    [Range(0, 1)]
    public float strength = 0.5f;

    [HideInInspector]
    public List<GameObject> terrains;
    private List<Transform> raycastObjects;
    private List<RenderTexture> splatMaps;
    private List<Material> snowMaterials;
    private Material drawMaterial;
    private RaycastHit hit;
    private int layerMask;

    private void Start()
    {
        terrains = new List<GameObject>();
        raycastObjects = new List<Transform>();
        splatMaps = new List<RenderTexture>();
        snowMaterials = new List<Material>();

        layerMask = LayerMask.GetMask("Ground");
        drawMaterial = new Material(drawShader);

        for (int i = 0; i < raycasters.childCount; i++)
        {
            raycastObjects.Add(raycasters.GetChild(i).transform);
        }
    }

    public void addTerrain(GameObject t)
    {
        terrains.Add(t);
        Material tempSnowMaterial = t.GetComponent<MeshRenderer>().material;

        if (tempSnowMaterial.GetTexture("_Splat") == null)
        {
            snowMaterials.Add(tempSnowMaterial);
            RenderTexture tempTexture = new RenderTexture(1024, 1024, 0, RenderTextureFormat.ARGBFloat);
            splatMaps.Add(tempTexture);
            tempSnowMaterial.SetTexture("_Splat", tempTexture);
        }
    }

    private void FixedUpdate()
    {
        foreach (Transform i in raycastObjects) {
            if (Physics.Raycast(i.position, -Vector3.up, out hit, 1, layerMask))
            {
                for (int j = 0; j < terrains.Count; j++)
                {
                    if (hit.collider.gameObject.Equals(terrains[j]))
                    {
                        drawMaterial.SetVector("_Coordinate", new Vector4(hit.textureCoord.x, hit.textureCoord.y, 0, 0));
                        drawMaterial.SetFloat("_Size", size);
                        drawMaterial.SetFloat("_Strength", strength);
                        RenderTexture temp = RenderTexture.GetTemporary(splatMaps[j].width, splatMaps[j].height, 0, RenderTextureFormat.ARGBFloat);
                        Graphics.Blit(splatMaps[j], temp);
                        Graphics.Blit(temp, splatMaps[j], drawMaterial);
                        RenderTexture.ReleaseTemporary(temp);
                    }
                }
            }
        }
    }
}
