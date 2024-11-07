using UnityEngine;

namespace Furusawa
{
    public class FrisbeeDirection : MonoBehaviour
    {
        private void Start()
        {
            _isLock = false;
        }

        private void Update()
        {
            AssistLine();
            if (GameManager.instance.State == FrisbeeState.Ready)
            {
                AssistLineLock();
            }
            else if (_isLock && GameManager.instance.State == FrisbeeState.Have)
            {
                _isLock = false;
            }
        }

        [SerializeField] private Transform frisbee;
        private Vector3 _direction;
        private LineRenderer _lineRenderer;

        private void SetLineRenderer()
        {
            _lineRenderer = GetComponent<LineRenderer>();
            _lineRenderer.positionCount = 2;
        }

        private Vector3 _start;
        private Vector3 _end;

        private void LineRendering(Vector3 start, Vector3 end, float length)
        {
            _lineRenderer.SetPosition(0, _start);  // 始点
            _lineRenderer.SetPosition(1, _direction * length);   // 終点
        }

        [Header("Lineの長さ")] [SerializeField] private float length;
        private void AssistLine()
        {
            if (_isLock)
                return;
            
            if (_lineRenderer == null)
            {
                SetLineRenderer();
            }
            
            _start = frisbee.position;
            _end = transform.position;

            LineRendering(_start, _end, length);
        }
        
        private bool _isLock;
        private void AssistLineLock()
        {
            LineRendering(_start, _end, length);
            
            if (_isLock == false)
            {
                DirectionConfirm();
                _isLock = true;
            }
        }

        private void DirectionConfirm()
        {
            _direction = (_end - _start).normalized;
            GameManager.instance.Directon = _direction;
        }
    }
}