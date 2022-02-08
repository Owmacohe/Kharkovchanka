using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DrawWithMouse : MonoBehaviour
{
    public Camera cam;
    public Shader drawShader;
    [Range(1, 500)]
    public float size;
    [Range(0, 1)]
    public float strength;

    private RenderTexture splatMap;
    private Material snowMaterial, drawMaterial;
    private RaycastHit hit;

    private void Start()
    {
        drawMaterial = new Material(drawShader);
        drawMaterial.SetVector("_Color", Color.red);

        snowMaterial = GetComponent<MeshRenderer>().material;
        splatMap = new RenderTexture(1024, 1024, 0, RenderTextureFormat.ARGBFloat);
        snowMaterial.SetTexture("_Splat", splatMap);
    }

    private void FixedUpdate()
    {
        if (Mouse.current.press.isPressed && Physics.Raycast(cam.ScreenPointToRay(Mouse.current.position.ReadValue()), out hit))
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
