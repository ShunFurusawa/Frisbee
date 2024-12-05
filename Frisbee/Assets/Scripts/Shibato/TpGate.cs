using System;
using UnityEngine;
using System.Collections;
namespace Shibato
{
    public class TpGate : MonoBehaviour
    {
        [SerializeField,JapaneseLabel("出口")] private Transform exitGate;
        [SerializeField] private float exitVelocityMultiplier = 1.0f;
        private BoxCollider exitGateBoxCollider;

        private void Awake()
        {
            exitGateBoxCollider = exitGate.GetComponent<BoxCollider>();
        }

        void OnTriggerEnter(Collider other)
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (other.CompareTag("Frisbee"))
            {
                exitGateBoxCollider.enabled = false;
                // 入射ベクトルを保存
                Vector3 incomingVelocity = rb.velocity;

                // 物体を出口に移動
                other.transform.position = exitGate.position;

                // 出口の方向を考慮して速度を設定
                Vector3 exitDirection = exitGate.forward; // 出口の前方向
                rb.velocity = exitDirection.normalized * incomingVelocity.magnitude * exitVelocityMultiplier;
                StartCoroutine(ReEnableExitGate());
            }
        }
        private IEnumerator ReEnableExitGate()
        {
            // 指定時間待機
            yield return new WaitForSeconds(10f);

            // ExitGateの判定を復活
            exitGateBoxCollider.enabled = true;
        }
    }
}