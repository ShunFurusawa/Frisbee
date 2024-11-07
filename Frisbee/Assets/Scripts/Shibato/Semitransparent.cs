using UnityEngine;

//MaterialPropetyBlockを使うと同じマテリアルを使用している
//複数のオブジェクトに対して、それぞれ独自のプロパティ値を設定できる

//透かしたいオブジェクトにつける
public class Semitransparent : MonoBehaviour
{
    [SerializeField] private float alphaValue = 0.5f;

    private Color color = Color.white;
    private MaterialPropertyBlock m_mpb;

    //親 子オブジェクトを格納。
    private MeshRenderer[] meshRenderers;

    private void Awake()
    {
        meshRenderers = GetComponentsInChildren<MeshRenderer>();
        m_mpb = new MaterialPropertyBlock();
    }

    public void ClearMaterialInvoke()
    {
        var transparentColor = Color.white;
        transparentColor.a = alphaValue;

        m_mpb.SetColor("_Color", transparentColor);
        ApplyMaterialPropertyBlock(Shader.Find("Legacy Shaders/Transparent/Diffuse"));
    }

    public void NotClearMaterialInvoke()
    {
        var opaqueColor = Color.white;
        opaqueColor.a = 1f;

        m_mpb.SetColor("_Color", opaqueColor);
        ApplyMaterialPropertyBlock(Shader.Find("Standard"));
    }

    private void ApplyMaterialPropertyBlock(Shader shader)
    {
        foreach (var meshRenderer in meshRenderers)
        {
            // シェーダーが異なる場合のみ変更
            if (meshRenderer.sharedMaterial.shader != shader) meshRenderer.sharedMaterial.shader = shader;
            meshRenderer.SetPropertyBlock(m_mpb);
        }
    }
}