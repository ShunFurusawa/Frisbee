using UnityEngine;
using System.Collections;

namespace Furusawa
{
    public class Reflection : MonoBehaviour
    {
        [Header("反射時は力を〇倍する")]
        [SerializeField] private Vector3 scalePower;
        private Rigidbody _rb;
        private bool _wait;
        private void OnCollisionEnter(Collision other)
        {
            if (_wait)
                return;
            
            if (_rb == null)
            {
                _rb = other.gameObject.GetComponent<Rigidbody>();
            }

            
            // 入射ベクトル (速度）
            Vector3 inDirection = _rb.velocity;
            // 法線ベクトル 接地点から取得
            Vector3 inNormal =  other.contacts[0].normal;
            Debug.Log("normal = " + other.contacts[0].normal);
           //Vector3 inNormal =  Vector3.up;
            // 反射ベクトル（速度）
            inNormal = Vector3.Scale(inNormal, new Vector3(1f, 1f, 1f));
            Vector3 result = Vector3.Reflect(inDirection, inNormal);
        
            // 反射後の速度を反映
            _rb.velocity = Vector3.Scale(result, scalePower);
            StartCoroutine("WaitAndReflective");
            Debug.Log("reflection!");
         
        }

        //OnCollisionが何度も起きないように待機時間入れてみる
        private IEnumerator WaitAndReflective()
        {
            _wait = true;
            yield return new WaitForSeconds(0.3f);
            _wait = false;
        }
    }
}