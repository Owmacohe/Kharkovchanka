using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowTracks : MonoBehaviour
{
    public GameObject terrain;
    public Transform raycasters;
    public Shader drawShader;
    [Range(1, 500)]
    public float size = 5;
    [Range(0, 1)]
    public float strength = 0.5f;

    private List<Transform> raycastObjects;
    private RenderTexture splatMap;
    private Material snowMaterial, drawMaterial;
    private RaycastHit hit;
    private int layerMask;

    private void Start()
    {
        layerMask = LayerMask.GetMask("Ground");
        drawMaterial = new Material(drawShader);
        snowMaterial = terrain.GetComponent<MeshRenderer>().material;
        splatMap = new RenderTexture(1024, 1024, 0, RenderTextureFormat.ARGBFloat);
        snowMaterial.SetTexture("_Splat", splatMap);

        raycastObjects = new List<Transform>();

        for (int i = 0; i < raycasters.childCount; i++)
        {
            raycastObjects.Add(raycasters.GetChild(i).transform);
        }
    }

    private void FixedUpdate()
    {
        foreach (Transform i in raycastObjects) {
            if (Physics.Raycast(i.position, -Vector3.up, out hit, 1, layerMask))
            {
                drawMaterial.SetVector("_Coordinate", new Vector4(hit.textureCoord.x, hit.textureCoord.y, 0, 0));
                drawMaterial.SetFloat("_Size", size);
                drawMaterial.SetFloat("_Strength", strength);
                RenderTexture temp = RenderTexture.GetTemporary(splatMap.width, splatMap.height, 0, RenderTextureFormat.ARGBFloat);
                Graphics.Blit(splatMap, temp);
                Graphics.Blit(temp, splatMap, drawMaterial);
                RenderTexture.ReleaseTemporary(temp);
            }
        }
    }
}
