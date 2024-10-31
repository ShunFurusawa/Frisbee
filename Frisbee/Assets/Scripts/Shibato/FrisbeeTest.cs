using UnityEngine;

namespace Shibato
{
    public class FrisbeeTest : MonoBehaviour
    {
        [SerializeField] [JapaneseLabel("プレイヤーカメラ")] private GameObject player;
        [SerializeField] [JapaneseLabel("プレイヤーカメラ")] private float speed = 5f;

        [SerializeField] [JapaneseLabel("火のオブジェクト、エフェクト")]
        private GameObject fireGameObject;

        private PlayerCamera _camera;
        private Rigidbody _rb;

        public bool FireElement { get; private set; }

        private void Awake()
        {
            _rb = transform.GetComponent<Rigidbody>();
            _camera = player.gameObject.GetComponent<PlayerCamera>();
        }

        private void Update()
        {
            _rb.velocity = new Vector3(speed, _rb.velocity.y, _rb.velocity.z);
        }

        private void OnTriggerEnter(Collider other)
        {
            switch (other.gameObject.tag)
            {
                case "Item":
                    ItemCollision(other);
                    break;
                case "spines":
                    SpinesCollision();
                    break;
                case "Fire":
                    FireCollision();
                    break;
            }
        }

        private void ItemCollision(Collider item)
        {
            var copiedObject = Instantiate(item.gameObject, transform.position + Vector3.up, Quaternion.identity,
                transform);
            copiedObject.transform.localScale = item.transform.localScale;
            Destroy(item.gameObject);
        }

        private void SpinesCollision()
        {
            _camera.MyDestroyed();
            Destroyanimetion();
        }

        private void FireCollision()
        {
            if (!FireElement)
            {
                Instantiate(fireGameObject, transform.position, Quaternion.identity, transform);
                FireElement = true;
            }
        }

        private void Destroyanimetion()
        {
            Invoke("camera.RemoveCamera", 1);
            //camera.RemoveCamera();
            Destroy(gameObject, 2f);
        }
    }
}