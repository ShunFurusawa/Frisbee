using UnityEngine;

namespace Shibato
{
    public class Fan : MonoBehaviour
    {
        [Header("風の強さ")] public float liftForce = 10f;

        [Header("風の向き")] public Vector3 windDirection = Vector3.up;

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