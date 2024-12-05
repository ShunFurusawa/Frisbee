using System;
using UnityEngine;
using System.Collections;
namespace Shibato
{
    public class TpGate : MonoBehaviour
    {
        [SerializeField,JapaneseLabel("出口")] private Transform exitGate;
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
                
                Vector3 entrancePosition = transform.position;
                Vector3 exitPosition = exitGate.transform.position;
                
                exitGateBoxCollider.enabled = false;
                // 入射ベクトルを保存
                Vector3 velocity = rb.velocity;

                // 入った時の角度を計算
                Vector3 direction = (other.transform.position - entrancePosition).normalized;

                // 出口での新しい位置と方向を設定
                other.transform.position = exitPosition + direction * 1.0f;
                rb.velocity = velocity; 
                
                StartCoroutine(ReEnableExitGate());
            }
        }
        private IEnumerator ReEnableExitGate()
        {
            yield return new WaitForSeconds(10f);
            
            exitGateBoxCollider.enabled = true;
        }
    }
}