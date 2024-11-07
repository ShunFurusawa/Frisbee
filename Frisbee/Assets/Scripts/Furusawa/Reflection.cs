using Unity.VisualScripting;
using UnityEngine;

namespace Furusawa
{
    public class Reflection : MonoBehaviour
    {
        [Header("反射時は力を〇倍する")]
        [SerializeField] private Vector3 scalePower;
        private void OnCollisionEnter(Collision other)
        {
            Rigidbody _rb = other.gameObject.GetComponent<Rigidbody>();

            if (_rb == null)
            {
                Debug.LogAssertion("Rigidbody is null");
                return;
            }
            
            // 入射ベクトル (速度）
            Vector3 inDirection = _rb.velocity;
            // 法線ベクトル 接地点から取得
            Vector3 inNormal =  other.contacts[0].normal;
            // 反射ベクトル（速度）
            Vector3 result = Vector3.Reflect(inDirection, inNormal);
          
            // 反射後の速度を反映
            _rb.velocity = Vector3.Scale(result, scalePower);

            Debug.Log("reflection!");
        }
    }
}