using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class MeshAnalyser : MonoBehaviour
{
    public int m_VertexCount;

    private void Update()
    {
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        if (meshFilter)
        {
            m_VertexCount = meshFilter.sharedMesh.vertexCount;
        }
    }
}
