using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{
    
    [SerializeField, JapaneseLabel("消すオブジェクト")]
    private GameObject[] deadGameObjects;
    [JapaneseLabel("ONのマテリアル")][SerializeField] private Material ON;
    [JapaneseLabel("OFFのマテリアル")][SerializeField] private Material OFF;
    private Renderer windRenderer;

    private void Awake()
    {
        windRenderer = GetComponent<Renderer>();
        windRenderer.material = OFF;
    }

    private void OnCollisionEnter(Collision other)
    {
        foreach (var obj in deadGameObjects) // 配列内の全要素を処理
        {
            if (obj != null) // null チェック
            {
                obj.SetActive(false); // 非アクティブ化
                windRenderer.material = ON;
            }
        }
    }
}
