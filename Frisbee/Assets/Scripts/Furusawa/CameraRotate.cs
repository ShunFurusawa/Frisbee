using System.Collections;
using UnityEngine;

namespace Furusawa
{
    public class CameraRotate : MonoBehaviour
    {
        private Vector2 _leftStickValue;
        [Header("一度の視点移動で回転する量")]
        [SerializeField] private float rotateAmount = 1f;
        [Header("補助線")]
        [SerializeField] private LineRenderer _lineRenderer;
        [Header("視点回転のシステム")]
        [SerializeField] private bool BeforeVer;
        [Header("Lineを透明化する時間")]
        [SerializeField] private float disableTime = 0.3f; 

        private bool once;
        void Update ()
        {
            //左スティックの入力取得
            _leftStickValue = OVRInput.Get(OVRInput.RawAxis2D.LThumbstick);

            if (BeforeVer == false)
            {
                Rotate(_leftStickValue);
            }
            else
            {
                BeforeRotate(_leftStickValue);
            }
          
        }

        private void Rotate(Vector2 vector)
        {
            if (vector == Vector2.zero && once)
            {
                once = false;
                return;
            }
            
            if (once)
                return;
            
            if (vector.x > 0.5f)
            {
               StartCoroutine(DelayAssistLineActive());
               transform.Rotate(0f, transform.rotation.y + rotateAmount , 0f);
               once = true;
            }
            else if (vector.x < -0.5f)
            {
                StartCoroutine(DelayAssistLineActive());
                transform.Rotate(0f, transform.rotation.y - rotateAmount, 0f);
                once = true;
            }
        }

        private void BeforeRotate(Vector2 vector)
        {
            if (vector == Vector2.zero)
                return;
            
            if (vector.x > 0)
            {
                transform.Rotate(0f, 1f, 0f);
            }
            else if (vector.x < 0)
            {
                transform.Rotate(0f, -1f, 0f);
            }

        }

        private IEnumerator DelayAssistLineActive()
        {
            _lineRenderer.enabled = false;
            yield return new WaitForSeconds(disableTime);
            _lineRenderer.enabled = true;
        }
    }
}