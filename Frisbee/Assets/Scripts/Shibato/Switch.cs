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

        [JapaneseLabel("揺れる時間")] [SerializeField] private float wobbleDuration = 2.0f;
        [JapaneseLabel("落下するまでの待機時間")] [SerializeField] private float fallDelay = 0.1f;
        [JapaneseLabel("揺れ幅")] [SerializeField] private float wobbleIntensity = 0.1f;
        private Rigidbody rb;
        private Vector3 initialPosition;
        private void Awake()
        {
            windRenderer.material = OFF;
            rb = deadGameObjects.GetComponent<Rigidbody>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Frisbee"))
            {
                //StartCoroutine(WobbleAndFallCoroutine());
                deadGameObjects.SetActive(false); // 非アクティブ化
                SoundManager.instance.Play("FallFloar");
                windRenderer.material = ON;
            }
        }
        
        IEnumerator WobbleAndFallCoroutine()
        {
            float elapsedTime = 0f;

            // 揺れ
            while (elapsedTime < wobbleDuration)
            {
                SoundManager.instance.Play("FallFloar");
                elapsedTime += Time.deltaTime;
                float offsetX = Mathf.Sin(elapsedTime * 10) * wobbleIntensity;
                float offsetZ = Mathf.Cos(elapsedTime * 10) * wobbleIntensity;
                transform.position = initialPosition + new Vector3(offsetX, 0, offsetZ);
                yield return null;
            }

            // 元の位置に戻して少し待機
            transform.position = initialPosition;
            yield return new WaitForSeconds(fallDelay);

            // 落下させる
            rb.isKinematic = false;
            rb.useGravity = true;
            
            yield return new WaitForSeconds(2f);
            Destroy(deadGameObjects);
        }
    }
}
