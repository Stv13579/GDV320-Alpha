using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[ExecuteAlways]
public class GradientToTexture : MonoBehaviour
{
    public bool m_Export = false;

    public string m_ExportPath = "Assets/";
    public string m_ExportName = "GradientExport";

    public int m_TextureResolution = 128;

    public Gradient m_TextureGradient;

#if UNITY_EDITOR
    // Update is called once per frame
    private void Export()
    {
        Texture2D tex = new Texture2D(m_TextureResolution, m_TextureResolution, TextureFormat.ARGB32, true);

        Color[] colours = new Color[m_TextureResolution * m_TextureResolution];
        for (int y = 0; y < m_TextureResolution; y++)
        {
            Color col = m_TextureGradient.Evaluate((float)y / (float)(m_TextureResolution - 1));
            for (int x = 0; x < m_TextureResolution; x++)
            {
                colours[(x) * m_TextureResolution + y] = col;
            }
        }
        tex.SetPixels(colours);
        tex.Apply();

        byte[] texBytes = tex.EncodeToPNG();
        DestroyImmediate(tex);

        File.WriteAllBytes(m_ExportPath + m_ExportName + ".png", texBytes);
        System.GC.Collect();

        UnityEditor.AssetDatabase.Refresh();
    }

    private void Update()
    {
        if (m_Export)
        {
            m_Export = false;
            Export();
        }
    }
#endif
}