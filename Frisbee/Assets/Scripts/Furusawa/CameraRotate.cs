using UnityEngine;

namespace Furusawa
{
    public class CameraRotate : MonoBehaviour
    {
        private Vector2 _leftStickValue;
        [Header("回転スピードの倍率(推奨1)")]
        [Range(0f, 3f)]
        [SerializeField] private float rotateAmount = 1f;
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
            
            if (vector.x > 0)
            {
               transform.Rotate(0f, rotateAmount, 0f);
            }
            else if (vector.x < 0)
            {
                transform.Rotate(0f, -rotateAmount, 0f);
            }
        }
    }
}