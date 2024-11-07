using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CameraRay : MonoBehaviour
{
    [SerializeField] private GameObject player;
    //Animator playerAnimator;
    //AnimatorStateInfo playerStateInfo;

    // 前フレームで遮蔽物として扱われていたゲームオブジェクトが格納される
    [SerializeField] private GameObject[] prevRaycast;
    [SerializeField] private List<GameObject> raycastHitsList_ = new();
    [SerializeField] private LayerMask obstacleLayer; // 障害物レイヤー
    private Vector3 _difference;

    private float maxDistance; //rayを飛ばす最大距離。

    // Start is called before the first frame update
    private void Awake()
    {
        if (player == null)
        {
            Debug.LogError($"Player reference not set in {gameObject.name}");
            enabled = false;
            return;
        }

        _difference = player.transform.position - transform.position;
        maxDistance = _difference.magnitude; //二つのオブジェクト間のベクトルの長さを求めて格納、Rayを飛ばす最大距離を制限
        //magnitudeは平方根の計算で長差を求める関数
    }

    // Update is called once per frame
    private void Update()
    {
        //playerAnimator = player.GetComponent<Animator>();
        //playerStateInfo = playerAnimator.GetCurrentAnimatorStateInfo(0);
        //if (!playerStateInfo.IsName("Jumping")) //ジャンプ中はrayを飛ばさない　根本の解決になってない
        //{
        //    Raycast();
        //}        
        Raycast();
    }

    private void FixedUpdate() //cinemachine cameraのUpdate Methodがfixed updataだからこっちかも
    {
    }

    private void Raycast()
    {
        //二つのオブジェクト間のベクトルを取得
        _difference = player.transform.position - transform.position;
        //.normalizedベクトルの正規化を行う
        var direction = _difference.normalized;
        //Debug.Log(_difference.sqrMagnitude);

        // Ray(開始地点,　進む方向)
        var ray = new Ray(transform.position, direction);

        // Rayが衝突した全てのコライダーの情報を得る
        var rayCastHits = Physics.RaycastAll(ray, maxDistance, obstacleLayer);

        // 前フレームで遮蔽物だった全てのGameObjectを保持
        prevRaycast = raycastHitsList_.ToArray();
        raycastHitsList_.Clear();

        foreach (var hit in rayCastHits)
        {
            var semiTransparent = hit.collider.GetComponent<Semitransparent>();
            if (semiTransparent != null) semiTransparent.ClearMaterialInvoke();

            //壁にまっすぐめり込んでも透明化してしまう

            //半透明化
            semiTransparent.ClearMaterialInvoke();
            //次のフレームで使いたいから、不透明にしたオブジェクトを追加する
            raycastHitsList_.Add(hit.collider.gameObject);
        }

        //このforeach文はExceptメソッドを使って、prevRaycastからraycastHitsList_の要素を除外した結果を走査
        //前フレームで遮蔽物だったもの以外が万が一入っていた時の保険？
        foreach (var _gameObject in prevRaycast.Except(raycastHitsList_))
        {
            var noSemiTransparent = _gameObject.GetComponent<Semitransparent>();
            // 遮蔽物でなくなったGameObjectを通常に戻す

            if (_gameObject != null) noSemiTransparent.NotClearMaterialInvoke();
        }
    }
}