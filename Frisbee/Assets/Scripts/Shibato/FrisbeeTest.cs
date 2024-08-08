using System;
using UnityEngine;

namespace Shibato
{
    public class FrisbeeTest:MonoBehaviour
    {
        private Rigidbody rb;

        private void Start()
        {
            rb = transform.GetComponent<Rigidbody>();
        }

        void Update()
        {
            Vector3 force = new Vector3(1, 0, 0);
            rb.AddForce(force, ForceMode.Force);
            
        }

        private void OnTriggerEnter(Collider collision)
        {
            if (collision.gameObject.CompareTag("Item"))
            {
                GameObject copiedObject =
                    Instantiate(collision.gameObject, transform.position + Vector3.up, Quaternion.identity,this.transform);
        
                // コピーしたオブジェクトの見た目を同じにする
                copiedObject.transform.localScale = collision.transform.localScale;
                
                Destroy(collision.gameObject);
            }

            if (collision.gameObject.CompareTag("spines"))
            {
                Destroy(this.gameObject);
            }
        }
    }
}