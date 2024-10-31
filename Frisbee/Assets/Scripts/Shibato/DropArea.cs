using UnityEngine;

namespace Shibato
{
    public class DropArea : MonoBehaviour
    {
        [SerializeField] private float time = 5f;
        private Rigidbody _rigidbody;
        private float lastTime;

        private void Awake()
        {
            _rigidbody = transform.GetComponent<Rigidbody>();
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.CompareTag("substance"))
            {
                Debug.Log("sub");
                if (Time.deltaTime - lastTime < time) 
                {
                    lastTime = Time.deltaTime;
                    
                }
                else
                {
                    _rigidbody.useGravity = true;
                }
            }
            
        }
    }
}