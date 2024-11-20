using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Furusawa
{
    public class LineRotate : MonoBehaviour
    {
        [Header("回転の中心(フリスビー)")]
        [SerializeField] GameObject centerObj;
        
        [SerializeField][Range(0f, 100f)]  float angle = 50f;

        private Vector2 _leftStickValue;
        void Update ()
        {
            //左スティックの入力取得
            _leftStickValue = OVRInput.Get(OVRInput.RawAxis2D.LThumbstick);

            Rotate(_leftStickValue);
        }

        private void Rotate(Vector2 vector)
        {
            if (vector == Vector2.zero)
                return;
            
            // 左スティックを左に倒していたら左回転
            if (vector.x < 0)
            {
                //RotateAround(中心の場所,軸,回転角度)
                transform.RotateAround (centerObj.transform.position, Vector3.up, -angle * Time.deltaTime);
            }
            else if (vector.x > 0)
            {
                // 右回転
                transform.RotateAround (centerObj.transform.position, Vector3.up, angle * Time.deltaTime);
            }
        }
    }
}