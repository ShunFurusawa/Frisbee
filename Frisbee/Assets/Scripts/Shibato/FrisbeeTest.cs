using System;
using UnityEngine;

namespace Shibato
{
    public class FrisbeeTest:MonoBehaviour
    {
        private Rigidbody rb;
        public GameObject Player;
        private PlayerCamera camera;
        
        private void Start()
        {
            rb = transform.GetComponent<Rigidbody>();
            camera = Player.gameObject.GetComponent<PlayerCamera>();
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
                //カメラを切り替え→壊れるアニメーション
                camera.MyDestroyed();
                
                //Destroy(this.gameObject);
            }
        }
    }
}