using UnityEngine;

namespace Shibato
{
    public class Fan : MonoBehaviour
    {
        [JapaneseLabel("風の強さ")] private float liftForce = 10f;

        [JapaneseLabel("風の向き")] private Vector3 windDirection = Vector3.up;

        private void OnTriggerStay(Collider other)
        {
            var rb = other.GetComponent<Rigidbody>();
            if (rb != null && other.CompareTag("Player")) // 浮かせたいオブジェクトのタグ
            {
                var lift = windDirection * liftForce;
                rb.AddForce(lift, ForceMode.Acceleration);
            }
        }
    }
}