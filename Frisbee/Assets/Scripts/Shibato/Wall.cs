using System;
using System.Collections;
using UnityEngine;

namespace Shibato
{
    public class Wall : MonoBehaviour
    {
        [SerializeField, JapaneseLabel("1秒ごとの移動距離")] private float moveDistance = 1f;
        [SerializeField, JapaneseLabel("移動間隔")] private float moveInterval = 1f;

        private Vector3 initialPosition;  // 初期位置の保存
        private Coroutine moveCoroutine;  // コルーチンの参照保持

        [SerializeField, JapaneseLabel("壊れる壁リセット用")]
        private GameObject[] rook;

        private void Start()
        {
            initialPosition = transform.position;  // 初期位置を保存
        }

        public void EscapeStart()
        {
            if (moveCoroutine == null)  // コルーチンが実行中でない場合
            {
                moveCoroutine = StartCoroutine(MoveWallCoroutine());  // コルーチン開始
            }
        }

        private IEnumerator MoveWallCoroutine()
        {
            for (int i = 0; i < 1000; i++)
            {
                transform.position += new Vector3(0, 0, moveDistance);  // Z軸に移動
                SoundManager.instance.Play("FallFloar");
                yield return new WaitForSeconds(moveInterval);  // 指定間隔待機
            }

            // 完了後にコルーチンを停止
            moveCoroutine = null;  // コルーチンの参照をリセット
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("spines"))  // プレイヤーと衝突したか確認
            {
                GameManager.instance.Respawn();
                if (moveCoroutine != null)
                {
                    StopCoroutine(moveCoroutine);  // コルーチン停止
                    moveCoroutine = null;  // コルーチンの参照をリセット
                }
                ResetWallPosition();
            }
        }

        public void ResetWallPosition()
        {
            transform.position = initialPosition;  // 初期位置に戻す
            gameObject.SetActive(false);
            SoundManager.instance.StopPlay("Last");
            SoundManager.instance.Play("BGM");
            
            foreach (GameObject obj in rook)
            {
                if (obj != null)
                {
                    obj.SetActive(false);  // オブジェクトをアクティブ化

                }
            }
        }
        
    }
}
