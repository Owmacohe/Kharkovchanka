using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundDeformation : MonoBehaviour
{
    [Range(0, 10)]
    public int bumps;
    public bool isBumpNumRandom;
    [Range(0.1f, 2)]
    public float bumpHeight = 0.1f;
    [Range(0.5f, 2)]
    public float bumpFalloff = 1;
    [Range(0, 2)]
    public float edgeBuffer = 1;

    public void deformMesh()
    {
        Mesh deform = GetComponent<MeshFilter>().mesh;
        Vector3[] verts = deform.vertices;
        int vertsLength = verts.Length;

        float curveMax = (1 / bumpFalloff) * Mathf.PI;

        if (isBumpNumRandom)
        {
            bumps = Random.Range(0, bumps);
        }

        for (int i = 0; i < bumps; i++)
        {
            int temp = Random.Range(0, vertsLength);

            while (Vector3.Distance(transform.TransformPoint(verts[temp]), transform.position) > curveMax)
            {
                temp = Random.Range(0, vertsLength);
            }

            for (int j = 0; j < vertsLength; j++)
            {
                float dist = Vector3.Distance(verts[temp], verts[j]);
                float displacement = bumpHeight * Mathf.Cos(bumpFalloff * dist) + bumpHeight;

                if (dist < curveMax - edgeBuffer)
                {
                    verts[j] += Vector3.up * displacement;
                }
            }
        }

        deform.SetVertices(verts);
        GetComponent<MeshCollider>().sharedMesh = deform;
    }
}
