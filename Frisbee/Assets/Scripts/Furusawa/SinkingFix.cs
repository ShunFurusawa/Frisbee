using UnityEngine;

/// <summary>
/// 二段階でめり込み検知、修正を行う。
/// 深くめり込むのを検知 -> 厳しめのめり込み検知、修正システムがONになるイメージ
/// </summary>
public class SinkingFix : MonoBehaviour
{
    private CapsuleCollider _cc;
    void Start()
    {
        isSinking = false;
        _cc = this.GetComponent<CapsuleCollider>();
        if (_cc == null)
        {
            Debug.Log("CapsuleCollider is null");
        }
    }
    void Update()
    {
        CheckSinking();
        CheckSinkingLoosely();
    }

    private bool isSinking = false;
    private Collider[] _overLapColliders;
    [Header("2.5で半分のサイズとしてめり込み検知")]
    [SerializeField] private float divisor = default!;
    private void CheckSinking()
    {
        if (isSinkingDeeply == false)
            return;
        
        // point0: カプセルの一方の端点の位置
        // point1: カプセルのもう一方の端点の位置
        // radius: カプセルの半径
         _overLapColliders = Physics.OverlapCapsule(
            transform.position + new Vector3(-_cc.center.x, (_cc.center.y - _cc.height / 2) + _cc.radius,
                -_cc.center.z),
            transform.position + new Vector3(-_cc.center.x, (_cc.center.y + _cc.height / 2) - _cc.radius,
                -_cc.center.z),
            _cc.radius,
            LayerMask.GetMask("Floar") 
        );
         //LayerMaskは判定を取得するオブジェクトの選択に使う

        if (0 < _overLapColliders.Length)
        {
            isSinking = true;
           // TempStopPhysics();
            FixSinking();
        }
        else if (isSinkingDeeply)
        {
            StopFix();
        }
    }

    private void StopFix()
    {
        isSinking = false;
        isSinkingDeeply = false;
        Debug.Log("StopFix");
    }
    
    private Collider[] _overLapColliders_D;
    private bool isSinkingDeeply = false;
    private void CheckSinkingLoosely()
    {
        //コライダーを通常より小さく指定して、深くめりこんでいるか検知
        
        _overLapColliders_D = Physics.OverlapCapsule(
            transform.position + new Vector3(-_cc.center.x, (_cc.center.y - _cc.height / divisor) + _cc.radius,
                -_cc.center.z),
            transform.position + new Vector3(-_cc.center.x, (_cc.center.y + _cc.height / divisor) - _cc.radius,
                -_cc.center.z),
            _cc.radius,
            LayerMask.GetMask("Floar") 
        );
        //LayerMaskは判定を取得するオブジェクトの選択に使う
        //床とのめり込みさえ修正できればいいからenemyとplayerの指定いらないかも

        if (0 < _overLapColliders_D.Length)
        {
            Debug.Log("Sinking!");
            isSinkingDeeply = true;
        }
    }
 
    /*private void TempStopPhysics()
    {
        _rb.velocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;
    }*/

    private Vector3 direction;
    private float distance;
    private void FixSinking()
    {
        if (isSinking)
        {
            foreach (Collider col in _overLapColliders)
            {
                bool penetrate = Physics.ComputePenetration(
                    _cc, transform.position, transform.rotation,
                    col.GetComponent<Collider>(), col.transform.position, col.transform.rotation,
                        out direction, out distance);

                if (penetrate)
                {
                    //direction = Vector3.Scale(direction, new Vector3(1, 7f, 1));
                    transform.position += direction * distance;
                //    TempStopPhysics();  //移動後の動き抑制
                }
            }
        }
    }
}