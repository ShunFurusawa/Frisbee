using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shibato
{
    public class Switch : MonoBehaviour
    {
        [SerializeField, JapaneseLabel("消すオブジェクト")]
        private GameObject deadGameObjects;

        [JapaneseLabel("ONのマテリアル")] [SerializeField]
        private Material ON;

        [JapaneseLabel("OFFのマテリアル")] [SerializeField]
        private Material OFF;

        [SerializeField] private Renderer windRenderer;

        private void Awake()
        {
            windRenderer.material = OFF;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Frisbee"))
            {
                deadGameObjects.SetActive(false); // 非アクティブ化
                windRenderer.material = ON;
            }
        }
    }
}
