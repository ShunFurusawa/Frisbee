using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Furusawa
{
    public class LineRotate : MonoBehaviour
    {
        [Header("回転の中心(フリスビー)")]
        [SerializeField] GameObject centerObj;
        [FormerlySerializedAs("angle")]
        [Header("回転スピードの倍率")]
        [SerializeField][Range(0f, 100f)]  float magnification = 50f;

        private Vector2 _rightStickValue;
        void Update ()
        {
            //左スティックの入力取得
            _rightStickValue = OVRInput.Get(OVRInput.RawAxis2D.RThumbstick);

            RotateAround(_rightStickValue);
        }

        private void RotateAround(Vector2 vector)
        {
            // 右スティックを左に倒していたら左回転
            if (vector.x != 0)
            {
                //RotateAround(中心の場所,軸,回転角度)
                transform.RotateAround (centerObj.transform.position, transform.up, vector.x * magnification * Time.deltaTime);
            }
        }
    }
}