using UnityEngine;

namespace Shibato
{
    public class FrisbeeTest : MonoBehaviour
    {
        [SerializeField] [JapaneseLabel("プレイヤーカメラ")] private GameObject Player;
        [SerializeField] [JapaneseLabel("プレイヤーカメラ")] private float speed = 5f;

        [SerializeField] [JapaneseLabel("火のオブジェクト、エフェクト")]
        private GameObject fireGameObject;

        private PlayerCamera camera;
        private Rigidbody rb;

        public bool FireElement { get; private set; }

        private void Start()
        {
            rb = transform.GetComponent<Rigidbody>();
            camera = Player.gameObject.GetComponent<PlayerCamera>();
        }

        private void Update()
        {
            rb.velocity = new Vector3(speed, rb.velocity.y, rb.velocity.z);
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
            camera.MyDestroyed();
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