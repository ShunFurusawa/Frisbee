using UnityEngine;
using System.Collections;

namespace Shibato
{
    public class DropArea : MonoBehaviour
    {
        [JapaneseLabel("揺れる時間")] [SerializeField] private float wobbleDuration = 2.0f;
        [JapaneseLabel("落下するまでの待機時間")] [SerializeField] private float fallDelay = 0.1f;
        [JapaneseLabel("揺れ幅")] [SerializeField] private float wobbleIntensity = 0.1f;

        private bool isWobbling = false;
        private Vector3 initialPosition;
        private Rigidbody rb;

        void Start()
        {
            initialPosition = transform.position;
            rb = GetComponent<Rigidbody>();
            
            if (rb == null)
            {
                rb = gameObject.AddComponent<Rigidbody>();
            }

            rb.isKinematic = true;
        }

        private bool hasFallen = false;

        void OnTriggerEnter(Collider other)
        {
            if (!isWobbling && !hasFallen && other.CompareTag("substance"))
            {
                StartCoroutine(WobbleAndFallCoroutine());
            }
        }


        IEnumerator WobbleAndFallCoroutine()
        {
            isWobbling = true;
            float elapsedTime = 0f;

            // 揺れ
            while (elapsedTime < wobbleDuration)
            {
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
            hasFallen = true;
            isWobbling = false;
        }
        private void OnDestroy()
        {
            StopAllCoroutines();
        }
    }
}