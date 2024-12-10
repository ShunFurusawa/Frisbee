using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// 二段階でめり込み検知、修正を行う。
/// 深くめり込むのを検知 -> 厳しめのめり込み検知、修正システムがONになるイメージ
/// </summary>
public class SinkingFix : MonoBehaviour
{
    private CapsuleCollider _cc;
    private Rigidbody _rb;
    void Start()
    {
        isSinking = false;
        _cc = this.GetComponent<CapsuleCollider>();
       // _rb = GetComponent<Rigidbody>();
        if (_cc == null)
        {
            Debug.Log("CapsuleCollider is null");
        }
    }
    void Update()
    {
        CheckSinking();
        CheckSinkingDeeply();
    }

    private bool isSinking = false;
    private Collider[] _overLapColliders_Floar;
    private Collider[] _overLapColliders_Wall;
    [FormerlySerializedAs("divisor")]
    [Header("2.5で半分のサイズとしてめり込み検知")]
    [SerializeField] private float _divisor = default!;
    
    /// <summary>
    /// めり込みチェックして必要に応じてめり込み修正の指示を出す関数
    /// </summary>
    private void CheckSinking()
    {
        if (isSinkingDeeply == false)
            return;

        _overLapColliders_Floar = OverlapCheck(LayerMask.GetMask("Floar"), 2f);
        _overLapColliders_Wall = OverlapCheck(LayerMask.GetMask("Wall"), 2f);
         //LayerMaskは判定を取得するオブジェクトの選択に使う

        if (0 < _overLapColliders_Floar.Length || 0 < _overLapColliders_Wall.Length)
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

    /// <summary>
    /// 実際にめり込みを検知する関数
    /// </summary>
    /// <param name="layer">FloarかWall</param>
    /// <param name="divisor">通常のめり込み検知と深めのめり込み検知で使い分け</param>
    /// <returns></returns>
    private Collider[] OverlapCheck(int layer, float divisor)
    {
        // point0: カプセルの一方の端点の位置
        // point1: カプセルのもう一方の端点の位置
        // radius: カプセルの半径
        return Physics.OverlapCapsule(
            transform.position + new Vector3(-_cc.center.x, (_cc.center.y - _cc.height / divisor) + _cc.radius,
                -_cc.center.z),
            transform.position + new Vector3(-_cc.center.x, (_cc.center.y + _cc.height / divisor) - _cc.radius,
                -_cc.center.z),
            _cc.radius,
            layer);
    }

    private void StopFix()
    {
        isSinking = false;
        isSinkingDeeply = false;
        Debug.Log("StopFix");
    }
    
    private Collider[] _overLapColliders_D_Floar;
    private Collider[] _overLapColliders_D_Wall;
    private bool isSinkingDeeply = false;
    private void CheckSinkingDeeply()
    {
        //コライダーを通常より小さく指定して、深くめりこんでいるか検知
        
        _overLapColliders_D_Floar = OverlapCheck(LayerMask.GetMask("Floar"), _divisor);
        _overLapColliders_D_Wall = OverlapCheck(LayerMask.GetMask("Wall"), _divisor);
        //LayerMaskは判定を取得するオブジェクトの選択に使う
        //床とのめり込みさえ修正できればいいからenemyとplayerの指定いらないかも

        if (0 < _overLapColliders_D_Floar.Length || 0 < _overLapColliders_D_Wall.Length) 
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
            foreach (Collider col in _overLapColliders_Floar)
            {
                bool penetrate = Physics.ComputePenetration(
                    _cc, transform.position, transform.rotation,
                    col.GetComponent<Collider>(), col.transform.position, col.transform.rotation,
                        out direction, out distance);

                if (penetrate)
                {
                    //direction = Vector3.Scale(direction, new Vector3(1, 7f, 1));
                    transform.position += direction * distance;
                  //  TempStopPhysics();  //移動後の動き抑制
                    Debug.Log("call TempStopPhysics");
                }
            }
        }
    }
}