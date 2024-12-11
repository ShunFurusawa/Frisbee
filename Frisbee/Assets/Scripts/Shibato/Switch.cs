using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{
    
    [SerializeField, JapaneseLabel("消すオブジェクト")]
    private GameObject[] deadGameObjects;
    private void OnCollisionEnter(Collision other)
    {
        foreach (GameObject obj in deadGameObjects) // 配列内の全要素を処理
        {
            if (obj != null) // null チェック
            {
                obj.SetActive(false); // 非アクティブ化
            }
        }
    }
}
