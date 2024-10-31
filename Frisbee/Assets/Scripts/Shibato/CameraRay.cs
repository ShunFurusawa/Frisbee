using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
public class CameraRay : MonoBehaviour
{
    [SerializeField] GameObject player;
    //Animator playerAnimator;
    //AnimatorStateInfo playerStateInfo;

    // 前フレームで遮蔽物として扱われていたゲームオブジェクトが格納される
    public GameObject[] prevRaycast;
    public List<GameObject> raycastHitsList_ = new List<GameObject>();
    public LayerMask obstacleLayer; // 障害物レイヤー
    
    float maxDistance; //rayを飛ばす最大距離。
    Vector3 _difference;

    // Start is called before the first frame update
    void Awake()
    {
        _difference = (player.transform.position - this.transform.position);
        maxDistance = _difference.magnitude;    //二つのオブジェクト間のベクトルの長さを求めて格納、Rayを飛ばす最大距離を制限
                                                //magnitudeは平方根の計算で長差を求める関数
    }

    // Update is called once per frame
    void Update()
    {
        //playerAnimator = player.GetComponent<Animator>();
        //playerStateInfo = playerAnimator.GetCurrentAnimatorStateInfo(0);
        //if (!playerStateInfo.IsName("Jumping")) //ジャンプ中はrayを飛ばさない　根本の解決になってない
        //{
        //    Raycast();
        //}        
        Raycast();
    }

    void FixedUpdate()      //cinemachine cameraのUpdate Methodがfixed updataだからこっちかも
    {
        
    }
    void Raycast()
    {
        //二つのオブジェクト間のベクトルを取得
         _difference = (player.transform.position - this.transform.position);   
         //.normalizedベクトルの正規化を行う
        Vector3 _direction = _difference.normalized;
         //Debug.Log(_difference.sqrMagnitude);

        // Ray(開始地点,　進む方向)
        Ray _ray = new Ray(this.transform.position, _direction);     

        // Rayが衝突した全てのコライダーの情報を得る
        RaycastHit[] rayCastHits = Physics.RaycastAll(_ray, maxDistance, obstacleLayer);

        // 前フレームで遮蔽物だった全てのGameObjectを保持
        prevRaycast = raycastHitsList_.ToArray();
        raycastHitsList_.Clear();

        foreach (RaycastHit hit in rayCastHits)
        {
            Semitransparent semiTransparent = hit.collider.GetComponent<Semitransparent>();
            if (semiTransparent != null)
            {
                semiTransparent.ClearMaterialInvoke();
            }
            
                //壁にまっすぐめり込んでも透明化してしまう
                
                //半透明化
                semiTransparent.ClearMaterialInvoke();
                //次のフレームで使いたいから、不透明にしたオブジェクトを追加する
                raycastHitsList_.Add(hit.collider.gameObject);
        }

        //このforeach文はExceptメソッドを使って、prevRaycastからraycastHitsList_の要素を除外した結果を走査
        //前フレームで遮蔽物だったもの以外が万が一入っていた時の保険？
        foreach (GameObject _gameObject in prevRaycast.Except<GameObject>(raycastHitsList_))
        {
            Semitransparent noSemiTransparent = _gameObject.GetComponent<Semitransparent>();
            // 遮蔽物でなくなったGameObjectを通常に戻す

            if (_gameObject != null)
            {
                noSemiTransparent.NotClearMaterialInvoke();
            }
        }
    }
}