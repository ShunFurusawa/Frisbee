using UnityEngine;

//MaterialPropetyBlockを使うと同じマテリアルを使用している
//複数のオブジェクトに対して、それぞれ独自のプロパティ値を設定できる

public class Semitransparent : MonoBehaviour
{
    [SerializeField] private float alphaValue = 0.5f;

    private Color color = Color.white;
    private MaterialPropertyBlock m_mpb;

    //親 子オブジェクトを格納。
    private MeshRenderer[] meshRenderers;

    //一つのマテリアルを共有していてもそれぞれ独自に設定をいじれる
    public MaterialPropertyBlock mpb =>
        //null の場合は右辺の値を返す。
        m_mpb ?? (m_mpb = new MaterialPropertyBlock());

    private void Awake()
    {
        meshRenderers = GetComponentsInChildren<MeshRenderer>();
    }

    public void ClearMaterialInvoke()
    {
        color.a = alphaValue;

        mpb.SetColor(Shader.PropertyToID("_Color"), color);
        for (var i = 0; i < meshRenderers.Length; i++)
        {
            meshRenderers[i].GetComponent<Renderer>().material.shader =
                Shader.Find("Legacy Shaders/Transparent/Diffuse");
            meshRenderers[i].SetPropertyBlock(mpb);
        }
    }

    public void NotClearMaterialInvoke()
    {
        //元に戻す
        color.a = 1f;

        mpb.SetColor(Shader.PropertyToID("_Color"), color);
        for (var i = 0; i < meshRenderers.Length; i++)
        {
            meshRenderers[i].GetComponent<Renderer>().material.shader = Shader.Find("Standard");
            meshRenderers[i].SetPropertyBlock(mpb);
        }
    }
}