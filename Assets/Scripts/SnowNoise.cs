using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowNoise : MonoBehaviour
{
    public Shader snowfallShader;
    [Range(0.001f, 0.1f)]
    public float flakeAmount = 0.01f;
    [Range(0, 1)]
    public float flakeOpacity = 0.04f;

    private Material snowfallMaterial;
    private MeshRenderer meshRenderer;

    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        snowfallMaterial = new Material(snowfallShader);
    }

    private void FixedUpdate()
    {
        if (meshRenderer.material.GetTexture("_Splat") != null)
        {
            snowfallMaterial.SetFloat("_FlakeAmount", flakeAmount);
            snowfallMaterial.SetFloat("_FlakeOpacity", flakeOpacity);
            RenderTexture snow = (RenderTexture)meshRenderer.material.GetTexture("_Splat");
            RenderTexture temp = RenderTexture.GetTemporary(snow.width, snow.height, 0, RenderTextureFormat.ARGBFloat);
            Graphics.Blit(snow, temp, snowfallMaterial);
            Graphics.Blit(temp, snow);
            meshRenderer.material.SetTexture("_Splat", snow);
            RenderTexture.ReleaseTemporary(temp);
        }
    }
}
