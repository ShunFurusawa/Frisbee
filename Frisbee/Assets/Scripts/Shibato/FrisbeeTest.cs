using System;
using UnityEngine;

namespace Shibato
{
    public class FrisbeeTest:MonoBehaviour
    {
        private Rigidbody rb;
        public GameObject Player;
        private PlayerCamera camera;
        public float speed = 5f;
        private void Start()
        {
            rb = transform.GetComponent<Rigidbody>();
            camera = Player.gameObject.GetComponent<PlayerCamera>();
        }

        void Update()
        {
            rb.velocity = new Vector3(speed, rb.velocity.y, rb.velocity.z);
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
                
                Destroyanimetion();
            }
        }
        private void Destroyanimetion()
        {
            Invoke("camera.RemoveCamera",1);
            //camera.RemoveCamera();
            Destroy(gameObject,2f);
        }
    }
}