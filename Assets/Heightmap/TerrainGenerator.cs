using System.Collections;
using UnityEngine;

[ExecuteInEditMode()]
public class TerrainGenerator : MonoBehaviour
{
    public float m_maxHeight = 10f;
    public int m_quality = 100;
    public float m_size = 10.0f;
    public float m_phase = 0f;
    public float m_scale = 0.25f;
    public Color m_topColor = new Color(0.2f, 1f, 0.1f);
    public Color m_midColor = new Color(0f, 0.5f, 0f);
    public Color m_bottomColor = new Color(0.7f, 0.3f, 0.1f);
    public float m_midColorPosition = 0.5f;
    private Texture2D m_texture = null;
    public Texture2D m_heightmap = null;

    public bool m_neverUpdated = true;
    public bool m_update = false;
    public bool m_dirty = false;

    private MeshFilter m_meshFilter = null;
    private MeshRenderer m_meshRenderer = null;

    private int m_oQuality = 0;
    private float m_oSize = 0f;
    private float m_oPhase = 0f;
    private float m_oScale = 0f;
    private float m_oMaxHeight = 0f;
    private Color m_oTopColor = Color.black;
    private Color m_oMidColor = Color.black;
    private Color m_oBottomColor = Color.black;
    private float m_oMidColorPosition = 0f;

    private void Start()
    {
        m_meshFilter = GetComponent<MeshFilter>();
        m_meshRenderer = GetComponent<MeshRenderer>();

        UpdateVariables();

        if (Application.isPlaying)
        {
            m_update = false;
        }
        else
        {
            m_update = true;
            m_dirty = false;

            if (m_neverUpdated)
            {
                m_dirty = true;
                m_neverUpdated = false;
            }
        }
    }

    private void Update()
    {
        if (m_update)
        {
            if (m_oQuality != m_quality)
                m_dirty = true;
            if (m_oScale != m_scale)
                m_dirty = true;
            if (m_oSize != m_size)
                m_dirty = true;
            if (m_oPhase != m_phase)
                m_dirty = true;
            if (m_oMaxHeight != m_maxHeight)
                m_dirty = true;
            if (m_oTopColor != m_topColor)
                m_dirty = true;
            if (m_oMidColor != m_midColor)
                m_dirty = true;
            if (m_oBottomColor != m_bottomColor)
                m_dirty = true;
            if (m_oMidColorPosition != m_midColorPosition)
                m_dirty = true;

            if (m_dirty)
            {
                if (m_heightmap != null)
                {
                    GenerateGrid();
                    GenerateTexture();
                    m_dirty = false;
                    UpdateVariables();
                }
            }
        }
    }

    private void UpdateVariables()
    {
        m_oQuality = m_quality;
        m_oSize = m_size;
        m_oPhase = m_phase;
        m_oScale = m_scale;
        m_oMaxHeight = m_maxHeight;
        m_oTopColor = m_topColor;
        m_oMidColor = m_midColor;
        m_oBottomColor = m_bottomColor;
        m_oMidColorPosition = m_midColorPosition;
    }

    private void GenerateTexture()
    {
        Gradient g = new Gradient();
        GradientColorKey[] gck = new GradientColorKey[3];
        GradientAlphaKey[] gak = new GradientAlphaKey[1];
        gak[0].alpha = 1f;
        gak[0].time = 0f;
        gck[0].color = m_topColor;
        gck[0].time = 0f;
        gck[1].color = m_midColor;
        gck[1].time = m_midColorPosition;
        gck[2].color = m_bottomColor;
        gck[2].time = 1f;
        g.SetKeys(gck, gak);
        int h = 255;
        m_texture = new Texture2D(
            1,
            h,
            TextureFormat.RGB24,
            false);
        for (int i = 0; i < h; ++i)
        {
            m_texture.SetPixel(
                0,
                i,
                g.Evaluate(1f - (i / (float)h)));
        }
        m_texture.Apply();
        m_meshRenderer.sharedMaterial.mainTexture = m_texture;
    }

    private void GenerateGrid()
    {
        Mesh m = new Mesh();

        float s = m_size / (m_quality - 1);

        int c = 0;
        Vector3[] verts = new Vector3[m_quality * m_quality];
        Vector2[] uvs = new Vector2[verts.Length];
        for (int i = 0; i < m_quality; ++i)
        {
            float y = i / (float)m_quality;
            for (int j = 0; j < m_quality; ++j)
            {
                float x = j / (float)m_quality;
                Color col = m_heightmap.GetPixel(
                    (int)Mathf.Floor(x * m_heightmap.width),
                    (int)Mathf.Floor(y * m_heightmap.height));
                Vector3 p = new Vector3(
                    j * s,
                    col.r * m_maxHeight,
                    i * s);
                verts[c] = p;
                uvs[c] = new Vector2(
                    0.0f,
                    p.y / m_maxHeight);
                ++c;
            }
        }

        c = 0;
        int[] triangles = new int[m_quality * m_quality * 6];
        for (int i = 0; i < m_quality - 1; ++i)
        {
            for (int j = 0; j < m_quality - 1; ++j)
            {
                triangles[c] = i * m_quality + j;
                triangles[c + 1] = (i + 1) * m_quality + j;
                triangles[c + 2] = i * m_quality + j + 1;

                triangles[c + 3] = triangles[c + 1];
                triangles[c + 4] = (i + 1) * m_quality + j + 1;
                triangles[c + 5] = triangles[c + 2];
                c += 6;
            }
        }

        m.vertices = verts;
        m.uv = uvs;
        m.triangles = triangles;
        m.RecalculateNormals();
        m.RecalculateBounds();
        m_meshFilter.mesh = m;
    }
}