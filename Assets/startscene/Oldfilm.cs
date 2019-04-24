using UnityEngine;
[ExecuteInEditMode]
public class Oldfilm : MonoBehaviour
{
    public Material m_mat = null;

    private void OnRenderImage(RenderTexture s, RenderTexture d)
    {
        Graphics.Blit(s, d, m_mat);
    }
}
